using Microsoft.AspNetCore.Mvc;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Classes.Dtos
{
	public class ClassDtos 
	{
		public class ClassCreateDto
		{
			public string ClassCode { get; set; } = null!;
			public string Name { get; set; } = null!;
			public string? Description { get; set; }
		}
		public class ClassDto
		{
			public Guid Id { get; set; }
			public string ClassCode { get; set; } = null!;
			public string Name { get; set; } = null!;
			public string? Description { get; set; }
			public Guid? TeacherId { get; set; }
			public string? TeacherFullName { get; set; }
			public int StudentCount { get; set; }
			public DateTime CreatedAt { get; set; }
		}

		public class ClassSummaryDto
		{
			public Guid Id { get; set; }
			public string ClassCode { get; set; } = null!;
			public string Name { get; set; } = null!;
			public string? Description { get; set; }

			// Guru
			public Guid? TeacherId { get; set; }
			public string? TeacherFullName { get; set; }

			// Siswa
			public List<StudentSummaryDto> Students { get; set; } = new();

			public DateTime CreatedAt { get; set; }
		}

		public class AssignTeacherDto
		{
			public Guid ClassId { get; set; }
			public Guid? TeacherId { get; set; } 
		}
	}
}
