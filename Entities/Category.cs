using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;   
    public string? CategoryIcon { get; set; }
    public virtual ICollection<PerCategory> PerCategories { get; set; } = new List<PerCategory>();
}
