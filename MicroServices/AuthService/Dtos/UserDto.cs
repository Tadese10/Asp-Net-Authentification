namespace Dtos
{
    public class UserDto
    {
        public UserDto(int userId)
        {
            UserId = userId;
        }
        public int UserId {get; set;}
    }
}