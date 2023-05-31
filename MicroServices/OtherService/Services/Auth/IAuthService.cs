namespace Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<SessionDto>> Register(User user, string password);
        Task<ServiceResponse<SessionDto>> Login(string email, string password);
    }
}