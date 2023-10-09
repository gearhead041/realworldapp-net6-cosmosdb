
namespace Entities.Dtos;

public record CommentDto
{
    public string Id { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public string Body { get; set; }
    public ProfileDto Author { get; set; }
}
