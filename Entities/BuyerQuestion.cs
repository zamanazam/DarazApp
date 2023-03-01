using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class BuyerQuestion
{
    public int BuyerQuestionId { get; set; }

    public string? BuyerQuestion1 { get; set; }

    public DateTime? BquestionDate { get; set; }

    public DateTime? BquestionupdateOn { get; set; }

    public virtual ICollection<SellerAnswer> SellerAnswers { get; } = new List<SellerAnswer>();
}
