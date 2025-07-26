using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Students.Service;
using SchoolManagementSystem.Modules.Teachers.Entities;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using static SchoolManagementSystem.Modules.Teachers.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Teachers.Service
{
	public interface ITeacherService
	{
		Task<ApiResponse<PaginatedResult<TeacherDto>>> GetAllTeacherAsync(PaginationParameters parameters);
		Task<ApiResponse<TeacherDto>> GetTeacherByIdAsync(Guid id);
		Task<ApiResponse<TeacherDto>> CreateTeacherAsync(CreateTeacherDto createTeacherDto);
		Task<ApiResponse<TeacherDto>> UpdateTeacherAsync(Guid id, UpdateTeacherDto updateTeacherDto);
		Task<ApiResponse<object>> DeleteTeacherAsync(Guid id);
	}
	public class TeacherService : ITeacherService
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly IMapper _mapper;

		public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
		{
			_teacherRepository = teacherRepository;
			_mapper = mapper;
		}

		public async Task<ApiResponse<PaginatedResult<TeacherDto>>> GetAllTeacherAsync(PaginationParameters parameters)
		{
			try
			{
				var teachers = await _teacherRepository.GetAllAsync(parameters);
				var teacherDtos = _mapper.Map<List<TeacherDto>>(teachers.Data);

				var result = new PaginatedResult<TeacherDto>(
					teacherDtos,
					teachers.TotalCount,
					teachers.Page,
					teachers.PageSize);

				return ApiResponse<PaginatedResult<TeacherDto>>.SuccessResponse(result, "Teachers retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<PaginatedResult<TeacherDto>>.ErrorResponse($"Error retrieving teachers: {ex.Message}");
			}
		}

		public async Task<ApiResponse<TeacherDto>> GetTeacherByIdAsync(Guid id)
		{
			try
			{
				var teacher = await _teacherRepository.GetByIdAsync(id);
				if (teacher == null)
				{
					return ApiResponse<TeacherDto>.ErrorResponse("Teacher not found");
				}

				var teacherDto = _mapper.Map<TeacherDto>(teacher);
				return ApiResponse<TeacherDto>.SuccessResponse(teacherDto, "Teacher retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<TeacherDto>.ErrorResponse($"Error retrieving teacher: {ex.Message}");
			}
		}

		public async Task<ApiResponse<TeacherDto>> CreateTeacherAsync(CreateTeacherDto createTeacherDto)
		{
			try
			{
				if (await _teacherRepository.ExistsByTeacherIdAsync(createTeacherDto.TeacherId))
				{
					return ApiResponse<TeacherDto>.ErrorResponse("Teacher ID already exists");
				}

				if (await _teacherRepository.ExistsByEmailAsync(createTeacherDto.Email))
				{
					return ApiResponse<TeacherDto>.ErrorResponse("Email already exists");
				}

				var teacher = _mapper.Map<Teacher>(createTeacherDto);
				var createdTeacher = await _teacherRepository.CreateAsync(teacher);
				var teacherDto = _mapper.Map<TeacherDto>(createdTeacher);

				return ApiResponse<TeacherDto>.SuccessResponse(teacherDto, "Teacher created successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<TeacherDto>.ErrorResponse($"Error creating teacher: {ex.Message}");
			}
		}

		public async Task<ApiResponse<TeacherDto>> UpdateTeacherAsync(Guid id, UpdateTeacherDto updateTeacherDto)
		{
			try
			{
				var existingTeacher = await _teacherRepository.GetByIdAsync(id);
				if (existingTeacher == null)
				{
					return ApiResponse<TeacherDto>.ErrorResponse("Teacher not found");
				}

				if (!string.IsNullOrWhiteSpace(updateTeacherDto.Email))
				{
					var teacherWithEmail = await _teacherRepository.GetByEmailAsync(updateTeacherDto.Email);
					if (teacherWithEmail != null && teacherWithEmail.Id != id)
					{
						return ApiResponse<TeacherDto>.ErrorResponse("Email already exists for another student");
					}

				}

				_mapper.Map(updateTeacherDto, existingTeacher);
				var updatedTeacher = await _teacherRepository.UpdateAsync(existingTeacher);
				var teacherDto = _mapper.Map<TeacherDto>(updatedTeacher);

				return ApiResponse<TeacherDto>.SuccessResponse(teacherDto, "Teacher updated successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<TeacherDto>.ErrorResponse($"Error updating teacher: {ex.Message}");
			}
		}

		public async Task<ApiResponse<object>> DeleteTeacherAsync(Guid id)
		{
			try
			{
				if (!await _teacherRepository.ExistsAsync(id))
				{
					return ApiResponse<object>.ErrorResponse("Teacher not found");
				}

				var deleted = await _teacherRepository.DeleteAsync(id);
				if (!deleted)
				{
					return ApiResponse<object>.ErrorResponse("Failed to delete teacher");
				}

				return ApiResponse<object>.SuccessResponse("Teacher deleted successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<object>.ErrorResponse($"Error deleting teacher: {ex.Message}");
			}
		}

	}
}
