namespace DarazApp.Entities
{
    public class ProductComments
    {
        public int CommentId { get; set; }
        public string? CommentDescription { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? OrderDetailsId { get; set; }
        public int? Rating { get; set; } 
        public int? VariationId { get; set; }
        public DateTime? CommentedOn { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
        public virtual ProductVariations? ProductVariations { get; set; }

    }
}
