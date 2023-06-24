using AutoMapper;
using Portfolio.Backend.Csharp.Models.Entities;
using Portfolio.Backend.Csharp.Models.Requests;
using Portfolio.Backend.Csharp.Models.Responses;

namespace Portfolio.Backend.Csharp.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<User, UserResponse>();
            CreateMap<LoginRequest, Login>();
        }
    }
}
