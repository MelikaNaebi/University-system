namespace University_system.Dtos
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
    }
}
