using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Behaviors;
using Scribble.Content.Infrastructure;
using Scribble.Content.Infrastructure.Interceptors;
using Scribble.Content.Infrastructure.Repositories;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Presentation.Services;
using Scribble.Content.Web.Options;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Web.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(configurePolicy =>
            {
                configurePolicy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });

        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Application.AssemblyReference.Assembly);

        services.AddMediatR(Application.AssemblyReference.Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(
            Application.AssemblyReference.Assembly,
            includeInternalTypes: true);

        services.AddTransient<IEntityRepository, EntityRepository>();
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
            
            options.UseNpgsql(configuration.GetConnectionString("Default"))
                .AddInterceptors(auditableInterceptor);
        });

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly);

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddScoped<ILinkService, LinkService>();
        services.AddHttpContextAccessor();

        return services;
    }
    
    public static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection("Identity")
            .Get<IdentityConfiguration>();

        if (config is null)
            throw new ArgumentNullException(nameof(config), 
                "Identity configuration section was not found in the appsettings.json file. Add this configuration and try again");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(config.Authority);
                options.AddAudiences(config.Audience);

                options.UseIntrospection()
                    .SetClientId(config.ClientId)
                    .SetClientSecret(config.ClientSecret);

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(DefaultPolicies.AdministratorOnly.Name, DefaultPolicies.AdministratorOnly.Build());
            options.AddPolicy(DefaultPolicies.AllButUser.Name, DefaultPolicies.AllButUser.Build());
            options.AddPolicy(DefaultPolicies.All.Name, DefaultPolicies.All.Build());
        });

        return services;
    }
    
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("MessageBrokerHostOptions");

        if (section == null)
            throw new ArgumentNullException(nameof(section),
                "'MessageBrokerOptions' section is undefined in the appsettings.json file");

        var brokerOptions = section.Get<RabbitMqConfiguration>();

        if (brokerOptions == null)
            throw new ArgumentNullException(nameof(brokerOptions),
                $"Cannot bind the options from appsettings.json file to '{nameof(RabbitMqConfiguration)}' class");

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();
            
            configurator.UsingRabbitMq((context, config) =>
            {
                config.Host(brokerOptions.Host, brokerOptions.VirtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(brokerOptions.Username);
                    hostConfigurator.Password(brokerOptions.Password);
                });

                config.ConfigureEndpoints(context);
            });
        });

        services.AddOptions<MassTransitHostOptions>()
            .Configure(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout = TimeSpan.FromSeconds(10);
                options.StopTimeout = TimeSpan.FromSeconds(30);
            });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Scribble.Content API",
                Description = "An ASP.NET Core Web API for managing blog content."
            });
            
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                        TokenUrl = new Uri("https://localhost:5001/connect/token"),
                        Scopes =
                        {
                            { "openid", "User Id" },
                            { "email", "User Email" },
                            { "profile", "User Profile" },
                            { "scrbl-content-api", "Default scope" }
                        }
                    }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "OAuth2"
                        },
                        Scheme = "OAuth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

            var xmlFilename = $"{Presentation.AssemblyReference.Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}