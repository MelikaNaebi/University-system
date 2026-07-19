using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interfaces;
using University_system.Models;

namespace University_system.Repositories
{
   
        public class StudentRepository : GenericRepository<Student>, IStudentRepository
        {
            public StudentRepository(DataContext context) : base(context)
            {
            }

        public async Task<List<Student>> GetFilteredStudentsAsync(string? studentNumber, string? name)
        {
            
            var query = _dbSet.Include(s => s.User).Include(s => s.Enrollments).ThenInclude(e => e.Course).AsQueryable();


            if (!string.IsNullOrEmpty(studentNumber))
            {
                query = query.Where(s => s.StudentNumber.Contains(studentNumber));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.User.FirstName.Contains(name) || s.User.LastName.Contains(name));
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetStudentIdByUserIdAsync(string userId)
        {
            var student = await _dbSet.FirstOrDefaultAsync(s => s.UserId == userId); 


            return student?.Id ?? 0;
        }

        public async Task<Student> GetStudentWithUserAsync(int studentId)
        {
            return await _dbSet.Include(s => s.User).Include(s => s.Department).Where(s => s.Id == studentId).FirstOrDefaultAsync();
        }
    }
    }

