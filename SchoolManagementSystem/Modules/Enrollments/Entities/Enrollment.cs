using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Enrollments.Entities
{
	public class Enrollment
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid StudentId { get; set; }
		public Guid ClassId { get; set; }

		public DateTime EnrolledAt { get; set; }

		// Navigation
		public Student Student { get; set; } = null!;
		public Class Class { get; set; } = null!;
	}
}
