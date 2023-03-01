namespace DarazApp.Entities
{
    public class App_Table
    {

        public int? App_Id { get; set; }    
        public int? OrderId { get; set; }
        public int? OrderDetails_Id { get; set; }  
        public int? Product_Id { get; set; }    
        public DateTime? Uploadon { get; set; }
        public Double? P_Commision { get; set; }
        public int? ComissionValue { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Orders? Orders { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
    }
}
