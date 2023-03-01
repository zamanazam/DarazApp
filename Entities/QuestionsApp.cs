using System;
using System.Collections.Generic;

namespace DarazApp.Entities;

public partial class QuestionsApp
{
    public int QuestionId { get; set; }

    public string? Questiontype { get; set; }

    public string? QuestionText { get; set; }

    public DateTime? QuestionDate { get; set; }

    public DateTime? QupdateDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<QuestionReply> QuestionReplies { get; } = new List<QuestionReply>();

    public virtual User? User { get; set; }
}
