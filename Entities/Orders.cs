using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(Product))]
        public int VariationsId { get; set; }
        [ForeignKey(nameof(ProductVariations))]
        public int BatchId { get; set; }
        [ForeignKey(nameof(ProductBatch))]
        public DateTime? UploadDate { get; set; }
        public DateTime? DeleteOn { get; set; }
        //public string? ShippingAddress { get; set; }
        public int? ProductPrice { get; set; }
        public int? TotalPrice { get; set; }
        public int? Net_Price { get; set; }
        public int? ProductQuantity { get; set; }
        public int? RemainingQuantity { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(User))]
        public int StoreId { get; set; }
        public virtual User? Store { get; set; }
        public virtual Product? Products { get; set; }
        public virtual ProductVariations? ProductVariations { get; set; }
        public virtual ProductBatch? ProductBatch { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
        public virtual ICollection<App_Table> App_Tables { get; set; } = new List<App_Table>(); 
        public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>();
    }
}
