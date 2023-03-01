namespace DarazApp.Services
{
    public class FilterSearchModel
    {
        public int KidId { get; set; }
        public IList<int?> Prices { get; set; }
        public string ProductName { get; set; }
        public IList<int> ColourIds { get; set; }
        public IList<int> SizeIds { get; set; }
        public IList<string> Brands { get; set; }

        public int MinimumPrice { get; set; }
        public int MaximumPrice { get; set; }
    }
}
