namespace DarazApp.Entities
{
	public class OrderDetails
	{
		public int OrderDetailsId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
		public string? FatherName { get; set; }
		public string? Email { get; set; }
        public int? OrderStatus { get; set; }
		public string? PhoneNo { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
        public string? Nationality { get; set; }
        public string? PostalCode { get; set; }
        public string? Message { get; set; }
        public int? MessageStatus { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<ProductComments> ProductComments { get; set; }
        //public virtual ICollection<App_Table> App_Tables { get; set; } = new List<App_Table>();
        public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>(); 
        public virtual ICollection<App_Table> App_Tables { get; set; } = new List<App_Table>();
        public virtual User? Buyer { get; set; }

    }
}
