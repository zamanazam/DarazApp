using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class SaleInvoice
{
    public int InvoiceId { get; set; }

    public int? PdescriptionId { get; set; }

    public int? ProductPrice { get; set; }

    public int? ProductQuantity { get; set; }

    public int? Amount { get; set; }

    public int? TotalDiscount { get; set; }

    public int? RemainingAmount { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? UpdateOn { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
