using AutoMapper;

namespace Lumberjack.API.Profiles
{
    public class ApplicationDto : Profile
    {
        public ApplicationDto()
        {
            CreateMap<Entities.Application, Models.ApplicationDto>();
        }
    }
}
