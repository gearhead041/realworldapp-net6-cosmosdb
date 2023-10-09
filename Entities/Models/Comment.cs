namespace Entities.Models;

public class Comment
{
    public string Id { get; set; }
    public string Body { get; set; }
    public User Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
}