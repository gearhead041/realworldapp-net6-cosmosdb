using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("{userName:string}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(string userName)
    {

    }

    [HttpPost("{userName:string}/follow")]
    public async Task<ActionResult<ProfileDto>> FollowUser(string userName)
    {

    }

    [HttpDelete("{userName:string}/unfollow")]
    public async Task<ActionResult<ProfileDto>> UnfollowUser(string userName)
    {

    }
}
