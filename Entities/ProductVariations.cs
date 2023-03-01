namespace DarazApp.Entities
{
    public class ProductVariations
    {
        public int P_VariationsID { get; set; } 
        public string? P_VariationsName { get; set; }
        //public string P_VAriationLabel { get; set; }
        public int? P_Id { get; set; }
        public DateTime? UploadOn { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public virtual ICollection<CardData> CardData { get; set; } = new List<CardData>();
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<ProductComments> ProductComments { get; set; } = new List<ProductComments>();
        public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>();

    }
}
