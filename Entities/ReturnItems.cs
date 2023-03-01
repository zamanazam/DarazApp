using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities
{
    public class ReturnItems
    {
        public int? R_ItemId { get; set; }
        [ForeignKey(nameof(Products))]
        public int? P_Id { get; set; }
        [ForeignKey(nameof(OrderDetails))]
        public int? Order_Id { get; set; }
        [ForeignKey(nameof(ProductVariations))]
        public int? Var_Id { get; set; }
        [ForeignKey(nameof(ProductBatch))]
        public int? B_id { get; set; }
        [ForeignKey(nameof(User))]
        public int? Ret_By { get; set; }
        [ForeignKey(nameof(Orders))]
        public int? OrderItems_Id { get; set; }
        public int? R_Number { get; set; }
        public int? Ret_Quantity { get; set; }
        public DateTime R_On { get; set; } 
        public string? Ret_Reason { get; set; }
        public string? ProductImage { get; set; }
        public virtual User? User { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
        public virtual Orders? Orders { get; set; }
        public virtual Product? Products { get; set; }
        public virtual ProductVariations? ProductVariations { get; set; }
        public virtual ProductBatch? ProductBatch { get; set; }

    }
}
