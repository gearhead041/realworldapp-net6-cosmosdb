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
        CreateMap<UserDto, User>().ReverseMap()
            .ForMember(u => u.Token, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>().ReverseMap();
    }
}