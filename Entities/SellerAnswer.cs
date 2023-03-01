using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class SellerAnswer
{
    public int SellerAnswerId { get; set; }

    public string? SellerAnswer1 { get; set; }

    public DateTime? SellerAnswerDate { get; set; }

    public DateTime? SanswerUpdateOn { get; set; }

    public int? BuyerQuestionId { get; set; }

    public int? UserId { get; set; }

    public virtual BuyerQuestion? BuyerQuestion { get; set; }

    public virtual User? User { get; set; }
}
