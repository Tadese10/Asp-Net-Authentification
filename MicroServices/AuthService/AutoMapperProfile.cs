namespace Server
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //<t> source => <t> destination
            CreateMap<UserDto, Session>();
        }
    }
}