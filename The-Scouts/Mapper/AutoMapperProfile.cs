using AutoMapper;
using The_Scouts.DTOs;
using The_Scouts.Models;

namespace MyProjectAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<ContactMessage, ContactMessageDto>().ReverseMap();
           
            CreateMap<ApplicationDto, Application>()
                .ForMember(dest => dest.ResumePath, opt => opt.Ignore()) // Set manually after saving the file
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ApplicationStatus.Submitted))
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ✅ Add reverse mapping so AutoMapper can map Application ➝ ApplicationDto
            CreateMap<Application, ApplicationDto>();
        }
    }
}