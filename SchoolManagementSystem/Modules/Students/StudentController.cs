using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Students.Service;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Students
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class StudentController : Controller
	{
		private readonly IStudentService _studentService;

		public StudentController(IStudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllStudents([FromQuery] PaginationParameters parameters)
		{
			var result = await _studentService.GetAllStudentsAsync(parameters);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetStudentById(Guid id)
		{
			var result = await _studentService.GetStudentByIdAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _studentService.CreateStudentAsync(createStudentDto);

			if (!result.Success)
				return BadRequest(result);

			return CreatedAtAction(nameof(GetStudentById), new { id = result.Data!.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _studentService.UpdateStudentAsync(id, updateStudentDto);

			if (!result.Success)
			{
				if (result.Message.Contains("not found"))
					return NotFound(result);
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(Guid id)
		{
			var result = await _studentService.DeleteStudentAsync(id);

			if (!result.Success)
			{
				if (result.Message.Contains("not found"))
					return NotFound(result);
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
