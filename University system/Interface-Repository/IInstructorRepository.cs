namespace University_system.Interface_Repository
{
    public interface IInstructorRepository
    {
        Task<int> GetInstructorIdByUserIdAsync(string userId);

    }
}
