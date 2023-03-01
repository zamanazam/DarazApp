using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class KidCategory
{
    public int KidCategoryId { get; set; }

    public string KidCategoryName { get; set; } = null!;
    public double? Comission { get; set; }
    public int PerCategoryId { get; set; }

    public string? KidCategoryIcon { get; set; }

    public virtual PerCategory PerCategory { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
