using Microsoft.EntityFrameworkCore;
using University_system.Data;
using University_system.Interface_Repository;
using University_system.Interfaces;
using University_system.Models;

namespace University_system.Repositories
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {

        public SemesterRepository(DataContext context) : base(context)
        {

        }

        public async Task<int> GetCurrentSemesterAsync()
        {
            var semester = await _dbSet.FirstOrDefaultAsync(s => s.IsCurrent);
          
            return semester?.Id  ?? 0;


        }
    }
}
