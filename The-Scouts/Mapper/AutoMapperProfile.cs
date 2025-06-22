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
           
            CreateMap<ApplicationDto, Application>();
            
            CreateMap<ContactMessageDto, ContactMessage>();
        }
    }
}