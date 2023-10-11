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
        string? favorited, int limit = 20, int offset = 0)
    {
        var articles = await serviceManager.ArticleService
            .GetArticles(tags, author, favorited, limit, offset);

        return Ok(new { articles, articlesCount = articles?.Count() });
    }

    [Authorize]
    [HttpGet("feed")]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticleFeed(int limit = 20, int offset = 0)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var articles = await serviceManager.ArticleService.GetUserFeed(jwtToken,limit,offset);
        return Ok(new { articles, articlesCount = articles.Count() });
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ArticleDto>> GetArticle(string slug)
    {
        var article = await serviceManager.ArticleService.GetArticle(slug);
        if (article == null)
            return NotFound();
        return Ok(new { article });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ArticleDto>> CreateArticle([FromBody] CreateArticleRequestDto articleToCreate)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        (bool result, ArticleDto article) = await serviceManager.ArticleService
            .CreateArticle(jwtToken, articleToCreate.Article);
        if (!result)
            return BadRequest("user not found");
        return Ok(new { article });
    }

    [Authorize]
    [HttpDelete("{slug}")]
    public async Task<ActionResult> DeleteArticle(string slug)
    {
        var result = await serviceManager.ArticleService.DeleteArticle(slug);
        if (result == false)
            return NotFound();
        return NoContent();
    }

    [HttpGet("{slug}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(string slug)
    {
        var comments = await serviceManager.ArticleService.GetCommentsForArticle(slug);
        if (comments == null)
            return BadRequest("Article not found");
        return Ok(new { comments });
    }

    [Authorize]
    [HttpPost("{slug}/comments")]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CommentDataDto commentToCreate, string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var comment = await serviceManager.ArticleService.AddComment(jwtToken, slug, commentToCreate.Comment);
        if (comment == null)
            return BadRequest("user or article not found");
        return Ok(new { comment });
    }

    [Authorize]
    [HttpDelete("{slug}/comments/{commentId}")]
    public async Task<ActionResult> DeleteComment(string slug, Guid commentId)
    {

        var result = await serviceManager.ArticleService.DeleteComment(slug, commentId);
        if (!result)
            return BadRequest("comment not found");
        return NoContent();
    }

    [Authorize]
    [HttpPost("{slug}/favorite")]
    public async Task<ActionResult<ArticleDto>> FavoriteArticle(string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        var article = await serviceManager.ArticleService.FavoriteArticle(jwtToken, slug);
        if (article == null)
            return BadRequest("user not found");
        return Ok(new { article });
    }

    [Authorize]
    [HttpDelete("{slug}/favorite")]
    public async Task<ActionResult<ArticleDto>> UnfavoriteArticle(string slug)
    {
        string jwtToken = HttpContext.Request.Headers["Authorization"];
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);
        jwtToken = jwtToken.Replace("Token ", string.Empty);
        var article = await serviceManager.ArticleService.UnfavoriteArticle(jwtToken, slug);
        if (article == null)
            return BadRequest("user not found");
        return Ok(new { article });
    }

    [Authorize]
    [HttpPut("{slug}")]
    public async Task<ActionResult<ArticleDto>> UpdateArticle(string slug,[FromBody] UpdateArticleRequestDto updateArticle)
    {
        var article = await serviceManager.ArticleService.UpdateArticle(slug, updateArticle.Article);
        if(article == null)
            return NotFound();
        return Ok(new { article });
    }


}
