using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class ProductSpecification
{
    public int ProductSpecificationId { get; set; }

    public int ProductId { get; set; }

    public string? ProductSpecificationsText { get; set; }

    public string? ProductDisplay { get; set; }

    public string? ProcessorCapacity { get; set; }

    public string? CameraQuality { get; set; }

    public string? Memory { get; set; }

    public string? Graphics { get; set; }

    public string? ProductWarrantyInfo { get; set; }

    public string? ProductShippingInfo { get; set; }

    public virtual Product Product { get; set; } = null!;
}
