using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Modules.Students.Dtos
{
	public class StudentDtos : Controller
	{
		public class CreateStudentDto
		{
			[Required]
			[StringLength(50)]
			public string StudentId { get; set; } = string.Empty;

			[Required]
			[StringLength(100)]
			public string FirstName { get; set; } = string.Empty;

			[Required]
			[StringLength(100)]
			public string LastName { get; set; } = string.Empty;

			[Required]
			[EmailAddress]
			[StringLength(200)]
			public string Email { get; set; } = string.Empty;

			[Phone]
			[StringLength(20)]
			public string? Phone { get; set; }

			[Required]
			public DateTime DateOfBirth { get; set; }
		}

		public class UpdateStudentDto
		{
			[Required]
			[StringLength(100)]
			public string FirstName { get; set; } = string.Empty;

			[Required]
			[StringLength(100)]
			public string LastName { get; set; } = string.Empty;

			[Required]
			[EmailAddress]
			[StringLength(200)]
			public string Email { get; set; } = string.Empty;

			[Phone]
			[StringLength(20)]
			public string? Phone { get; set; }

			[Required]
			public DateTime DateOfBirth { get; set; }
		}

		public class StudentDto
		{
			public Guid Id { get; set; }
			public string StudentId { get; set; } = string.Empty;
			public string FirstName { get; set; } = string.Empty;
			public string LastName { get; set; } = string.Empty;
			public string FullName { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
			public string? Phone { get; set; }
			public DateTime DateOfBirth { get; set; }
			public DateTime CreatedAt { get; set; }
			public DateTime UpdatedAt { get; set; }
			public int EnrollmentCount { get; set; }
		}

		public class StudentSummaryDto
		{
			public Guid Id { get; set; }
			public string StudentId { get; set; } = string.Empty;
			public string FullName { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
		}
	}
}
