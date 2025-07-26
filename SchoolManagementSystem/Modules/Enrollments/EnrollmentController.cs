using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Enrollments.Service;
using static SchoolManagementSystem.Modules.Enrollments.Dtos.EnrollmentDtos;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Enrollments
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class EnrollmentController : Controller
	{
		private readonly IEnrollmentService _enrollmentService;
		public EnrollmentController(IEnrollmentService enrollmentService) {
			_enrollmentService = enrollmentService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllEnrollments([FromQuery] PaginationParameters parameters)
		{
			var result = await _enrollmentService.GetAllEnrollmentsAsync(parameters);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto createEnrollmentDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _enrollmentService.CreateEnrollmentAsync(createEnrollmentDto);

			if (!result.Success)
				return BadRequest(result);

			return CreatedAtAction(nameof(GetEnrollmentById), new { id = result.Data!.Id }, result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEnrollmentById(Guid id)
		{
			var result = await _enrollmentService.GetEnrollmentByIdAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEnrollment(Guid id)
		{
			var result = await _enrollmentService.DeleteEnrollmentAsync(id);

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
