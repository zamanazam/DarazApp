using DarazApp.Entities;
namespace DarazApp.Services
{
    public class SaveData
    {
        public List<Sizes> savedatasizes = new List<Sizes>();
        public List<Colour> savedatacolours= new List<Colour>();
        public List<ProductDescription> savedataproductdescription = new List<ProductDescription>();
        public List<Product> savedataproducts = new List<Product>();
        public List<ProductDescriptionimage> savedataprodudesimages= new List<ProductDescriptionimage>();
        public int UserId { get; set; }
        public int RoleId { get; set; } 
        public int StoreId { get; set; } 
        public int VariationId { get; set; }
        public IList<int> ProdQuantity { get; set; }
        public IList<int> ProdPrice { get; set; } 
        public IList<int> BatchIds { get; set; }
        public string UserName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string Password { get; set; }
        public IList<string> Labels { get; set; }
        public IList<string> Texts { get; set; }
        public string Message { get; set; }
        public int ProductSizeId { get; set; }
        public int PdescriptionId { get; set; }
        public bool? userstatus { get; set; }
        public string? ProductType { get; set; }

        public string? ProductQuality { get; set; }
        public string? PostalCode { get; set; }

        public string? ProductColor { get; set; }
        public int ProductQuantity { get; set; }

        public int Quantity { get; set; }

        public string? ProductDes { get; set; }
        public string? blobToBase64 { get; set; } 
        public IList<string>? blobToBase64s { get; set; }

        public int P { get; set; }

        public string? ProductImage { get; set; }

        public string? ProductBrand { get; set; }
        public int ProductColourId { get; set; } 
        public string? ProductMaterial { get; set; }
        public string? ProductSizes { get; set; }
        public string Password1 { get; set; } 

        public string ProductName { get; set; }
          public string P_VariationsName { get; set; }   
        public string KidCategoryName { get; set; }
        public string PerCategoryName { get; set; }
        public string CategoryName { get; set; } = null!;
        public int KidCategoryId { get; set; }
        public int PerCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string text { get; set; }
  
        public int ProductDescriptionImagesId { get; set; }

        public string? ProductdesImage1 { get; set; }
        public IFormFile file { get; set; }
        public IList<IFormFile> savedimages { get; set; }
        public IList<int> pcolourid { get; set; }
        public IList<int> psizesid { get; set; }
        public IList<string> psizes { get; set; }
        public IList<string> sizes { get; set; }
        public IList<int> savedimagesid { get; set; }
        public IList<string> pcolours { get; set; } 
        public IList<string> colours { get; set; }
        public Object justcolours { get; set; }
        public IList<string> dropdown { get; set; }
        public IList<int> pprice { get; set; }
        public IList<int> pquantity { get; set; }

        public string? ProductColour { get; set; }

        public int ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int CardDataId { get; set; } 
        public int BatchId { get; set; } 
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public int? OrderDetailsId { get; set; }

        public int ProductPrice { get; set; }
        public int TotalPrice { get; set; }
        public string ProductImagedes { get; set; }
        public int ProductSpecificationId { get; set; }
        public string? ProductSpecificationsText { get; set; }
        public string? ProductDisplay { get; set; }
        public string? ProcessorCapacity { get; set; }
        public string? CameraQuality { get; set; }
        public string? Memory { get; set; }
        public string? Graphics { get; set; }
        public string? ProductWarrantyInfo { get; set; }
        public string? ProductShippingInfo { get; set; }

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }
        public string? UserImage { get; set; }
        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }
        public string? StatusName { get; set; }

        public int? Age { get; set; }

    }
    public class StatusModel
    {
        public int UserId { get; set; }
        public int OrderDetailsId { get; set; }
        public int StatusName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? Status { get; set; }
        public string? PhoneNo { get; set; }
        public string? Address { get; set; }
        public string? Nationality { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? FatherName { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Message { get; set; }

    }
    public class RatingModel
    {
        public string CommentDescription { get; set; }
        public int ProductId { get; set; }
        public int VariationsId { get; set; }
        public int OrderDetailsId { get; set; }
        public int Rating { get; set; }
    }
    public class BatchAddModel
    {
        public IList<int> VariationId { get; set; }
        public IList<int> pprice { get; set; }
        public IList<int> pquantity { get; set; }

    }
    public class GetColoursandSizes
    {
        public int VariationId { get; set; }

        public IList<int> ColourId { get; set; }

        public IList<int> SizeId { get; set; }

    }
    public class AddToCartModel
    {
        public int ProductId { get; set; }
        public int BatchId { get; set; }
        public int VariationId { get; set; }
        public int Quantity { get; set; }
        public int ProductPrice { get; set; }
    }
    public class AddHelpModal
    {
        public int type { get; set; }
        public string HelpText { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }  
        public string RoleType { get; set; }
        public string Address { get; set; }
        public int? secretkey { get; set; }
    }
}
