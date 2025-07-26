using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Teachers.Repositories
{

    public interface ITeacherRepository
    {
        Task<PaginatedResult<Teacher>> GetAllAsync(PaginationParameters parameters);
        Task<Teacher?> GetByIdAsync(Guid id);
        Task<Teacher?> GetByEmailAsync(string email);
        Task<Teacher> CreateAsync(Teacher teacher);
        Task<Teacher> UpdateAsync(Teacher teacher);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByTeacherIdAsync(string studentId);

		Task<bool> ExistsByEmailAsync(string email);
    }

    public class TeacherRepository : ITeacherRepository
	{
        private readonly SchoolDbContext _context;

        public TeacherRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Teacher>> GetAllAsync(PaginationParameters parameters)
        {
            var query = _context.Teachers.AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(searchTerm) ||
                    s.LastName.ToLower().Contains(searchTerm) ||
                    s.Email.ToLower().Contains(searchTerm));
            }

            // Sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "firstname" => parameters.SortDescending ? query.OrderByDescending(s => s.FirstName) : query.OrderBy(s => s.FirstName),
                    "lastname" => parameters.SortDescending ? query.OrderByDescending(s => s.LastName) : query.OrderBy(s => s.LastName),
                    "email" => parameters.SortDescending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                    "createdat" => parameters.SortDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                    _ => query.OrderBy(s => s.LastName)
                };
            }
            else
            {
                query = query.OrderBy(s => s.LastName);
            }

            var totalCount = await query.CountAsync();
            var teachers = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PaginatedResult<Teacher>(teachers, totalCount, parameters.Page, parameters.PageSize);
        }


        public async Task<Teacher?> GetByIdAsync(Guid id)
        {
            return await _context.Teachers
                .Include(s => s.Classes)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Teacher?> GetByEmailAsync(string email)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            teacher.UpdatedAt = DateTime.UtcNow;
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Teachers.AnyAsync(s => s.Id == id);
        }

		public async Task<bool> ExistsByTeacherIdAsync(string studentId)
		{
			return await _context.Teachers.AnyAsync(s => s.TeacherId == studentId);
		}
		public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Teachers.AnyAsync(s => s.Email.ToLower() == email.ToLower());
        }

    }
}
