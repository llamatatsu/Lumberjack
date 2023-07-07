using AutoMapper;

namespace Lumberjack.API.Profiles
{
    public class MessageProfile : Profile 
    {
        public MessageProfile()
        {
            CreateMap<Entities.Message, Models.MessageWithSublistsDto>();
            CreateMap<Entities.Message, Models.MessageWithFilesDto>();
            CreateMap<Entities.Message, Models.MessageWithSegmentsDto>();
            CreateMap<Entities.Message, Models.MessageDto>();
            CreateMap<Models.MessageWithFilesForCreationDto, Entities.Message>();
            CreateMap<Models.MessageWithSegmentsForCreationDto, Entities.Message>();
            CreateMap<Models.MessageForUpdateDto, Entities.Message>();
            CreateMap<Entities.Message, Models.MessageForUpdateDto>();
        }
    }
}
