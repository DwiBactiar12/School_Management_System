using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using System.Linq;

namespace SchoolManagementSystem.Modules.Enrollments.Repositories
{
	public interface IEnrollmentRepository
	{
		Task<PaginatedResult<Enrollment>> GetAllAsync(PaginationParameters parameters);
		Task<Enrollment?> GetByIdAsync(Guid id);
		Task<Enrollment> AddAsync(Enrollment enrollment);
		Task<bool> IsDuplicateAsync(Guid studentId, Guid classId);
		Task<bool> DeleteAsync(Guid id);
	}
	public class EnrollmentRepository : IEnrollmentRepository
	{
		private readonly SchoolDbContext _context;
		public EnrollmentRepository(SchoolDbContext context) {
			_context = context;
		}
		public async Task<Enrollment?> GetByIdAsync(Guid id)
		{
			return await _context.Enrollments
				.Include(e => e.Student)
				.Include(e => e.Class)
				.FirstOrDefaultAsync(e => e.Id == id);
		}
		public async Task<Enrollment> AddAsync(Enrollment enrollment)
		{
			_context.Enrollments.Add(enrollment);
			await _context.SaveChangesAsync();
			return enrollment;
		}
		public async Task<bool> IsDuplicateAsync(Guid studentId, Guid classId)
		{
			return await _context.Enrollments
				.AnyAsync(e => e.StudentId == studentId && e.ClassId == classId);
		}
		public async Task<bool> DeleteAsync(Guid id)
		{
			var enrollment = await _context.Enrollments.FindAsync(id);
			if (enrollment == null)
				return false;

			_context.Enrollments.Remove(enrollment);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<PaginatedResult<Enrollment>> GetAllAsync(PaginationParameters parameters)
		{
			var query = _context.Enrollments
				.Include(e => e.Student)
				.Include(e => e.Class)
				.AsQueryable();

			// Search by student or class
			if (!string.IsNullOrEmpty(parameters.SearchTerm))
			{
				var searchTerm = parameters.SearchTerm.ToLower();
				query = query.Where(e =>
					(e.Student != null && ((e.Student.FirstName + " " + e.Student.LastName).ToLower().Contains(searchTerm))) ||
					e.Student.Email.ToLower().Contains(searchTerm) ||
					e.Class.Name.ToLower().Contains(searchTerm) ||
					e.Class.ClassCode.ToLower().Contains(searchTerm));
			}

			if (!string.IsNullOrEmpty(parameters.SortBy))
			{
				query = parameters.SortBy.ToLower() switch
				{
					"studentname" => parameters.SortDescending
						? query.OrderByDescending(e => e.Student.FirstName)
						: query.OrderBy(e => e.Student.FirstName),
					"classname" => parameters.SortDescending
						? query.OrderByDescending(e => e.Class.Name)
						: query.OrderBy(e => e.Class.Name),
					"enrolledat" => parameters.SortDescending
						? query.OrderByDescending(e => e.EnrolledAt)
						: query.OrderBy(e => e.EnrolledAt),
					_ => query.OrderBy(e => e.EnrolledAt)
				};
			}
			else
			{
				query = query.OrderBy(e => e.EnrolledAt);
			}

			var totalCount = await query.CountAsync();
			var data = await query
				.Skip((parameters.Page - 1) * parameters.PageSize)
				.Take(parameters.PageSize)
				.ToListAsync();

			return new PaginatedResult<Enrollment>(data, totalCount, parameters.Page, parameters.PageSize);
		}

	}
}
