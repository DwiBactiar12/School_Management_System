using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Enrollments.Repositories;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Teachers.Entities;
using static SchoolManagementSystem.Modules.Enrollments.Dtos.EnrollmentDtos;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;
using static SchoolManagementSystem.Modules.Teachers.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Enrollments.Service
{
	public interface IEnrollmentService
	{
		Task<ApiResponse<EnrollmentDto>> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto);
		Task<ApiResponse<PaginatedResult<EnrollmentDto>>> GetAllEnrollmentsAsync(PaginationParameters parameters);
		Task<ApiResponse<EnrollmentDto>> GetEnrollmentByIdAsync(Guid id);
		Task<ApiResponse<object>> DeleteEnrollmentAsync(Guid id);
	}
	public class EnrollmentService : IEnrollmentService
	{
		private readonly IEnrollmentRepository _enrollmentRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IClassRepository _classRepository;
		private readonly IMapper _mapper;
		public EnrollmentService(IEnrollmentRepository enrollmentRepository, IClassRepository classRepository, IStudentRepository studentRepository, IMapper mapper) {
			_enrollmentRepository = enrollmentRepository;
			_studentRepository = studentRepository;
			_classRepository = classRepository;
			_mapper = mapper;
		}
		public async Task<ApiResponse<EnrollmentDto>> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto)
		{
			try
			{
				var student = await _studentRepository.GetByIdAsync(createEnrollmentDto.StudentId);
				if (student == null)
				{
					return ApiResponse<EnrollmentDto>.ErrorResponse("Student not found");
				}

				var kelas = await _classRepository.GetByIdAsync(createEnrollmentDto.ClassId);
				if (kelas == null)
				{
					return ApiResponse<EnrollmentDto>.ErrorResponse("Class not found");
				}

				if (await _enrollmentRepository.IsDuplicateAsync(createEnrollmentDto.StudentId, createEnrollmentDto.ClassId))
				{
					return ApiResponse<EnrollmentDto>.ErrorResponse("Enrollment already exists in this class");
				}

				var createdEnrollment = await _enrollmentRepository.AddAsync(new Enrollment
				{
					StudentId = createEnrollmentDto.StudentId,
					ClassId = createEnrollmentDto.ClassId,
					EnrolledAt = DateTime.UtcNow
				});
				var enrollmentDto = _mapper.Map<EnrollmentDto>(createdEnrollment);

				return ApiResponse<EnrollmentDto>.SuccessResponse(enrollmentDto, "Enrollment created successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<EnrollmentDto>.ErrorResponse($"Error creating enrollment: {ex.Message}");
			}
		}
		public async Task<ApiResponse<PaginatedResult<EnrollmentDto>>> GetAllEnrollmentsAsync(PaginationParameters parameters)
		{
			try
			{
				var enrollments = await _enrollmentRepository.GetAllAsync(parameters);

				var enrollmentDtos = _mapper.Map<List<EnrollmentDto>>(enrollments.Data);

				var result = new PaginatedResult<EnrollmentDto>(
					enrollmentDtos,
					enrollments.TotalCount,
					enrollments.Page,
					enrollments.PageSize
				);

				return ApiResponse<PaginatedResult<EnrollmentDto>>.SuccessResponse(result, "Enrollments retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<PaginatedResult<EnrollmentDto>>.ErrorResponse($"Error retrieving enrollments: {ex.Message}");
			}
		}

		public async Task<ApiResponse<EnrollmentDto>> GetEnrollmentByIdAsync(Guid id)
		{
			try
			{
				var enrollment = await _enrollmentRepository.GetByIdAsync(id);
				if (enrollment == null)
				{
					return ApiResponse<EnrollmentDto>.ErrorResponse("Enrollment not found");
				}

				var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);
				return ApiResponse<EnrollmentDto>.SuccessResponse(enrollmentDto, "Enrollment retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<EnrollmentDto>.ErrorResponse($"Error retrieving enrollment: {ex.Message}");
			}
		}

		public async Task<ApiResponse<object>> DeleteEnrollmentAsync(Guid id)
		{
			try
			{
				var enrollment = await _enrollmentRepository.GetByIdAsync(id);
				if (enrollment == null)
				{
					return ApiResponse<object>.ErrorResponse("Enrollment not found");
				}

				var deleted = await _enrollmentRepository.DeleteAsync(id);
				if (!deleted)
				{
					return ApiResponse<object>.ErrorResponse("Failed to delete enrollment");
				}

				return ApiResponse<object>.SuccessResponse("Enrollment deleted successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<object>.ErrorResponse($"Error deleting enrollment: {ex.Message}");
			}
		}

	}
}
