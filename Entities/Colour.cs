namespace DarazApp.Entities
{
    public class Colour
    {
        public int ColourId { get; set; }

        public string ColourName { get; set; } = null!;

        public virtual ICollection<ProductColours> ProductColourses { get; set; } = new List<ProductColours>();

    }
}
