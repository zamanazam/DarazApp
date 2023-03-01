namespace DarazApp.Entities
{
    public class AdminChat
    {

        public int A_ChatId { get; set; }
        public string? MessageText { get; set; }
        public int? MessageStatus { get; set; }
        public int? MessageBy { get; set; }
        public DateTime? MessageOn { get; set; }
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }
        public virtual User? User { get; set; }
    }
}
