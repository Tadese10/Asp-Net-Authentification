using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Get user based on his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>GetUserDto</returns>
        public async Task<ServiceResponse<GetUserDto>> GetUserByUsername(string username)
        {
            var response = new ServiceResponse<GetUserDto>();

            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                if(dbUser is null)
                {
                    throw new NullReferenceException("user not found");
                }

                if(username.GetType() != typeof(string))
                {
                    throw new Exception("missing type");
                }

                response.Data = _mapper.Map<GetUserDto>(dbUser);
            }

            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        /// <summary>
        /// Get my profile informations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user profile informations</returns>
        public async Task<ServiceResponse<GetUserDto>> GetMyProfile(int userId)
        {
            var response = new ServiceResponse<GetUserDto>();

            try
            {   
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

                if(user is null)
                {
                    throw new NullReferenceException("user not found");
                }

                response.Data = _mapper.Map<GetUserDto>(user);
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }
    }
}