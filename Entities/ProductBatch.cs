namespace DarazApp.Entities
{
    public class ProductBatch
    {
        public int BatchId { get; set; }
        public int? Quantity { get; set; } 
        public int? RemainingQuantity { get; set; }
        public int? Price { get; set; }
        public int P_VariationsId { get; set; }
        public DateTime? UploadOn { get; set; } 
        public DateTime? SaleOn { get; set; }
        public virtual ProductVariations? ProductVariations { get; set; }
        public virtual ICollection<CardData> CardData { get; set; } = new List<CardData>();
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>();

    }
}
