
namespace Entities.Models;

public class User
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string Bio {  get; set; }
    //image url?
    public string Image { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
}
