namespace Entities.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Body { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
}