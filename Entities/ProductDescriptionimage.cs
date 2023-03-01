using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class ProductDescriptionimage
{
    public int ProductDescriptionImagesId { get; set; }

    public string? ProductdesImage1 { get; set; }
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
