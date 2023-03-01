namespace DarazApp.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        
        public int UserId { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string Contact { get; set; } = null!;  
        public string StoreName { get; set; } = null!;
        public string Email { get; set; } = null!;     
        public string ShippingInfo { get; set; } = null!;
        public DateTime? Createon { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
