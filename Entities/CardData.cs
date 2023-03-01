using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class CardData
    {
        public int CardDataId { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductPrice { get; set; }
        public int TotalPrice { get; set; }
        public int ProductId { get; set; }
        public int BatchId { get; set; }
        public int Variations_Id { get; set; }
        public int UserId { get; set; }
        public DateTime? UploadDate { get; set; }
        public virtual Product Products { get; set; }
        public virtual User User { get; set; }
        public virtual ProductVariations ProductVariations { get; set; }
        public virtual ProductBatch ProductBatch { get; set; }

    }
}
