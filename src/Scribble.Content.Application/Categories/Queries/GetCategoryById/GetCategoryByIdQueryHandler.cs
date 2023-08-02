using AutoMapper;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Categories.Queries.GetCategoryById;

internal class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetCategoryByIdQueryHandler(IMapper mapper, 
        IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken token)
    {
        var category = await _repository.GetAsync<Category, CategoryId>(query.CategoryId, token)
            .ConfigureAwait(false);

        if (category is null)
            return Result.Failure<CategoryResponse>(Errors.Category.NotExistsById);

        var response = _mapper.Map<CategoryResponse>(category);
        
        AddLinksForCategories(response);

        return Result.Success(response);
    }

    private void AddLinksForCategories(CategoryResponse response)
    {
        response.Links.Add(
            _linkService.Generate(
                "GetCategoryById",
                new { categoryId = response.Id }, 
                "self", 
                "GET"));
        
        response.Links.Add(
            _linkService.Generate(
                "UpdateCategory",
                new { categoryId = response.Id },
                "update-category",
                "PUT"));
        
        response.Links.Add(
            _linkService.Generate(
                "RemoveCategory",
                new { categoryId = response.Id },
                "delete-category",
                "DELETE"));
    }
}