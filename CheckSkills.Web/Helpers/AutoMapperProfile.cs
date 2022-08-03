using AutoMapper;
using CheckSkills.Web.Dtos.Student;
using CheckSkills.Web.Models;

namespace CheckSkills.Web.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, GetRequestStudentDto>();
            CreateMap<PostRequestStudentDto, Student>();
            CreateMap<GetRequestStudentDto, Student>();
            CreateMap<UpdateStudentRequestDto, Student>();
            CreateMap<Student, UpdateStudentRequestDto>();
            CreateMap<Student, AuthRequestStudentDto>();
            CreateMap<Student, VerifyAccountRequestDto>();
        }
    }
}
