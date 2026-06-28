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

        public async Task<Student> GetStudentWithUserAsync(int studentId)
        {
            return await _dbSet.Where(s => s.Id == studentId).FirstOrDefaultAsync();
        }
    }
    }

