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

       

        public async Task<int> GetStudentIdByUserIdAsync(string userId)
        {
            var student = await _dbSet.FirstOrDefaultAsync(s => s.UserId == userId); // فرض بر این است که رابطه‌اش را داری


            return student?.Id ?? 0;
        }

        public async Task<Student> GetStudentWithUserAsync(int studentId)
        {
            return await _dbSet.Where(s => s.Id == studentId).FirstOrDefaultAsync();
        }
    }
    }

