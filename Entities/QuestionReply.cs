using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class QuestionReply
{
    public int QreplyId { get; set; }

    public string? ReplyText { get; set; }

    public DateTime? ReplyDate { get; set; }

    public DateTime? RupdateDate { get; set; }

    public int? QuestionId { get; set; }

    public int? UserId { get; set; }

    public virtual QuestionsApp? Question { get; set; }

    public virtual User? User { get; set; }
}
