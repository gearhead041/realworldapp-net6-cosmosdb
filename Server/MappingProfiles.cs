using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace Server;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        //ShouldMapProperty = prop => prop !=null && prop.GetValue(prop) != null;
        CreateMap<User, ProfileDto>()
            .ForMember(u => u.Following, opt => opt.Ignore());
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")))
            .ForMember(a => a.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")));
        CreateMap<CreateArticleDto, Article>();
        CreateMap<UpdateArticleDto,Article>()
            .ForAllMembers(opt => opt.Condition((updateArticleDto,article,srcMember,destMember) => srcMember != null));
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")))
            .ForMember(a => a.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")))
            .ReverseMap();
        CreateMap<CommentCreateDto,Comment>();
        CreateMap<UserDto, User>().ReverseMap()
            .ForMember(u => u.Token, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<UserUpdate, User>().ReverseMap();
        CreateMap<User, Author>().ReverseMap();
        CreateMap<Author, ProfileDto>().ReverseMap();
    }
}