using DarazApp.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DarazApp.Helpers;
using DarazApp.Models;
using System.Security.Principal;

using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        int GetUserId();
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        //    private List<User> _users = new List<User>
        //{
        //    new User { UserId = 1, UserName = "test", Password = "test" }
        //};
        private readonly CommerceDbContext _commerceDbContext;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
      
        public UserService(IOptions<AppSettings> appSettings, CommerceDbContext commerceDb, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _commerceDbContext = commerceDb;
            _httpContextAccessor = httpContextAccessor;
        
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _commerceDbContext.Users.FirstOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _commerceDbContext.Users;
        }
        public User GetById(int id)
        {
            return _commerceDbContext.Users.FirstOrDefault(x => x.UserId == id);
        }
        public int GetUserId()
        {
            return (_httpContextAccessor.HttpContext.Items["User"] as User).UserId;
            
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var token = new JwtSecurityToken
            (
                claims: new List<Claim>()
                { new Claim("Id", user.UserId.ToString()), 
                  new Claim("UserName", user.UserName),
               
                },

                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);

        }
    }
}
