namespace Entities.Models;

public record User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public string[] FollowingIds { get; set; } = Array.Empty<string>();
    public string[] FavouritedArticlesSlugs { get; set; } = Array.Empty<string>();
    public IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
}



