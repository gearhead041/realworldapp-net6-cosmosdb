
namespace Entities.Models;

public class Article
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public string[] TagList { get; set; } = Array.Empty<string>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int FavoritesCount { get; set; }
    public Author Author { get; set; }
    public string[] CommentIds { get; set; } = Array.Empty<string>();
}

public class Author
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}
