using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class Chat
    {
        public int Id { get; set; }
      
        [ForeignKey(nameof(ReceiverChat))]
        public int? SenderId { get; set; }

        [ForeignKey(nameof(SenderChat))]
        public int? ReceiverId { get; set; }
        public int? M_Status { get; set; }
        public DateTime? ChatOn { get; set; } 
        public DateTime? ViewOn { get; set; }

        public string? MessageText { get; set; }

        public virtual User? ReceiverChat { get; set; }
        public virtual User? SenderChat { get; set; }
    }
}
