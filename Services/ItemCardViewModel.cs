using DarazApp.Entities;

namespace DarazApp.Services
{
    public class ItemCardViewModel
    {
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int? UserId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductPrice { get; set; }
        public int Sizeid { get; set; }
        public int ProductId { get; set; }

        public string SizeName { get; set; } = null!;
        public int ColourId { get; set; }
        public string P_VariationName { get; set; } = null!;

        public string ColourName { get; set; } = null!;
        public int? TotalPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public int? CardDataId { get; set; }
        public ProductBatch ProductBatch { get; set; }
        public ProductVariations ProductVariations { get; set; }

    }
    public class AllOrdersView
    {
        public Product Product { get; set; }
        public User User { get; set; }
    }
    public class AdminChatModel 
    {
        public string? UserName { get; set; }
        public int? UserId { get; set; }
        public string? RoomText { get; set; }
        public int? A_ChatId { get; set; }
        public List<AdminChat> AdminChat { get; set; }
    }
}
