using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class PerCategory
{
    public int PerCategoryId { get; set; }

    public string PerCategoryName { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? PerCategoriesIcon { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<KidCategory> KidCategories { get; } = new List<KidCategory>();
}
