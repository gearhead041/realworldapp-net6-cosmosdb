using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace Server;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, ProfileDto>()
            .ForMember(u => u.Following, opt => opt.Ignore());
        CreateMap<Article,ArticleDto>().ReverseMap();
        CreateMap<CreateArticleDto, Article>();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap()
            .ForMember(u => u.Token, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<Author, User>().ReverseMap();
        CreateMap<Author,ProfileDto>().ReverseMap();
    }
}