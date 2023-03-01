using DarazApp.Entities;

namespace DarazApp.Services
{
    public class ViewModel
    {

        public List<User> usersvm = new List<User>();
        public List<Category> categoriesvm = new List<Category>();
        public List<PerCategory> perCategoriesvm = new List<PerCategory>();
        public List<Product> ProductVm = new List<Product>();
        public List<ProductDescription> ProductDescriptionVm = new List<ProductDescription>();
        public List<KidCategory> kidCategoriesvm = new List<KidCategory>();
        public List<ProductDescriptionimage> productDescriptionimages = new List<ProductDescriptionimage>();
        public List<ProductSpecification> productSpecificationVm = new List<ProductSpecification>();
        public List<Product> similarproducts = new List<Product>();
        public List<ProductSize> productSizesvm = new List<ProductSize>();
        public List<ProductColours> ProductColoursvm = new List<ProductColours>();
        public List<Colour> Coloursvm = new List<Colour>();
        public List<Sizes> Sizesvm = new List<Sizes>();
        public List<Orders> Orders = new List<Orders>();
        public List<OrderDetails> orderDetails = new List<OrderDetails>();

    };

    public class SearchViewModel
    {
        public List<Product> Products { get; set; }
        public IList<string?> ProductDescriptionVm { get; set; }
        public List<Colour> Coloursvm { get; set; }
        public List<Sizes> Sizesvm { get; set; }
    };
    public class ColourandSizes
    {
        public List<Colour> Colours { get; set; }
        public List<Sizes> Sizes { get; set; }
    }

    public class ViewModelProductById
    {
        public int ProductId { get; set; }
        public int ProductColourId { get; set; }
        public int ProductSizeId { get; set; }

        public string? ProductName { get; set; }

        public DateTime? UploadDate { get; set; }

        public string? ProductImage { get; set; }

        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public int? ProductPrice { get; set; }
        public string? ColourName { get; set; }
        public string? SizeName { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public KidCategory? KidCategory { get; set; }

        public List<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();

        public ProductDescription ProductDescriptions { get; set; } = new ProductDescription();

        public ProductSpecification ProductSpecifications { get; set; } = new ProductSpecification();

        public List<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
        public List<ProductSize> ProductSize { get; set; } = new List<ProductSize>();
        public List<Sizes> Sizes { get; set; } = new List<Sizes>();
        public List<Colour> Colours { get; set; } = new List<Colour>();
        public List<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
        public List<CardData> CardDatas { get; set; } = new List<CardData>();
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public Orders order { get; set; }
        public User? User { get; set; }
        public Sizes Size { get; set; }
        public Colour Colour { get; set; }
        public List<ProductVariations> productVariations { get; set; }
        public List<ProductBatch> productBatches { get; set; }
    }

    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public int ProductColourId { get; set; }
        public int ProductSizeId { get; set; }

        public string? ProductName { get; set; }

        public DateTime? UploadDate { get; set; }

        public string? ProductImage { get; set; }

        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public int? ProductPrice { get; set; }
        public string? ColourName { get; set; }
        public string? SizeName { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        //public WishList wishList { get; set; }
        public KidCategory? KidCategory { get; set; }

        public List<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();

        public List<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();

        public List<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();

        public List<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
        public List<ProductSize> ProductSize { get; set; } = new List<ProductSize>();
        public List<Sizes> Sizes { get; set; } = new List<Sizes>();
        public List<Colour> Colours { get; set; } = new List<Colour>();
        public List<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
        public List<CardData> CardDatas { get; set; } = new List<CardData>();
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public Orders order { get; set; }
        public User? User { get; set; }
        public Sizes Size { get; set; }
        public Colour Colour { get; set; }
        public List<ProductVariations> productVariations { get; set; }
        public List<ProductBatch> productBatches { get; set; }
    }
    public class BatchViewModel
    {
        public string? ColourName { get; set; }
        public string? SizeName { get; set; }
        public int ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductViewModelDup
    {
        public int ProductId { get; set; }
        public int ProductColourId { get; set; }
        public int ProductSizeId { get; set; }

        public string? ProductName { get; set; }

        public DateTime? UploadDate { get; set; }

        public string? ProductImage { get; set; }

        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public int? ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public KidCategory? KidCategory { get; set; }

        public List<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();

        public ProductDescription[] ProductDescriptions { get; set; }

        public ProductSpecification[] ProductSpecifications { get; set; }

        public List<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
        public List<ProductSize> ProductSize { get; set; } = new List<ProductSize>();
        public Sizes[] Sizes { get; set; }
        public Colour[] Colours { get; set; }
        public List<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
        public List<CardData> CardDatas { get; set; } = new List<CardData>();
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
        public Orders order { get; set; }
        public User? User { get; set; }
        public Sizes Size { get; set; }
        public Colour Colour { get; set; }

    }
    public class CheckInViewModel
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public DateTime? UploadDate { get; set; }

        public string? ProductImage { get; set; }

        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public int? ProductPrice { get; set; }
        public string PostalCode { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }

        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }
        public string? Message { get; set; }

        public int? Age { get; set; }

        public string UserImage { get; set; }
        public string UserName { get; set; } = null!;
        public KidCategory? KidCategory { get; set; }

        public List<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();

        public List<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();

        public List<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();

        public List<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
        public List<ProductSize> ProductSize { get; set; } = new List<ProductSize>();
        public List<Sizes> Sizes { get; set; } = new List<Sizes>();
        public List<Colour> Colours { get; set; } = new List<Colour>();
        public List<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
        public List<CardData> CardDatas { get; set; } = new List<CardData>();
        public List<Orders> Orders { get; set; }
        public User? User { get; set; }
        public Colour Colour { get; set; }
        public List<Product> Product { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
    public class SellerAllOrdersSelectView
    {
        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public string? UserName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Nationality { get; set; }
        public string? PostalCode { get; set; }
        public string? Message { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public Orders Orders { get; set; }
        public List<SellerAllOrdersViewModel> SellerAllOrdersViewModel { get; set; }
    }
}
