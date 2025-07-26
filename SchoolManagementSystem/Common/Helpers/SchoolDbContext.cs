using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Teachers.Entities;
using System.Security.Claims;

namespace SchoolManagementSystem.Common.Helpers
{
	public class SchoolDbContext : DbContext
	{
		public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Student>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
				entity.Property(e => e.StudentId).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Phone).HasMaxLength(20);

				entity.HasIndex(e => e.Email).IsUnique();
				entity.HasIndex(e => e.StudentId).IsUnique();

				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			modelBuilder.Entity<Teacher>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
				entity.Property(e => e.TeacherId).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Phone).HasMaxLength(20);
				entity.Property(e => e.Subject).HasMaxLength(100);

				entity.HasIndex(e => e.Email).IsUnique();
				entity.HasIndex(e => e.TeacherId).IsUnique();

				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			modelBuilder.Entity<Class>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
				entity.Property(e => e.ClassCode).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Description).HasMaxLength(500);

				entity.HasIndex(e => e.ClassCode).IsUnique();

				entity.HasOne(e => e.Teacher)
					  .WithMany(t => t.Classes)
					  .HasForeignKey(e => e.TeacherId)
					  .OnDelete(DeleteBehavior.SetNull);

				entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
				entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});

			modelBuilder.Entity<Enrollment>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.Student)
					  .WithMany(s => s.Enrollments)
					  .HasForeignKey(e => e.StudentId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(e => e.Class)
					  .WithMany(c => c.Enrollments)
					  .HasForeignKey(e => e.ClassId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasIndex(e => new { e.StudentId, e.ClassId }).IsUnique();

				entity.Property(e => e.EnrolledAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});
		}
	}
}
