
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class Room
    {
        public int? R_Id { get; set; }
        public string? UserName { get; set; } 
        public string? Email { get; set; } 
        public string? PhoneNo { get; set; } 
        public string? RoomText { get; set; }
        public DateTime? RoomOn { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(Admins))]
        public int? AdminId { get; set; }

        [ForeignKey(nameof(Users))]
        public int? SecretKey { get; set; } 

        [ForeignKey(nameof(Assign))]
        public int? AssignedTo { get; set; }
        public DateTime? AssignedOn { get; set; }

        [ForeignKey(nameof(HelpType))]
        public int? H_Id { get; set; } 
        public int? RoomStatus { get; set; } 
        public virtual User? Admins { get; set; }
        public virtual User? Users { get; set; }
        public virtual User? Assign { get; set; }
        public virtual HelpType? HelpType { get; set; }
        public virtual ICollection<AdminChat> AdminChat { get; set; } = new List<AdminChat>();
    }
}
