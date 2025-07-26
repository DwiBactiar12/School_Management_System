using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Classes.Entities
{
	public class Class
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string ClassCode { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public Guid? TeacherId { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Navigation
		public Teacher? Teacher { get; set; }
		public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
	}
}
