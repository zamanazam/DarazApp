using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class MoneyDescription
{
    public int WithdarwDate { get; set; }

    public int? WithdrawAmount { get; set; }

    public int? DepositMoney { get; set; }

    public DateTime? OnDate { get; set; }

    public int? AdetailId { get; set; }

    public virtual AccountDetail? Adetail { get; set; }
}
