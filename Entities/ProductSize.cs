using System;
using System.Collections.Generic;

namespace DarazApp.Entities
{
    public class ProductSize
    {
        public int ProductSizeId { get; set; }

        //public string? ProductSizes { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Sizeid { get; set; }
        public virtual Sizes? Sizes { get; set; }
    }
}
