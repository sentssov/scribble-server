namespace Scribble.Content.Web.Options;

public class IdentityConfiguration
{
    public string Authority { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}