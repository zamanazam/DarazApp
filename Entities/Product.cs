using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public DateTime? UploadDate { get; set; }

    public string? ProductImage { get; set; }
    public int? UserId { get; set; }
    public int KidCategoryId { get; set; }
    public int? ProductPrice { get; set; }
    public virtual KidCategory? KidCategory { get; set; }
    public virtual ICollection<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();
    public virtual ICollection<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();
    public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();
    public virtual ICollection<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
    public virtual ICollection<ProductSize> ProductSize { get; set; } = new List<ProductSize>();
    public virtual ICollection<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
    public virtual ICollection<CardData> CardData { get; set; } = new List<CardData>();
    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public virtual ICollection<WishList> WishList { get; set; } = new List<WishList>();
    public virtual ICollection<ProductComments> ProductComments { get; set; } = new List<ProductComments>();
    public virtual ICollection<MoreSepcifications> MoreSepcifications { get; set; } = new List<MoreSepcifications>();
    public virtual ICollection<ProductVariations> ProductVariations { get; set; } = new List<ProductVariations>();
    public virtual ICollection<App_Table> App_Tables { get; set; } = new List<App_Table>(); 
    public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>();
    public virtual User? User { get; set; }
}
