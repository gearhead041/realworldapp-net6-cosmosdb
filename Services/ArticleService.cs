﻿using AutoMapper;
using Contracts.Repository;
using Contracts.Services;
using Entities.Dtos;
using Entities.Models;
//using XAct;

namespace Services;

public class ArticleService : IArticleService
{
    private IRepositoryManager repositoryManager;
    private IMapper mapper;

    public ArticleService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        this.repositoryManager = repositoryManager;
        this.mapper = mapper;
    }

    public async Task<CommentDto> AddComment(string token, string slug, CommentDto commentToCreate)
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
        comment.CreatedAt = DateTime.UtcNow;
        repositoryManager.CommentRepository.CreateComment(comment);
        await repositoryManager.Save();

        article.CommentIds = article.CommentIds.Append(comment.Id.ToString()).ToArray();
        mapper.Map(comment, commentToCreate);
        commentToCreate.Author = mapper.Map<ProfileDto>(user);
        return commentToCreate;
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
        article.CreatedAt = DateTime.UtcNow;
        repositoryManager.ArticleRepository.CreateArticle(article);
        await repositoryManager.Save();
        var returnArticle = mapper.Map<ArticleDto>(article);
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
        var article = await repositoryManager.ArticleRepository.GetArticle(slug, true, null);
        if (article == null)
            return false;
        article.CommentIds = article.CommentIds.Where(id => id != commentId.ToString())
            .ToArray();
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
        int limit, int offset, string token)
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
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);

        if (favorited != null && token != null)
            articles = articles.Where(a => user.FavouritedArticlesSlugs
                .Contains(a.Slug));

        var articlesToReturn = mapper.Map<IEnumerable<ArticleDto>>(articles);
        foreach (var article in articlesToReturn)
        {
            article.Favorited = user.FavouritedArticlesSlugs.Contains(article.Slug);
        };
        return articlesToReturn;
    }

    public async Task<IEnumerable<ArticleDto>> GetUserFeed(string token)
    {
        var email = UserService.GetEmailFromToken(token);
        var user = await repositoryManager.UserRepository.GetUser(email, false, null);
        IEnumerable<Article> articles;
        articles = await repositoryManager.ArticleRepository.GetAllArticles(false, null);
        articles = articles.Where(a => user.FollowingIds
            .Contains(a.Author.Id.ToString()));
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
        article.FavouritesCount += 1;
        user.FavouritedArticlesSlugs = user.FavouritedArticlesSlugs
            .Append(slug).ToArray();
        await repositoryManager.Save();
        return await GetArticle(slug);
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
        article.FavouritesCount -= 1;
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
}