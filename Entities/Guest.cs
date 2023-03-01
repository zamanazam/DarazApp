namespace DarazApp.Entities
{
    public class Guest
    {

        public int Guestid { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? SecretKey { get; set; } 
        public string? G_Name { get; set; }
        public string? G_Email { get; set; }
        public string? G_Phone { get; set; }

        public virtual ICollection<GuestandStoreChat> GuestandStoreChat { get; set; }
    }
}
