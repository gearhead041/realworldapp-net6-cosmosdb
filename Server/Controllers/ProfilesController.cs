using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public ProfilesController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(string userName)
    {
        var profile = await serviceManager.UserService.GetProfile(userName);
        if (profile == null)
            return NotFound("User not found");
        return Ok(new { profile });
    }

    [HttpPost("{userName}/follow")]
    public async Task<ActionResult<ProfileDto>> FollowUser(string userName)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var user = await serviceManager.UserService.GetUser(token);
        if (user == null)
            return Unauthorized();
        var profile = await serviceManager.UserService.FollowUser(user.UserName, userName);
        if (profile == null)
            return NotFound("User not found");
        return Ok(new { profile });
    }

    [HttpDelete("{userName}/unfollow")]
    public async Task<ActionResult<ProfileDto>> UnfollowUser(string userName)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var user = await serviceManager.UserService.GetUser(token);
        if (user == null)
            return Unauthorized();
        var profile = await serviceManager.UserService.UnfollowUser(user.UserName, userName);
        if (profile == null)
            return NotFound("User not found");
        return Ok(new { profile });
    }
}
