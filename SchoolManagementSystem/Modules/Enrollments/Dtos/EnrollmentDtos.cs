using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Modules.Enrollments.Dtos
{
	public class EnrollmentDtos 
	{
		public class CreateEnrollmentDto
		{
			public Guid StudentId { get; set; }
			public Guid ClassId { get; set; }
		}

		public class EnrollmentDto
		{
			public Guid Id { get; set; }
			public Guid StudentId { get; set; }
			public string StudentName { get; set; } = string.Empty;
			public string StudentEmail { get; set; } = string.Empty;

			public Guid ClassId { get; set; }
			public string ClassName { get; set; } = string.Empty;
			public string ClassCode { get; set; } = string.Empty;

			public DateTime EnrolledAt { get; set; }
		}
	}
}
