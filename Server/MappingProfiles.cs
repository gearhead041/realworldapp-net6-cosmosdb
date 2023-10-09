using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace Server;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ProfileDto, User>().ReverseMap();
        CreateMap<Article,ArticleDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
    }
}