
namespace Entities.Dtos;

public record ArticleDto
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public string[] TagList { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set;}
    public bool Favorited { get; set; }
    public int FavouritesCount { get; set; }
    public ProfileDto Author { get; set; }
}
