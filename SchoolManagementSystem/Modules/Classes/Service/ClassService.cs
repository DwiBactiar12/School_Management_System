using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using static SchoolManagementSystem.Modules.Classes.Dtos.ClassDtos;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Classes.Service
{
	public interface ICalssService
	{
		Task<ApiResponse<PaginatedResult<ClassDto>>> GetAllClasssAsync(PaginationParameters parameters);
		Task<ApiResponse<ClassDto>> CreateClassAsync(ClassCreateDto createClassDto);
		Task<ApiResponse<ClassSummaryDto>> GetClassByIdAsync(Guid id);
		Task<ApiResponse<ClassDto>> AssignTeacherAsync(Guid id, AssignTeacherDto assignTeacherDto);
	}
	public class ClassService : ICalssService
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly IClassRepository _classRepository;
		private readonly IMapper _mapper;
		public ClassService(IClassRepository classRepository, ITeacherRepository teacherRepository, IMapper mapper)
		{
			_mapper = mapper;
			_classRepository= classRepository;
			_teacherRepository = teacherRepository;
		}

		public async Task<ApiResponse<PaginatedResult<ClassDto>>> GetAllClasssAsync(PaginationParameters parameters)
		{
			try
			{
				var students = await _classRepository.GetAllAsync(parameters);
				var studentDtos = _mapper.Map<List<ClassDto>>(students.Data);

				var result = new PaginatedResult<ClassDto>(
					studentDtos,
					students.TotalCount,
					students.Page,
					students.PageSize);

				return ApiResponse<PaginatedResult<ClassDto>>.SuccessResponse(result, "Class retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<PaginatedResult<ClassDto>>.ErrorResponse($"Error retrieving class: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ClassDto>> CreateClassAsync(ClassCreateDto createClassDto)
		{
			try
			{
				// Check if student ID already exists
				if (await _classRepository.ExistsByClassCodeIdAsync(createClassDto.ClassCode))
				{
					return ApiResponse<ClassDto>.ErrorResponse("Class ID already exists");
				}

				var student = _mapper.Map<Class>(createClassDto);
				var createdStudent = await _classRepository.CreateAsync(student);
				var studentDto = _mapper.Map<ClassDto>(createdStudent);

				return ApiResponse<ClassDto>.SuccessResponse(studentDto, "Class created successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<ClassDto>.ErrorResponse($"Error creating class: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ClassSummaryDto>> GetClassByIdAsync(Guid id)
		{
			try
			{
				var classes = await _classRepository.GetByIdAsync(id);
				if (classes == null)
				{
					return ApiResponse<ClassSummaryDto>.ErrorResponse("Class not found");
				}

				var classesDto = _mapper.Map<ClassSummaryDto>(classes);
				return ApiResponse<ClassSummaryDto>.SuccessResponse(classesDto, "Class retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<ClassSummaryDto>.ErrorResponse($"Error retrieving class: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ClassDto>> AssignTeacherAsync(Guid id, AssignTeacherDto assignTeacherDto)
		{
			try
			{
				var existingClass = await _classRepository.GetByIdAsync(id);
				if (existingClass == null)
				{
					return ApiResponse<ClassDto>.ErrorResponse("Class not found");
				}

				if (assignTeacherDto.TeacherId.HasValue)
				{
					var teacher = await _teacherRepository.GetByIdAsync(assignTeacherDto.TeacherId.Value);
					if (teacher == null)
					{
						return ApiResponse<ClassDto>.ErrorResponse("Teacher not found");
					}
				}

				// Update TeacherId pada kelas
				existingClass.TeacherId = assignTeacherDto.TeacherId;

				var updatedClass = await _classRepository.UpdateAsync(existingClass);
				var classDto = _mapper.Map<ClassDto>(updatedClass);

				return ApiResponse<ClassDto>.SuccessResponse(classDto, "Class updated successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<ClassDto>.ErrorResponse($"Error updating class: {ex.Message}");
			}
		}

	}
}
