using University_system.Interfaces;
using University_system.Models;

namespace University_system.Interface_Repository
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Task<int> GetCurrentSemesterAsync();
    }
}
