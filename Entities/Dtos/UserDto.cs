
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public record UserDto
{
    public string Username { get; set; }
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid Email address")]
    public string Email { get; set; }
    public string Token { get; set; }
    public string Bio {  get; set; }
    public string? Image { get; set; }
}

public record UserUpdate
{
    public string? Username { get; set; }
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid Email address")]
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
}


public record UserUpdateDto
{
    public UserUpdate User { get; set; }
}

public record UserForAuthDto
{
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid Email address")]
    public string Email { get; set; }
    public string Password { get; set; }
}

public record UserForAuthRequestDto
{
    public UserForAuthDto User {  get; set; }
}



public record CreateUserDto
{
    public string Username { get; set; }
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be a valid Email address")]
    public string Email { get; set; }
    public string Password { get; set; }
}

public record CreateUserRequestDto
{
    public CreateUserDto User { get; set; }
}
