using AutoMapper;

namespace Lumberjack.API.Profiles
{
    public class SegmentProfile : Profile
    {
        public SegmentProfile()
        {
            CreateMap<Entities.Segment, Models.SegmentDto>();
            CreateMap<Models.SegmentForCreationDto, Entities.Segment>();
            CreateMap<Entities.Segment, Models.SegmentForCreationDto>();
        }
    }
}
