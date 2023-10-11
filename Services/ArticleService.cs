using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;
using Entities.Models;
//using XAct;

namespace Services;

public class ArticleService : IArticleService
{
    private readonly IRepositoryManager repositoryManager;
    private readonly IMapper mapper;

    public ArticleService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
    }

    public async Task<CommentDto> AddComment(string token, string slug, CommentCreateDto commentToCreate)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        if (user == null)
            return null;

        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (article == null)
            return null;

        var comment = mapper.Map<Comment>(commentToCreate);
        comment.AuthorId = user.Id;
        comment.CreatedAt = DateTime.Now.ToUniversalTime();
        comment.UpdatedAt = DateTime.Now.ToUniversalTime();
        repositoryManager.CommentRepository.CreateComment(comment);
        await repositoryManager.Save();

        article.CommentIds = article.CommentIds.Append(comment.Id.ToString()).ToArray();
        var commentToReturn = mapper.Map<CommentDto>(comment);
        commentToReturn.Author = mapper.Map<ProfileDto>(user);
        return commentToReturn;
    }

    public async Task<(bool, ArticleDto)> CreateArticle(string token, CreateArticleDto createArticle)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        if (user == null)
            return (false, null);
        var article = mapper.Map<Article>(createArticle);
        article.Author = mapper.Map<Author>(user);
        string slug = createArticle.Title.Replace(" ", "-");
        var articles = await repositoryManager.ArticleRepository.GetAllArticles(false, null);
        var articleSlugs = articles.Select(a => a.Slug);
        int i = 1;
        while (articleSlugs.Contains(slug))
        {
            slug += $"-{i}";
            i++;
        }
        article.Slug = slug;
        article.CreatedAt = DateTime.Now.ToUniversalTime();
        article.UpdatedAt = DateTime.Now.ToUniversalTime();
        repositoryManager.ArticleRepository.CreateArticle(article);
        await repositoryManager.Save();
        var returnArticle = mapper.Map<ArticleDto>(article);
        returnArticle.FavoritesCount = 0;
        return (true, returnArticle);
    }



    public async Task<bool> DeleteArticle(string slug)
    {
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (article == null)
            return false;
        repositoryManager.ArticleRepository.DeleteArticle(article);
        await repositoryManager.Save();
        return true;
    }

    public async Task<bool> DeleteComment(string slug, Guid commentId)
    {
        var comment = await repositoryManager.CommentRepository.GetComment(commentId, true, null);
        if (comment == null)
            return false;

        repositoryManager.CommentRepository.DeleteComment(comment);
        await repositoryManager.Save();
        return true;
    }


    public async Task<ArticleDto> GetArticle(string slug)
    {
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, false, null);
        var articleToReturn = mapper.Map<ArticleDto>(article);
        return articleToReturn;
    }

    public async Task<IEnumerable<ArticleDto>> GetArticles(string? tag, string? author, string? favorited,
        int limit, int offset)
    {
        IEnumerable<Article> articles;
        articles = await repositoryManager.ArticleRepository.GetAllArticles(false, null);
        if (offset > 0)
            articles = articles.Skip(offset);
        articles = articles.Take(limit).ToList();

        if (tag != null)
            articles = articles.Where(a => a.TagList.Contains(tag));
        if (author != null)
        {
            articles = articles.Where(a => a.Author.UserName == author);
        }
        IEnumerable<ArticleDto> articlesToReturn = mapper.Map<IEnumerable<ArticleDto>>(articles);
        if (favorited != null)
        {
            var user = await repositoryManager.UserRepository.GetUserByName(favorited, false, null);
            if (user == null)
                articles = Array.Empty<Article>();
            else
                articles = articles.Where(a => user.FavouritedArticlesSlugs
                    .Contains(a.Slug)).ToList();
            foreach (var article in articlesToReturn)
            {
                article.Favorited = true;
            };
            articlesToReturn = mapper.Map<IEnumerable<ArticleDto>>(articles);
        }
        return articlesToReturn;
    }

    public async Task<IEnumerable<ArticleDto>> GetUserFeed(string token, int limit, int offset)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        IEnumerable<Article> articles;
        articles = await repositoryManager.ArticleRepository.GetAllArticles(false, null);
        articles = articles.Where(a => user.FollowingIds
            .Contains(a.Author.Id.ToString())).Skip(offset).Take(limit);
        var articlesToReturn = mapper.Map<IEnumerable<ArticleDto>>(articles);
        foreach (var article in articlesToReturn)
        {
            article.Author.Following = true;
        }
        return articlesToReturn;
    }

    public async Task<ArticleDto> FavoriteArticle(string token, string slug)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, true, null);
        if (user.FavouritedArticlesSlugs.Contains(slug))
            return await GetArticle(slug);
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (user == null || article == null)
            return null;
        article.FavoritesCount += 1;
        user.FavouritedArticlesSlugs = user.FavouritedArticlesSlugs
            .Append(slug).ToArray();
        await repositoryManager.Save();
        var articleToReturn = await GetArticle(slug);
        articleToReturn.Favorited = true;
        return articleToReturn;
    }

    public async Task<ArticleDto> UnfavoriteArticle(string token, string slug)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, true, null);
        if (!user.FavouritedArticlesSlugs.Contains(slug))
            return await GetArticle(slug);
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (user == null || article == null)
            return null;
        user.FavouritedArticlesSlugs = user.FavouritedArticlesSlugs
            .Where(s => s != slug).ToArray();
        article.FavoritesCount -= 1;
        await repositoryManager.Save();
        return await GetArticle(slug);
    }

    public async Task<IEnumerable<string>> GetTags()
    {
        var articles = await repositoryManager.ArticleRepository.GetAllArticles(false, null);
        var tags = articles.Select(a => a.TagList);
        var tagsToReturn = tags.SelectMany(s => s).Distinct();
        return tagsToReturn;
    }

    public async Task<ArticleDto> UpdateArticle(string slug, UpdateArticleDto articleDto)
    {
        var articles = await repositoryManager.ArticleRepository.GetAllArticles(true, null);
        var article = articles.Where(a => a.Slug == slug).FirstOrDefault();
        if (article == null)
            return null;
        mapper.Map(articleDto, article);
        var newSlug = article.Title.Replace(" ", "-");
        int i = 0;
        while (articles.Select(a => a.Slug).Contains(article.Title))
        {
            newSlug += $"-{i}";
            i++;
        }
        article.UpdatedAt = DateTime.Now.ToUniversalTime();
        await repositoryManager.Save();
        return mapper.Map<ArticleDto>(article);

    }

    public async Task<IEnumerable<CommentDto>> GetCommentsForArticle(string slug)
    {
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (article == null)
            return null;
        var comments = await repositoryManager.CommentRepository.GetAllComments(false,null);
        comments = comments.Where(c => article.CommentIds.Contains(c.Id.ToString()));
        var commentsToReturn = mapper.Map<IEnumerable<CommentDto>>(comments);
        return commentsToReturn;
    }
}