using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Common.Helpers;
using SchoolManagementSystem.Common.Utilities;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Students.Repositories
{

    public interface IStudentRepository
    {
        Task<PaginatedResult<Student>> GetAllAsync(PaginationParameters parameters);
        Task<Student?> GetByIdAsync(Guid id);
        Task<Student?> GetByStudentIdAsync(string studentId);
        Task<Student?> GetByEmailAsync(string email);
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByStudentIdAsync(string studentId);
        Task<bool> ExistsByEmailAsync(string email);
    }

    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Student>> GetAllAsync(PaginationParameters parameters)
        {
            var query = _context.Students.Include(s => s.Enrollments).AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(searchTerm) ||
                    s.LastName.ToLower().Contains(searchTerm) ||
                    s.Email.ToLower().Contains(searchTerm) ||
                    s.StudentId.ToLower().Contains(searchTerm));
            }

            // Sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "firstname" => parameters.SortDescending ? query.OrderByDescending(s => s.FirstName) : query.OrderBy(s => s.FirstName),
                    "lastname" => parameters.SortDescending ? query.OrderByDescending(s => s.LastName) : query.OrderBy(s => s.LastName),
                    "email" => parameters.SortDescending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                    "studentid" => parameters.SortDescending ? query.OrderByDescending(s => s.StudentId) : query.OrderBy(s => s.StudentId),
                    "createdat" => parameters.SortDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                    _ => query.OrderBy(s => s.LastName)
                };
            }
            else
            {
                query = query.OrderBy(s => s.LastName);
            }

            var totalCount = await query.CountAsync();
            var students = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PaginatedResult<Student>(students, totalCount, parameters.Page, parameters.PageSize);
        }


        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Class)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetByStudentIdAsync(string studentId)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
        }

        public async Task<Student> CreateAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            student.UpdatedAt = DateTime.UtcNow;
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsByStudentIdAsync(string studentId)
        {
            return await _context.Students.AnyAsync(s => s.StudentId == studentId);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Students.AnyAsync(s => s.Email.ToLower() == email.ToLower());
        }

    }
}
