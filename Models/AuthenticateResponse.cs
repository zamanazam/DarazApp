using DarazApp.Entities;

namespace DarazApp.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.UserId;
            UserName = user.UserName;
            Password = user.Password;
            Token = token;
        }
    }
}
