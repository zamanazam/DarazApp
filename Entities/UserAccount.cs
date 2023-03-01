using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class UserAccount
{
    public int AccountId { get; set; }

    public int? Deposits { get; set; }

    public DateTime? Date { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<AccountDetail> AccountDetails { get; } = new List<AccountDetail>();

    public virtual User? User { get; set; }
}
