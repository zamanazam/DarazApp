namespace DarazApp.Services
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public int? secretkey { get; set; }

        public string? UserName { get; set; } 

        public string? Password { get; set; } 
        public string? PostalCode { get; set; } 

        public string? PhoneNo { get; set; } 

        public string? Email { get; set; }
        public string? UserImage { get; set; }
        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? FatherName { get; set; }

        public int? Age { get; set; }

        public DateTime? Date { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public IFormFile? file { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Contact { get; set; } 
        public string? StoreName { get; set; } 
        public string? StoreEmail { get; set; }
        public string? ShippingInfo { get; set; } 
        public DateTime? Createon { get; set; }
    }
}
