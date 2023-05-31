namespace Services
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> GetUserByUsername(string username);
        Task<ServiceResponse<GetUserDto>> GetMyProfile(int userId);
    }
}