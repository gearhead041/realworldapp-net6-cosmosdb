using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;

namespace Server.Controllers;

[Route("api")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public UsersController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpPost("users/login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] UserForAuthRequestDto userForAuth)
    {

        (bool result, UserDto userReturn) = await serviceManager.UserService.AuthenticateUser(userForAuth.user);
        if (result == false)
            return Unauthorized("Wrong password");
        if (userReturn == null)
            return NotFound("User with email not found");
        return Ok(new { user = userReturn });

    }

    [HttpPost("users")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserRequestDto createUserDto)
    {
         (bool result, UserDto userReturn) = await serviceManager.UserService.CreateUser(createUserDto.user);
        if (!result)
            return BadRequest("Username or Email exists already");
        return Ok(new { user = userReturn });
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var user = await serviceManager.UserService.GetUser(jwtToken);
        if (user == null)
            return NotFound(user);
        return Ok( new { user });
    }

    [Authorize]
    [HttpPut("user")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserUpdateDto userUpdate)
    {
        (bool result, UserDto user) = await serviceManager.UserService.UpdateUser(userUpdate.User);
        if (!result)
            return BadRequest("username taken");
        if (user == null)
            return BadRequest("user not found");
        return Ok(new { user });
    }
}
