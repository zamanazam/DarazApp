using System;
using System.Collections.Generic;

namespace DarazApp.Entities
{
    public class ProductColours
    {
        public int ProductColourId { get; set; }

        //public string? ProductColour { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ColourId { get; set; }
        public virtual Colour Colour{ get; set; }
    }
}
