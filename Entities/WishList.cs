namespace DarazApp.Entities
{
    public class WishList
    {
        public int WishListid { get; set; }
        public int ProductId { get; set; }
        public DateTime? Date { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public Product? Product { get; set; }

    }
}
