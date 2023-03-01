using DarazApp.Entities;

namespace DarazApp.Models
{
    public class JoinandViewModle
    {
        public partial class Products
        {
            public int ProductId { get; set; }

            public string? ProductName { get; set; }

            public DateTime? UploadDate { get; set; }

            public string? ProductImage { get; set; }

            public int? UserId { get; set; }

            public int? KidCategoryId { get; set; }

            public virtual KidCategory? KidCategory { get; set; }

            public virtual ICollection<ProductDescription> ProductDescriptions { get; } = new List<ProductDescription>();

            public virtual ICollection<SaleInvoice> SaleInvoices { get; } = new List<SaleInvoice>();

            public virtual User? User { get; set; }
        }
        public partial class ProductDescriptions
        {
            public int PdescriptionId { get; set; }

            public string? ProductType { get; set; }

            public string? ProductQuality { get; set; }

            public string? ProductColor { get; set; }

            public string? ProductQuantity { get; set; }

            public string? ProductDes { get; set; }

            public int? ProductId { get; set; }

            public int ProductPrice { get; set; }
            public string ProductImage { get; set; }

            public int? UserId { get; set; }

            public virtual User Pdescription { get; set; } = null!;

            public virtual Product? Product { get; set; }
        }
    }
}
