using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<UserDto>> Login([FromBody] UserForAuthDto user)
    {

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {

    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {

    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserDto user)
    {

    }
}
