using AutoMapper;
using NFLWeeklyPicksAPI.Models.Entities;

namespace NFLWeeklyPicksAPI.Models.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}