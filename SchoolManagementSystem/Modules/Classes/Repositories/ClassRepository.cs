using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Classes.Repositories
{
	public interface IClassRepository
	{
		Task<PaginatedResult<Class>> GetAllAsync(PaginationParameters parameters);
		Task<Class> UpdateAsync(Class newClass);
		Task<bool> ExistsByClassCodeIdAsync(string classCode);
		Task<bool> ExistsAsync(Guid id);
		Task<Class> CreateAsync(Class newClass);
		Task<Class?> GetByIdAsync(Guid id);
	}

	public class ClassRepository : IClassRepository
	{
		private readonly SchoolDbContext _context;
		public ClassRepository(SchoolDbContext context)
		{
			_context = context;
		}

		public async Task<PaginatedResult<Class>> GetAllAsync(PaginationParameters parameters)
		{
			var query = _context.Classes.Include(s => s.Enrollments).Include(s=>s.Teacher).AsQueryable();

			// Search functionality
			if (!string.IsNullOrEmpty(parameters.SearchTerm))
			{
				var searchTerm = parameters.SearchTerm.ToLower();
				query = query.Where(s =>
					s.Name.ToLower().Contains(searchTerm) ||
					s.ClassCode.ToLower().Contains(searchTerm) ||
					(s.Teacher != null && ((s.Teacher.FirstName + " " + s.Teacher.LastName).ToLower().Contains(searchTerm))));
			}

			if (!string.IsNullOrEmpty(parameters.SortBy))
			{
				query = parameters.SortBy.ToLower() switch
				{
					"name" => parameters.SortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
					"classcode" => parameters.SortDescending ? query.OrderByDescending(c => c.ClassCode) : query.OrderBy(c => c.ClassCode),
					"createdat" => parameters.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
					_ => query.OrderBy(s => s.Name)
				};
			}
			else
			{
				query = query.OrderBy(s => s.Name);
			}

			var totalCount = await query.CountAsync();
			var students = await query
				.Skip((parameters.Page - 1) * parameters.PageSize)
				.Take(parameters.PageSize)
				.ToListAsync();

			return new PaginatedResult<Class>(students, totalCount, parameters.Page, parameters.PageSize);
		}

		public async Task<Class> CreateAsync(Class newClass)
		{
			_context.Classes.Add(newClass);
			await _context.SaveChangesAsync();
			return newClass;
		}

		public async Task<bool> ExistsAsync(Guid id)
		{
			return await _context.Classes.AnyAsync(s => s.Id == id);
		}

		public async Task<bool> ExistsByClassCodeIdAsync(string classCode)
		{
			return await _context.Classes.AnyAsync(s => s.ClassCode == classCode);
		}
		public async Task<Class> UpdateAsync(Class newClass)
		{
			newClass.UpdatedAt = DateTime.UtcNow;
			_context.Classes.Update(newClass);
			await _context.SaveChangesAsync();
			return newClass;
		}

		public async Task<Class?> GetByIdAsync(Guid id)
		{
			return await _context.Classes
				.Include(c => c.Teacher)
				.Include(c => c.Enrollments)
				.ThenInclude(e => e.Student)
				.FirstOrDefaultAsync(c => c.Id == id);
		}
	}
}
