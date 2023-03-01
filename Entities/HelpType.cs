namespace DarazApp.Entities
{
    public class HelpType
    {
        public int TypeId { get; set; }
        public string? TypeText { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    }
}
