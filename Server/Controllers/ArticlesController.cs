using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public ArticlesController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(string? tags, string? author,
        string? favorited, int limit = 0, int offset = 0)
    {
        var articles = await serviceManager.ArticleService.GetArticles(tags, author, favorited, limit, offset);
    }

    [Authorize]
    [HttpGet("feed")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticleFeed(int limit = 20, int offset = 0)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var articles = await serviceManager.ArticleService.GetUserFeed(jwtToken);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ArticleDto>> GetArticle(string slug)
    {
        var article = await serviceManager.ArticleService.GetArticle(slug);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ArticleDto>> CreateArticle([FromBody] CreateArticleDto articleToCreate)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var article = await serviceManager.ArticleService.CreateArticle(jwtToken, articleToCreate);
    }

    [Authorize]
    [HttpDelete("{slug}")]
    public async Task<ActionResult> DeleteArticle(string slug)
    {
        await serviceManager.ArticleService.DeleteArticle(slug);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{slug}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CommentDto commentToCreate, string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var comment = await serviceManager.ArticleService.AddComment(jwtToken, slug, commentToCreate)
    }

    [Authorize]
    [HttpDelete("{slug}/comments/{commentId}")]
    public async Task<ActionResult> DeleteComment(string slug, Guid commentId)
    {
        await serviceManager.ArticleService.DeleteComment(slug, commentId);
        return NoContent();
    }

    //HACK should create prop for liked articles on user
    [Authorize]
    [HttpPost("{slug}/favorite")]
    public async Task<ActionResult<ArticleDto>> FavoriteArticle(string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var article = await serviceManager.ArticleService.FavoriteArticle(jwtToken, slug);
    }

    [Authorize]
    [HttpDelete("{slug}/favorite")]
    public async Task<ActionResult<ArticleDto>> UnfavoriteArticle(string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var article = await serviceManager.ArticleService.UnfavoriteArticle(jwtToken, slug);
    }
}
