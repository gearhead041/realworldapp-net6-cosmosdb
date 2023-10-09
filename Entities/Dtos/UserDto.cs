
namespace Entities.Dtos;

public record UserDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string UserName { get; set; }
    public string Bio {  get; set; }
    public string? Image { get; set; }
}
