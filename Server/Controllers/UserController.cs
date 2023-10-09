using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public UsersController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] UserForAuthDto userForAuth)
    {

        (bool result, UserDto userReturn) = await serviceManager.UserService.AuthenticateUser(userForAuth);
        if (result == false)
            return Unauthorized("Wrong password");
        if (userReturn == null)
            return NotFound("User with email not found");
        return Ok(new { user = userReturn });

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {
         (bool result, UserDto userReturn) = await serviceManager.UserService.CreateUser(createUserDto);
        if (!result)
            return BadRequest("Email exists already");
        return Ok(new { user = userReturn });
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var user = await serviceManager.UserService.GetUser(token);
        if (user == null)
            return NotFound(user);
        return Ok( new { user });
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserDto userUpdate)
    {
        var user = await serviceManager.UserService.UpdateUser(userUpdate);
        if (user.IsNull())
            return BadRequest("User Not Found");
        return Ok(new { user });
    }
}
