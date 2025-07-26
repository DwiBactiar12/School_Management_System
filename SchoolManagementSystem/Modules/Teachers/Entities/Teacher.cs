using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Classes.Entities;

namespace SchoolManagementSystem.Modules.Teachers.Entities
{
	public class Teacher
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string TeacherId { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? Phone { get; set; }
		public string? Subject { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Navigation
		public ICollection<Class> Classes { get; set; } = new List<Class>();
	}
}
