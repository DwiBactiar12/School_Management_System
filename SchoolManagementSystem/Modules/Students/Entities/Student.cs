using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Enrollments.Entities;

namespace SchoolManagementSystem.Modules.Students.Entities
{
	public class Student
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string StudentId { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? Phone { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
	}
}
