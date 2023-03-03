using DarazApp.Entities;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Services
{
    public class ProductSelectView
    {
        public int ProductId { get; set; }
        public int Sizeid { get; set; }
        public int PdescriptionId { get; set; }
        public string ProductImage { get; set; }
        public int ProductDescriptionImagesId { get; set; }
        public int ProductSpecificationId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string SellerName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string SellerImage { get; set; }
        public int SellerId { get; set; }
        public int? ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public KidCategory kidCategory { get; set; }
        public ProductViewModel ProductViewModels { get; set; }
        public ViewModelProductById ViewModelProductById { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public List<WishList> wishList { get; set; }

    }

    public class FilterSearchView
    {
        public int ProductId { get; set; }
        public int Sizeid { get; set; }
        public int PdescriptionId { get; set; }
        public string ProductImage { get; set; }
        public int ProductDescriptionImagesId { get; set; }
        public int ProductSpecificationId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string SellerName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string SellerImage { get; set; }
        public int SellerId { get; set; }
        public int? ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public Sizes sizes { get; set; }
        public Colour colours { get; set; }
        public ProductDescription productDescriptions { get; set; }
        public KidCategory kidCategory { get; set; }
        public ProductViewModel ProductViewModels { get; set; }
        public List<ProductComments> ProductComments { get; set; }

    }

    public class ProductSelectViewDup
    {
        public int ProductId { get; set; }
        public int Sizeid { get; set; }
        public int PdescriptionId { get; set; }
        public string ProductImage { get; set; }
        public int ProductDescriptionImagesId { get; set; }
        public int ProductSpecificationId { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string SellerName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string SellerImage { get; set; }
        public int SellerId { get; set; }
        public int? ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public KidCategory kidCategory { get; set; }
        public ProductViewModelDup ProductViewModelDup { get; set; }
        public List<ProductComments> ProductComments { get; set; }

    }
    public class CardSelectView
    {
        public Product Product { get; set; }
        public Sizes Size { get; set; }
        public Colour Colour { get; set; }
    }

    public class ReturnModelView
    {
        public int? Ret_Quantity { get; set; }
        public int? Ret_Price { get; set; }
        public int? Order_Id { get; set; }
        public User User {get; set;}
        public int? OrderItems_Id { get; set; }
        public string? ProductImage { get; set; }
        public int? Ret_By { get; set; }
        public string? Ret_Reason { get; set; }
        public DateTime? R_On { get; set; }
        public int? R_Number { get; set; }
        public int? P_Id { get; set; }
    }
    public class CheckInSelectView
    {
        public int OrderId { get; set; }
        public int ODOrderdetailId { get; set; }
        public int WishListId { get; set; }
        public int SizeId { get; set; }

        public int ColourId { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public string? ShippingAddress { get; set; }  
        public int? ProductPrice { get; set; }
        public int? Stauts { get; set; }
        public string? StatusName { get; set; }
        public int? TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColourName { get; set; }
        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public string PostalCode { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }

        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }

        public int? Age { get; set; }

        public string UserImage { get; set; }
        public string odUserImage { get; set; }
        public string UserName { get; set; } = null!;
        public string ODUserName { get; set; } = null!;
        public string ODPhoneNo { get; set; } = null!;
        public string ODEmail { get; set; } = null!;
        public string ODNationality { get; set; } = null!;
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductId { get; set; }
        public StatusesName statusesName { get; set; }
        public List<Product> Product { get; set; }
        public List<OrderDetails> orderDetails { get; set; }
        public List<Orders> Order { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public List<OrderHistoryView> OrderHistoryView { get; set; }
        public List<ProductDescription> ProductDescription { get; set; }

    }

    public class AllUsersChat
    {
       public string? CurrentImage { get; set; }
       public int? CurrentId { get; set; }
       public string? CurrentName { get; set; }
       public string? UserName { get; set; }
       public int? UserId { get; set; }
       public string? UserImage { get; set; }

    }
    public class StatusesName
    {
        public string? StatusName { get; set; }
    }
    public class OrdersSelectView
    {
        public int OrderId { get; set; }
        public int ODOrderdetailId { get; set; }
        public int WishListId { get; set; }
        public int SizeId { get; set; }
        public int ColourId { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? ShippingAddress { get; set; }
        public int? ProductPrice { get; set; }
        public int? Stauts { get; set; }
        public string? StatusName { get; set; }
        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColourName { get; set; }
        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public string PostalCode { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }

        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }

        public int? Age { get; set; }

        public string UserImage { get; set; }
        public string odUserImage { get; set; }
        public string UserName { get; set; } = null!;
        public string ODUserName { get; set; } = null!;
        public string ODPhoneNo { get; set; } = null!;
        public string ODEmail { get; set; } = null!;
        public string ODNationality { get; set; } = null!;
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductId { get; set; }
        public List<Product> Product { get; set; }
        public OrderDetails orderDetails { get; set; }
        public Orders Order { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public List<OrderHistoryView?> OrderHistoryView { get; set; }

    }
    public class OrdersSelectViewSeller
    {
        public int OrderId { get; set; }
        public int ODOrderdetailId { get; set; }
        public int WishListId { get; set; }
        public int SizeId { get; set; }
        public int ColourId { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? ShippingAddress { get; set; }
        public int? ProductPrice { get; set; }
        public string? Stauts { get; set; }

        public int TotalPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColourName { get; set; }
        public int? UserId { get; set; }

        public int? KidCategoryId { get; set; }

        public string PostalCode { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }

        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }

        public int? Age { get; set; }

        public string UserImage { get; set; }
        public string odUserImage { get; set; }
        public string UserName { get; set; } = null!;
        public string ODUserName { get; set; } = null!;
        public string ODPhoneNo { get; set; } = null!;
        public string ODEmail { get; set; } = null!;
        public string ODNationality { get; set; } = null!;
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductId { get; set; }
        public List<Product> Product { get; set; }
        //public OrderDetails orderDetails { get; set; }
        public Orders Order { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public List<OrderHistorySellerView> OrderHistorySellerView { get; set; }

    }
    public class CardItemsView
    {
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int? ProductPrice { get; set; }
        List<Product> product { get; set; }
    }
    public class OrderHistoryView
    {
        public string StoreName { get; set; } = null!;
        public string StorePhone { get; set; } = null!;
        public string StoreEmail { get; set; } = null!;
        public string StoreImage { get; set; } = null!;
        public int? StoreId { get; set; } = null!;
        public string ODUserName { get; set; } = null!;
        public string ODPhoneNo { get; set; } = null!;
        public string ODEmail { get; set; } = null!;
        public string ODNationality { get; set; } = null!;
        public string ODPostalCode { get; set; } = null!;
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int? ProductPrice { get; set; }
        public double? CommisionValue { get; set; }
        public int? NetPrice { get; set; }

        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductQuantity { get; set; }
        public int? RemainingQuantity { get; set; }
        public int? TotalPrice { get; set; }
        public string CommentDescription { get; set; }
        public int Rating { get; set; }
        public Product product { get; set; }
        public List<Product> productlist { get; set; }

        public OrderDetails Orderdetails { get; set; }
        public Orders Orders { get; set; }
        public List<Orders> Order { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public ProductVariations ProductVariations { get; set; }
        public ProductBatch ProductBatches { get; set; }
    }
    public class OrderHistorySellerView
    {
        public string ODUserName { get; set; } = null!;
        public string ODPhoneNo { get; set; } = null!;
        public string ODEmail { get; set; } = null!;
        public string ODNationality { get; set; } = null!;
        public string ODPostalCode { get; set; } = null!;
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int? ProductPrice { get; set; }
        public int? ProductId { get; set; }
        public int? ProductQuantity { get; set; }
        public int? TotalPrice { get; set; }
        public string CommentDescription { get; set; }
        public int Rating { get; set; }
        public Product product { get; set; }
        public OrderDetails Orderdetails { get; set; }
        public Orders Orders { get; set; }
        public List<Orders> Order { get; set; }
        public ProductComments ProductCommentsSeller { get; set; }
    }
    public class SellerAllOrdersViewModel
    {
        public string? UserName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Nationality { get; set; }
        public string? PostalCode { get; set; }
        public string? Message { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public Orders Orders { get; set; }
        public List<Product> Product { get; set; }
        public User User { get; set; }
        //public OrderDetails OrderDetails { get; set; }
    }
    public class OrdersSelllerView
    {
        public int? StoreId { get; set; }
        public string UserName { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string StorePostalCode{ get; set; } = null!;

        public string PhoneNo { get; set; } = null!;
        public string StorePhoneNo { get; set; } = null!;

        public string? Email { get; set; }
        public string? StoreEmail { get; set; }
        public string? UserImage { get; set; }
        public string? StoreUserImage { get; set; }
        public string? Cnic { get; set; }
        public string? StoreCnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }
        public string? StoreNationality { get; set; }

        public string? City { get; set; }
        public string? StoreCity { get; set; }
        public string? OTP { get; set; }

        public string Address { get; set; } = null!;
        public string StoreAddress { get; set; } = null!;

        public string? FatherName { get; set; }

        public int? Age { get; set; }
        
        public ProductVariations ProductVariations { get; set; }
        public Product Product { get; set; }
       public User User { get; set; }
        public List<OrdersSellerViewChild> OrdersSellerViewChild { get; set; }

    }
    public class OrdersSellerViewChild
    {
        public int? OrderDetailsId { get; set; }
        public int? ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public string? ProductName { get; set; }
        public string VariationName { get; set; }
        public string? ProductImage { get; set; }
        public int? VariatioId { get; set; }
        public int? ProductId { get; set; }
        public List<Product> Products { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public List<ProductVariations> ProductVariations { get; set; }
        public List<ProductComments> ProductComments { get; set; }
        public ProductBatch ProductBatches { get; set; }
        public Orders Orders { get; set; }
        public List<ProductVariationModel> productVariationModels { get; set; }

    }

    public class RoomViewModel
    {
        public int? RoomId { get; set; }
        public Room Room { get; set; }
        public HelpType HelpType { get; set; }
        public int? A_ChatId { get; set; }
        public string? MessageText { get; set; }
        public int? MessageBy { get; set; } 
        public int? UserId { get; set; }
        public int? MessageStatus{ get; set; }
        public DateTime? MessageOn { get; set; }
    }

    public class AllChatsUserViewModel
    {
        public DateTime? RoomOn { get; set; }
        public int? RoomStatus { get; set; }
        public string? RoomText { get; set; }
        public int? R_Id { get; set; }
        public int? RoleId { get; set; }
        public AdminChat? AdminChat { get; set; }
        public int? AdminId { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? AssignedOn { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public int? SecretKey { get; set; }
        public int? H_Id { get; set; }
        public string UserImage { get; set; }
        public User Users{ get; set; }

}
    public class ProductVariationModel
    {
        public int? OrderDetailsId { get; set; }
        public int P_VariationsID { get; set; }     
        public string P_VariationsName { get; set; }
        //public List<ProductVariations> ProductVariations { get; set; }

        public List<OrderDetails> OrderDetails { get; set; } 
        public List<ProductComments> ProductComments { get; set;}
    }
    public class UsersAdminModel
    {
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string UserName { get; set; } = null!;
        public bool? Status { get; set; }
        public string Password { get; set; } = null!;
        public string PostalCode { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string? Email { get; set; }
        public string? UserImage { get; set; }
        public string? Cnic { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? City { get; set; }
        public string? OTP { get; set; }

        public string Address { get; set; } = null!;

        public string? FatherName { get; set; }
        public string? RoleName { get; set; }
        public int? Age { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public Role Roles { get; set; }

    }
    public class SaleCurrentUser
    {
        public int? MonthlySale { get; set; }
        public int? YearlySale { get; set; }
        public int? TotalSale { get; set; }
        public int? TotalSaleWithCommision { get; set; }
        public int? ProgressSale { get; set; }
        public List<Orders> PendingOrders { get; set; }
        public List<OrderDetails> Pendings { get; set; }
    }

    public class RoomAllAssigner
    {

      public int? adminId { get; set; }

      public User? admins { get; set; }

      public User? assign { get; set; }
      public User? users { get; set; }
      public DateTime? assignedOn { get; set; }
      public int? assignedTo { get; set; }

      public string? email { get; set; }
      public int? h_Id { get; set; }

      public string? helpType { get; set; }

      public string? phoneNo { get; set; }

      public int? r_Id { get; set; }

      public DateTime? roomOn { get; set; }

      public int? roomStatus { get; set; }

      public string? roomText { get; set; }

      public int? secretKey { get; set; }

      public int? userId { get; set; }

      public string? userName { get; set; }

    }
    
}

