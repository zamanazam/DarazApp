using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DarazApp.Models
{
    [Keyless]
    public class ForgetPasswordModel
    {
        [Required ,EmailAddress, Display(Name = "Register Email Address")]
        
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}