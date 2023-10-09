namespace Entities.Models;

public record User
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public string Bio { get; set; }
    //image url?
    public string Image { get; set; }
    public IEnumerable<User> Following { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
}



