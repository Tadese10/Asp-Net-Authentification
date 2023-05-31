namespace Services
{
    public interface ISessionService
    {
        Task<ServiceResponse<SessionDto>> CreateSession(int userId, string sessionCookie);
        Task<ServiceResponse<bool>> DeleteSession(int userId);
        Task<ServiceResponse<bool>> IsConnected(string sessionCookie);
        Task<ServiceResponse<UserDto>> GetUserInformation(string sessionCookie);
    }
}