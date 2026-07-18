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
            var instructor = await _dbSet.FirstOrDefaultAsync(i =>
                    i.UserId.Trim().ToLower() == userId.Trim().ToLower());
            return instructor?.Id ?? 0;
        }
    }
}
