using DarazApp.Entities;

namespace DarazApp.Services
{
    public class MonthlySaleChartData
    {
        public int? January { get;set; }
        public int? Febrary { get; set; }
        public int? March { get; set; }
        public int? April { get; set; }
        public int? May { get; set; }
        public int? June { get; set; }
        public int? July { get; set; }
        public int? August { get; set; }
        public int? September { get; set; }
        public int? October { get; set; }
        public int? November { get; set; }
        public int? December { get; set; }
    }

    public class MessagesNotifications
    {
        public string? UserImage { get; set; }
        public string? UserName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public int? MessageStatus { get; set; }
        public int? OrderDetailsId { get; set; }
        public int? StoreId { get; set; }
    }

    public class NewNotification
    {
        public User Sender { get; set; }
        public User Reveiver { get; set; }
        public string? Message { get; set; }
        public int? MessageStatus { get; set; }
        public DateTime? ChatOn { get; set; }
        public DateTime? ViewOn { get; set; }

    }
    public class AllChats
    {
        public DateTime? ChatOn { get; set; }
        public string? MessageText { get; set; }
        public int? SenderId { get; set; }
        public int? Id { get; set; }
        public int? MessgeId { get; set; }
        public int? ReceiverId { get; set; }
        public string? Userimage { get; set; }
        public string? UserName { get; set; }
        public User Sender { get; set; }
        //public User Receiver { get; set; }
    }
}
