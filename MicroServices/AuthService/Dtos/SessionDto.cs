namespace Dtos
{
    public class SessionDto
    {
        public SessionDto(string sessionId, string token)
        {
            SessionId = sessionId;
            Token = token;

        }

        //ecnrypted id
        public string SessionId {get; set;}
        public string Token {get; set;}
    }
}
