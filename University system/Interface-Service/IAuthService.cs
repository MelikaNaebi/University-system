

using University_system.Dtos;

namespace University_system.Interface_Service

{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);


    }
}
