using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class GuestandStoreChat
    {

        public int GS_ChatId { get; set; }

        [ForeignKey(nameof(Store))]
        public int? StoreId  { get; set; }

        [ForeignKey(nameof(Guest))]
        public int? GuestId { get; set; } 
        public string? Msg_Text { get; set; }
        public int? Msg_Status { get; set; }
        public DateTime? ChatOn { get; set; }
        public DateTime? Seen_On { get; set; }
        public virtual User? Store { get; set; }
        public virtual Guest? Guest { get; set; }
    }
}
