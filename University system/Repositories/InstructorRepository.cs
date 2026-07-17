using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interface_Repository;
using University_system.Models;

namespace University_system.Repositories
{
    public class InstructorRepository : GenericRepository<Student>, IInstructorRepository
    {
        public InstructorRepository(DataContext context) : base(context) { }
        
        public async Task<int> GetInstructorIdByUserIdAsync(string userId)
        {
            var Instructor = await _dbSet.FirstOrDefaultAsync(I => I.UserId == userId);

            return Instructor?.Id ?? 0;
        }
    }
}
