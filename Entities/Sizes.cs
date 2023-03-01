namespace DarazApp.Entities
{
    public class Sizes
    {
        public int Sizeid { get; set; }

        public string SizeName { get; set; } = null!;

        public virtual ICollection<ProductSize> ProductSize { get; set; } = new List<ProductSize>();


    }
}
