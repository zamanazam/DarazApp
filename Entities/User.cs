using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarazApp.Entities;

public partial class User
{
    public int UserId { get; set; }
    public string? UserName { get; set; } = null!;
    public string? Password { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public string? PhoneNo { get; set; } = null!;
    public string? Email { get; set; }
    public string? UserImage { get; set; }
    public string? Cnic { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? City { get; set; }
    public string? OTP { get; set; }
    public bool? status { get; set; }
    public string Address { get; set; } = null!;
    public string? FatherName { get; set; }
    public int? Age { get; set; } 
    public int? SecretKey { get; set; }
    public DateTime? Date { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<QuestionReply> QuestionReplies { get; set; } = new List<QuestionReply>();

    public virtual ICollection<QuestionsApp> QuestionsApps { get; set; } = new List<QuestionsApp>();

    public virtual ICollection<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();

    public virtual ICollection<SellerAnswer> SellerAnswers { get; set; } = new List<SellerAnswer>();

    public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<CardData> CardData { get; set; } = new List<CardData>();
    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    public virtual ICollection<WishList> WishList { get; set; } = new List<WishList>();
    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
    public virtual ICollection<ProductComments> ProductComments { get; set; } = new List<ProductComments>();
    public virtual ICollection<ReturnItems> ReturnItems { get; set; } = new List<ReturnItems>();

    //public virtual ICollection<Room> Helps { get; set; } = new List<Room>();

    [InverseProperty("ReceiverChat")]
    public virtual ICollection<Chat> U_Chat { get; set; }

    [InverseProperty("SenderChat")]
    public virtual ICollection<Chat> S_Chat { get; set; }

    [InverseProperty("Admins")]
    public virtual ICollection<Room> R_Admin { get; set; }

    [InverseProperty("Users")]
    public virtual ICollection<Room> R_Users { get; set; }

    [InverseProperty("Assign")]
    public virtual ICollection<Room> R_Assign { get; set; }

    public virtual ICollection<AdminChat> AdminChat { get; set; }

    public virtual ICollection<GuestandStoreChat> GuestandStoreChat { get; set; }
}

