namespace University_system.Interface_Repository
{
    public interface ISemesterRepository
    {
        Task<int> GetCurrentSemesterAsync();
    }
}
