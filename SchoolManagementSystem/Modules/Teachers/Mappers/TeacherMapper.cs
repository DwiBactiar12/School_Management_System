using AutoMapper;
using SchoolManagementSystem.Modules.Teachers.Entities;
using static SchoolManagementSystem.Modules.Teachers.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Teachers.Mappers
{
	public class TeacherMapper : Profile
	{
		public TeacherMapper()
		{
			CreateMap<CreateTeacherDto, Teacher>()
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

			CreateMap<UpdateTeacherDto, Teacher>()
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.TeacherId, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)) ;

			CreateMap<Teacher, TeacherDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

			CreateMap<Teacher, TeacherSummaryDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
		}
	}
}
