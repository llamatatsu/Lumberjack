using AutoMapper;

namespace Lumberjack.API.Profiles
{
    public class FileProfile: Profile
    {
        public FileProfile() 
        {
            CreateMap<Entities.File, Models.FileDto>();
            CreateMap<Models.FileForCreationDto, Entities.File>();
        }
    }
}
