using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class ProductDescription
{
    public int PdescriptionId { get; set; }

    public string? ProductType { get; set; }

    public string? ProductQuality { get; set; }

    //public string? ProductColor { get; set; }

    public string? ProductDes { get; set; }

    public int ProductQuantity { get; set; }

    public string? ProductImage { get; set; }

    public int? ProductId { get; set; }

    public string? ProductBrand { get; set; }

    public string? ProductMaterial { get; set; }

    public virtual Product? Products { get; set; }
}
