using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public TagsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetTags()
    {
        var tags = await serviceManager.ArticleService.GetTags();
        return Ok( new { tags });
    }
}
