using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Students.Repositories;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Students.Service
{
	public interface IStudentService
	{
		Task<ApiResponse<PaginatedResult<StudentDto>>> GetAllStudentsAsync(PaginationParameters parameters);
		Task<ApiResponse<StudentDto>> GetStudentByIdAsync(Guid id);
		Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentDto createStudentDto);
		Task<ApiResponse<StudentDto>> UpdateStudentAsync(Guid id, UpdateStudentDto updateStudentDto);
		Task<ApiResponse<object>> DeleteStudentAsync(Guid id);
	}
	public class StudentService : IStudentService
	{
		private readonly IStudentRepository _studentRepository;
		private readonly IMapper _mapper;

		public StudentService(IStudentRepository studentRepository, IMapper mapper)
		{
			_studentRepository = studentRepository;
			_mapper = mapper;
		}

		public async Task<ApiResponse<PaginatedResult<StudentDto>>> GetAllStudentsAsync(PaginationParameters parameters)
		{
			try
			{
				var students = await _studentRepository.GetAllAsync(parameters);
				var studentDtos = _mapper.Map<List<StudentDto>>(students.Data);

				var result = new PaginatedResult<StudentDto>(
					studentDtos,
					students.TotalCount,
					students.Page,
					students.PageSize);

				return ApiResponse<PaginatedResult<StudentDto>>.SuccessResponse(result, "Students retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<PaginatedResult<StudentDto>>.ErrorResponse($"Error retrieving students: {ex.Message}");
			}
		}

		public async Task<ApiResponse<StudentDto>> GetStudentByIdAsync(Guid id)
		{
			try
			{
				var student = await _studentRepository.GetByIdAsync(id);
				if (student == null)
				{
					return ApiResponse<StudentDto>.ErrorResponse("Student not found");
				}

				var studentDto = _mapper.Map<StudentDto>(student);
				return ApiResponse<StudentDto>.SuccessResponse(studentDto, "Student retrieved successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<StudentDto>.ErrorResponse($"Error retrieving student: {ex.Message}");
			}
		}

		public async Task<ApiResponse<StudentDto>> CreateStudentAsync(CreateStudentDto createStudentDto)
		{
			try
			{
				// Check if student ID already exists
				if (await _studentRepository.ExistsByStudentIdAsync(createStudentDto.StudentId))
				{
					return ApiResponse<StudentDto>.ErrorResponse("Student ID already exists");
				}

				// Check if email already exists
				if (await _studentRepository.ExistsByEmailAsync(createStudentDto.Email))
				{
					return ApiResponse<StudentDto>.ErrorResponse("Email already exists");
				}

				var student = _mapper.Map<Student>(createStudentDto);
				var createdStudent = await _studentRepository.CreateAsync(student);
				var studentDto = _mapper.Map<StudentDto>(createdStudent);

				return ApiResponse<StudentDto>.SuccessResponse(studentDto, "Student created successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<StudentDto>.ErrorResponse($"Error creating student: {ex.Message}");
			}
		}

		public async Task<ApiResponse<StudentDto>> UpdateStudentAsync(Guid id, UpdateStudentDto updateStudentDto)
		{
			try
			{
				var existingStudent = await _studentRepository.GetByIdAsync(id);
				if (existingStudent == null)
				{
					return ApiResponse<StudentDto>.ErrorResponse("Student not found");
				}

				// Check if email already exists for another student
				var studentWithEmail = await _studentRepository.GetByEmailAsync(updateStudentDto.Email);
				if (studentWithEmail != null && studentWithEmail.Id != id)
				{
					return ApiResponse<StudentDto>.ErrorResponse("Email already exists for another student");
				}

				_mapper.Map(updateStudentDto, existingStudent);
				var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
				var studentDto = _mapper.Map<StudentDto>(updatedStudent);

				return ApiResponse<StudentDto>.SuccessResponse(studentDto, "Student updated successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<StudentDto>.ErrorResponse($"Error updating student: {ex.Message}");
			}
		}

		public async Task<ApiResponse<object>> DeleteStudentAsync(Guid id)
		{
			try
			{
				if (!await _studentRepository.ExistsAsync(id))
				{
					return ApiResponse<object>.ErrorResponse("Student not found");
				}

				var deleted = await _studentRepository.DeleteAsync(id);
				if (!deleted)
				{
					return ApiResponse<object>.ErrorResponse("Failed to delete student");
				}

				return ApiResponse<object>.SuccessResponse("Student deleted successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<object>.ErrorResponse($"Error deleting student: {ex.Message}");
			}
		}

	}
}
