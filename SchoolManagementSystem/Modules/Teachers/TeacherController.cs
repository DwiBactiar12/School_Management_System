using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Teachers.Service;
using static SchoolManagementSystem.Modules.Teachers.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Teachers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class TeacherController : Controller
	{
		private readonly ITeacherService _teacherService;

		public TeacherController(ITeacherService teacherService)
		{
			_teacherService = teacherService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTeachers([FromQuery] PaginationParameters parameters)
		{
			var result = await _teacherService.GetAllTeacherAsync(parameters);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetTeacherById(Guid id)
		{
			var result = await _teacherService.GetTeacherByIdAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto createTeacherDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _teacherService.CreateTeacherAsync(createTeacherDto);

			if (!result.Success)
				return BadRequest(result);

			return CreatedAtAction(nameof(GetTeacherById), new { id = result.Data!.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] UpdateTeacherDto updateTeacherDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _teacherService.UpdateTeacherAsync(id, updateTeacherDto);

			if (!result.Success)
			{
				if (result.Message.Contains("not found"))
					return NotFound(result);
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTeacher(Guid id)
		{
			var result = await _teacherService.DeleteTeacherAsync(id);

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
