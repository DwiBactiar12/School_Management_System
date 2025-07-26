using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Modules.Teachers.Dtos
{
	public class TeacherDtos : Controller
	{
		public class CreateTeacherDto
		{
			[Required]
			[StringLength(50, ErrorMessage = "TeacherId maksimal 50 karakter.")]
			public string TeacherId { get; set; } = null!;

			[Required]
			[StringLength(100, ErrorMessage = "Nama depan maksimal 100 karakter.")]
			public string FirstName { get; set; } = null!;

			[Required]
			[StringLength(100, ErrorMessage = "Nama belakang maksimal 100 karakter.")]
			public string LastName { get; set; } = null!;

			[Required]
			[EmailAddress(ErrorMessage = "Format email tidak valid.")]
			[StringLength(200, ErrorMessage = "Email maksimal 200 karakter.")]
			public string Email { get; set; } = null!;

			[Phone(ErrorMessage = "Format nomor telepon tidak valid.")]
			[StringLength(20, ErrorMessage = "No telepon maksimal 20 karakter.")]
			public string? Phone { get; set; }

			[StringLength(100, ErrorMessage = "Mata pelajaran maksimal 100 karakter.")]
			public string? Subject { get; set; }
		}

		public class UpdateTeacherDto
		{
			[StringLength(100, ErrorMessage = "Nama depan maksimal 100 karakter.")]
			public string? FirstName { get; set; }

			[StringLength(100, ErrorMessage = "Nama belakang maksimal 100 karakter.")]
			public string? LastName { get; set; }

			[EmailAddress(ErrorMessage = "Format email tidak valid.")]
			[StringLength(200, ErrorMessage = "Email maksimal 200 karakter.")]
			public string? Email { get; set; }

			[Phone(ErrorMessage = "Format nomor telepon tidak valid.")]
			[StringLength(20, ErrorMessage = "No telepon maksimal 20 karakter.")]
			public string? Phone { get; set; }

			[StringLength(100, ErrorMessage = "Mata pelajaran maksimal 100 karakter.")]
			public string? Subject { get; set; }
		}

		public class TeacherDto
		{
			public Guid Id { get; set; }
			public string TeacherId { get; set; } = null!;
			public string FullName { get; set; } = null!;
			public string Email { get; set; } = null!;
			public string? Phone { get; set; }
			public string? Subject { get; set; }
		}

		public class TeacherSummaryDto
		{
			public Guid Id { get; set; }
			public string TeacherId { get; set; } = null!;
			public string FullName { get; set; } = null!;
			public string Email { get; set; } = null!;
			public string? Subject { get; set; }
		}
	}
}
