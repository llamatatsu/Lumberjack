using AutoMapper;

namespace Lumberjack.API.Profiles
{
    public class UserApplicationMapProfile : Profile
    {
        public UserApplicationMapProfile() 
        {
            CreateMap<Entities.UserApplicationMap,Models.UserAppplicationMapForUpdateDto>();
        }
    }
}
