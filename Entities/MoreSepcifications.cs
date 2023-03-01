namespace DarazApp.Entities
{
    public class MoreSepcifications
    {
        public int MoreSpecificationsId { get; set; }

        public string Label { get; set; }
        public string SpecificationsText { get; set; }
        public int ProductId { get; set; }
        public DateTime UploadOn { get; set; }
        public virtual Product? Product { get; set; }
    }
}
