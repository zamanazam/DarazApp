using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class AccountDetail
{
    public int AdetailId { get; set; }

    public int? VoucherAmmount { get; set; }

    public int? Coins { get; set; }

    public int? WithdrawMoney { get; set; }

    public DateTime? UpdateOn { get; set; }

    public int? AccountId { get; set; }

    public string? PaymentOrDepositMethod { get; set; }

    public virtual UserAccount? Account { get; set; }

    public virtual ICollection<MoneyDescription> MoneyDescriptions { get; } = new List<MoneyDescription>();
}
