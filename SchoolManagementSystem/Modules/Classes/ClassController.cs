using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Responses;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Classes.Service;
using static SchoolManagementSystem.Modules.Classes.Dtos.ClassDtos;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;
using static SchoolManagementSystem.Modules.Teachers.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Classes
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class ClassController : Controller
	{
		private readonly ICalssService _classService;

		public ClassController(ICalssService classService)
		{
			_classService = classService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllClass([FromQuery] PaginationParameters parameters)
		{
			var result = await _classService.GetAllClasssAsync(parameters);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateStudent([FromBody] ClassCreateDto createClassDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _classService.CreateClassAsync(createClassDto);

			if (!result.Success)
				return BadRequest(result);

			return CreatedAtAction(nameof(GetClassById), new { id = result.Data!.Id }, result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClassById(Guid id)
		{
			var result = await _classService.GetClassByIdAsync(id);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		[HttpPut("{idClass}/assign-teacher")]
		public async Task<IActionResult> AssignTeacher(Guid idClass, [FromBody] AssignTeacherDto updateTeacherDto)
		{
			if (!ModelState.IsValid)
			{
				var errors = ValidationHelper.ExtractErrors(ModelState);
				return BadRequest(ApiResponse<object>.ErrorResponse("Validasi gagal", errors));
			}

			var result = await _classService.AssignTeacherAsync(idClass, updateTeacherDto);

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
