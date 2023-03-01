using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DarazApp.Models;

namespace DarazApp.Entities;

public partial class CommerceDbContext : DbContext
{
    public CommerceDbContext()
    {
    }

    public CommerceDbContext(DbContextOptions<CommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountDetail> AccountDetails { get; set; }

    public virtual DbSet<BuyerQuestion> BuyerQuestions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<KidCategory> KidCategories { get; set; }

    public virtual DbSet<MoneyDescription> MoneyDescriptions { get; set; }
    public virtual DbSet<Chat> Chat { get; set; }
    public virtual DbSet<PerCategory> PerCategories { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<CardData> CardData { get; set; }
    public virtual DbSet<Room> Room { get; set; }
    public virtual DbSet<HelpType> HelpType { get; set; }
    public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }
    public virtual DbSet<ProductComments> ProductComments { get; set; }
    public virtual DbSet<ProductDescriptionimage> ProductDescriptionimages { get; set; }
    public virtual DbSet<ProductSize> ProductSize { get; set; }
    public virtual DbSet<ReturnItems> ReturnItems { get; set; }
    public virtual DbSet<Sizes> Sizes { get; set; }
    public virtual DbSet<Orders> Orders { get; set; }
    public virtual DbSet<App_Table> App_Table { get; set; }
    public virtual DbSet<ProductBatch> ProductBatch { get; set; }
    public virtual DbSet<MoreSepcifications> MoreSepcifications{get; set;}
    public virtual DbSet<Guest> Guest{get; set;}
    public virtual DbSet<GuestandStoreChat> GuestandStoreChat{get; set;}

    //public virtual DbSet<OrderStatus> OrderStatus { get; set; }
    //public virtual DbSet<Status> Status { get; set; }
    public virtual DbSet<ProductVariations> ProductVariations { get; set; }
    public virtual DbSet<OrderDetails> OrderDetails { get; set; }
    public virtual DbSet<ProductColours> ProductColours { get; set; }
    public virtual DbSet<Colour> Colour { get; set; }
    public virtual DbSet<WishList> WishList { get; set; }
    public virtual DbSet<AdminChat> AdminChat { get; set; }
    public virtual DbSet<Store> Store { get; set; }
    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }

    public virtual DbSet<QuestionReply> QuestionReplies { get; set; }

    public virtual DbSet<QuestionsApp> QuestionsApps { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SaleInvoice> SaleInvoices { get; set; }

    public virtual DbSet<SellerAnswer> SellerAnswers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserAccount> UserRoleAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=CommerceDb; TrustServerCertificate=True; Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountDetail>(entity =>
        {
            entity.HasKey(e => e.AdetailId).HasName("PK__AccountD__1807A98670FA569A");

            entity.ToTable("AccountDetail");

            entity.Property(e => e.AdetailId).HasColumnName("ADetailId");
            entity.Property(e => e.PaymentOrDepositMethod)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Payment_OR_DepositMethod");
            entity.Property(e => e.UpdateOn).HasColumnType("date");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountDetails)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__AccountDe__Accou__4BAC3F29");
        });


        modelBuilder.Entity<ReturnItems>(entity =>
        {
            entity.HasKey(e => e.R_ItemId).HasName("PK__ReturnItemS__F9C4A9ECEE8E33A1");
            entity.Property(e => e.Ret_Quantity).IsUnicode(false);

            entity.Property(e => e.R_On)
                .HasColumnType("date");

            entity.Property(e => e.R_Number).HasMaxLength(245)
              .IsUnicode(false);

            entity.Property(e => e.ProductImage)
                   .HasMaxLength(2000)
                   .IsFixedLength();

            entity.Property(e => e.Ret_Reason)
                .HasColumnType("text")
                .HasColumnName("Ret_Reason");

            entity.HasOne(d => d.User).WithMany(p => p.ReturnItems)
               .HasForeignKey(d => d.Ret_By)
               .HasConstraintName("FK__ReturnItems__User__7FB5F314").OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.OrderDetails).WithMany(p => p.ReturnItems)
               .HasForeignKey(d => d.Order_Id)
               .HasConstraintName("FK__ReturnItems__OrderDetails__7FB5F314").OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(d => d.Orders).WithMany(p => p.ReturnItems)
               .HasForeignKey(d => d.OrderItems_Id)
               .HasConstraintName("FK__ReturnItems__Orders__7FB5F314").OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.ProductVariations).WithMany(p => p.ReturnItems)
               .HasForeignKey(d => d.Var_Id)
               .HasConstraintName("FK__ReturnItems__ProductVariations__7FB5F314").OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.ProductBatch).WithMany(p => p.ReturnItems)
               .HasForeignKey(d => d.B_id)
               .HasConstraintName("FK__ReturnItems__ProdcutBatch__7FB5F314").OnDelete(DeleteBehavior.NoAction);

        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Guestid).HasName("PK__Guest__F9C4A9ECEE8E33A1");

            entity.Property(e => e.SecretKey)
                      .HasMaxLength(2000)
                      .IsFixedLength();

            entity.Property(e => e.CreatedOn).HasColumnType("date");

            entity.Property(e => e.G_Name).HasColumnType("text");

            entity.Property(e => e.G_Phone).HasColumnType("text");

            entity.Property(e => e.G_Email).HasColumnType("text"); 
            
            entity.Property(e => e.G_Email).HasColumnType("text");

        });  
        
        
        modelBuilder.Entity<GuestandStoreChat>(entity =>
        {
            entity.HasKey(e => e.GS_ChatId).HasName("PK__GuestandStoreChat__F9C4A9ECEE8E33A1");

            entity.Property(e => e.Msg_Status)
                      .HasMaxLength(2000)
                      .IsFixedLength();

            entity.Property(e => e.ChatOn).HasColumnType("date");

            entity.Property(e => e.Seen_On).HasColumnType("date");

            entity.Property(e => e.Msg_Text).HasColumnType("text");


            entity.HasOne(d => d.Guest).WithMany(p => p.GuestandStoreChat)
               .HasForeignKey(d => d.GuestId)
               .HasConstraintName("FK__GuestandStoreChat__Guest__7FB5F314").OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Store).WithMany(p => p.GuestandStoreChat)
               .HasForeignKey(d => d.StoreId)
               .HasConstraintName("FK__GuestandStoreChat__Store__7FB5F314").OnDelete(DeleteBehavior.NoAction);


        }); 



        
        modelBuilder.Entity<BuyerQuestion>(entity =>
        {
            entity.HasKey(e => e.BuyerQuestionId).HasName("PK__BuyerQue__F9C4A9ECEE8E33A1");

            entity.ToTable("BuyerQuestion");

            entity.Property(e => e.BquestionDate)
                .HasColumnType("date")
                .HasColumnName("BQuestionDate");
            entity.Property(e => e.BquestionupdateOn)
                .HasColumnType("date")
                .HasColumnName("BQuestionupdateOn");
            entity.Property(e => e.BuyerQuestion1)
                .HasColumnType("text")
                .HasColumnName("BuyerQuestion");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChatS__19093A0B3366B02C");

            entity.Property(e => e.MessageText).HasColumnType("text");
            entity.Property(e => e.ChatOn).HasColumnType("date");
            entity.Property(e => e.ViewOn).HasColumnType("date");

            entity.Property(e => e.M_Status).IsUnicode(false);

            entity.HasOne(d => d.SenderChat).WithMany(p => p.S_Chat)
               .HasForeignKey(d => d.SenderId)
               .HasConstraintName("FK__Chat__SenderChat__7FB5F314").OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.ReceiverChat).WithMany(p => p.U_Chat)
               .HasForeignKey(d => d.ReceiverId)
               .HasConstraintName("FK__Chat__ReceiverChat__7FB5F314").OnDelete(DeleteBehavior.NoAction);

        });

        modelBuilder.Entity<MoreSepcifications>(entity =>
        {
            entity.HasKey(e => e.MoreSpecificationsId).HasName("PK__MoreSpecificationS__19093A0B3366B02C");

            entity.Property(e => e.Label)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.SpecificationsText).HasColumnType("text");
            entity.Property(e => e.UploadOn).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.MoreSepcifications)
               .HasForeignKey(d => d.ProductId)
               .HasConstraintName("FK__MoreSpecifications__Products__7FB5F314");

        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__CategoriD__19093A0B3366B02C");


            entity.Property(e => e.CategoryIcon)
                .HasMaxLength(2000)
                .IsFixedLength();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(550)
                .IsFixedLength();
        });

        modelBuilder.Entity<HelpType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__HelpTypE__19093A0B3366B02C");

            entity.Property(e => e.TypeText)
                .HasMaxLength(550)
                .IsFixedLength();
        });


        modelBuilder.Entity<AdminChat>(entity => 
        {

            entity.HasKey(e => e.A_ChatId).HasName("PK__AdminChat__19093A0B3366B02C");

            entity.Property(e => e.MessageText).HasColumnType("text");
            entity.Property(e => e.MessageStatus)
                            .HasMaxLength(2000)
                            .IsFixedLength();
            entity.Property(e => e.MessageOn).HasColumnType("date");

            entity.HasOne(d => d.Room).WithMany(p => p.AdminChat)
                .HasForeignKey(d => d.RoomId)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AdminChat__Room__7FB5F314");

            entity.HasOne(d => d.User).WithMany(p => p.AdminChat)
               .HasForeignKey(d => d.MessageBy)
               .HasConstraintName("FK__AdminChat__User__7FB5F314");

        });


        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.R_Id).HasName("PK__RoomS__19093A0B3366B02C");

            entity.Property(e => e.RoomText).HasColumnType("text");

            entity.Property(e => e.UserName).HasColumnType("text");   

            entity.Property(e => e.PhoneNo).HasColumnType("text");

            entity.Property(e => e.Email).HasColumnType("text");

            entity.Property(e => e.SecretKey)
                         .HasMaxLength(2000)
                         .IsFixedLength();

            entity.Property(e => e.RoomStatus)
                            .HasMaxLength(2000)
                            .IsFixedLength();

            entity.Property(e => e.RoomOn).HasColumnType("date");

            entity.Property(e => e.AssignedOn).HasColumnType("date");

            entity.HasOne(d => d.Admins).WithMany(p => p.R_Admin)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__Admins__7FB5F314");


            entity.HasOne(d => d.Users).WithMany(p => p.R_Users)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__Users__7FB5F314");


            entity.HasOne(d => d.Assign).WithMany(p => p.R_Assign)
                .HasForeignKey(d => d.AssignedTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__Assign__7FB5F314");

            entity.HasOne(d => d.HelpType).WithMany(p => p.Rooms)
               .HasForeignKey(d => d.H_Id)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK__Help__HelpTypE__7FB5F314");
        });

        modelBuilder.Entity<KidCategory>(entity =>
        {
            entity.HasKey(e => e.KidCategoryId).HasName("PK__KidCateg__832375C5C9ED1EF4");

            entity.Property(e => e.KidCategoryIcon).IsUnicode(false);
            entity.Property(e => e.KidCategoryName)
                .HasMaxLength(550)
                .IsFixedLength();

            entity.Property(e => e.Comission);

            entity.HasOne(d => d.PerCategory).WithMany(p => p.KidCategories)
                .HasForeignKey(d => d.PerCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KidCatego__PerCa__7FB5F314");
        });

        modelBuilder.Entity<ProductVariations>(entity =>
        {
            entity.HasKey(e => e.P_VariationsID).HasName("PK__ProductVariationS__E3214C562842B9BA");

            entity.Property(e => e.P_VariationsName)
               .HasMaxLength(550)
               .IsFixedLength();

            //entity.Property(e => e.P_VAriationLabel)
            //  .HasMaxLength(550)
            //  .IsFixedLength();

            entity.Property(e => e.UploadOn).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariations)
                .HasForeignKey(d => d.P_Id)
                .HasConstraintName("FK__ProductVariationS__ProductS__4E88ABD4");
        });


       modelBuilder.Entity<App_Table>(entity =>
        {
            entity.HasKey(e => e.App_Id).HasName("PK__App_Id__E3214C562842B9BA");

            entity.Property(e => e.ComissionValue).HasMaxLength(245)
              .IsUnicode(false);

            entity.ToTable("App_TablE");

            entity.Property(e => e.P_Commision);

            entity.Property(e => e.Uploadon).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.App_Tables)
                .HasForeignKey(d => d.Product_Id)
                .HasConstraintName("FK__App_TablE__Product__4E88ABD4");

            entity.HasOne(d => d.Orders).WithMany(p => p.App_Tables)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__App_TablE__OrderS__4E88ABD4"); 
            
            entity.HasOne(d => d.OrderDetails).WithMany(p => p.App_Tables)
                .HasForeignKey(d => d.OrderDetails_Id)
                .HasConstraintName("FK__App_TablE__OrderDetails__4E88ABD4");
        });

        modelBuilder.Entity<MoneyDescription>(entity =>
        {
            entity.HasKey(e => e.WithdarwDate).HasName("PK__MoneyDes__E3214C562842B9BA");

            entity.ToTable("MoneyDescription");

            entity.Property(e => e.AdetailId).HasColumnName("ADetailId");
            entity.Property(e => e.OnDate).HasColumnType("date");

            entity.HasOne(d => d.Adetail).WithMany(p => p.MoneyDescriptions)
                .HasForeignKey(d => d.AdetailId)
                .HasConstraintName("FK__MoneyDesc__ADeta__4E88ABD4");
        });

        modelBuilder.Entity<PerCategory>(entity =>
        {
            entity.HasKey(e => e.PerCategoryId).HasName("PK__PerCateg__1FB2D06C4A7F65F3");

            entity.Property(e => e.PerCategoriesIcon).IsUnicode(false);
            entity.Property(e => e.PerCategoryName)
                .HasMaxLength(550)
                .IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.PerCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PerCatego__Categ__7CD98669");
        });
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__StorE__B40CC6CD28FADC26");

            entity.Property(e => e.StoreName)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.ShippingAddress)
                .IsUnicode(false);
            entity.Property(e => e.Contact)
                .IsUnicode(false);
            entity.Property(e => e.ShippingInfo)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .IsUnicode(false);
            entity.Property(e => e.Createon).HasColumnType("date");
            entity.HasOne(d => d.User).WithMany(p => p.Stores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Store__UserId__52E34C9D");


        });

        modelBuilder.Entity<ProductBatch>(entity =>
        {
            entity.HasKey(e => e.BatchId).HasName("PK__ProductBatcH__B40CC6CD28FADC26");

            entity.Property(e => e.Price)
                .HasMaxLength(2000)
                .IsFixedLength(); 
            
            entity.Property(e => e.RemainingQuantity)
                .HasMaxLength(2000)
                .IsFixedLength();

            entity.Property(e => e.Quantity)
                .HasMaxLength(2000)
                .IsFixedLength();

            entity.Property(e => e.UploadOn).HasColumnType("date");
            entity.Property(e => e.SaleOn).HasColumnType("date");

            entity.HasOne(d => d.ProductVariations).WithMany(p => p.ProductBatches)
                .HasForeignKey(d => d.P_VariationsId)
                .HasConstraintName("FK__ProductBatch__ProductVariations__52E34C9D");
        
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD28FADC26");

            entity.Property(e => e.ProductImage)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.ProductName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("date");

            entity.HasOne(d => d.KidCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.KidCategoryId)
                .HasConstraintName("FK__Products__KidCat__0C1BC9F9");

            entity.HasOne(d => d.User).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Products__UserId__52E34C9D");
        });

        modelBuilder.Entity<ProductDescription>(entity =>
        {
            entity.HasKey(e => e.PdescriptionId).HasName("PK__ProductD__69D4DAB3F307D842");

            entity.ToTable("ProductDescription");

            entity.Property(e => e.PdescriptionId).HasColumnName("PDescriptionId");
            entity.Property(e => e.ProductBrand)
                .HasMaxLength(250)
                .IsUnicode(false);
            //entity.Property(e => e.ProductColor)
            //    .HasMaxLength(250)
            //    .IsUnicode(false);
            entity.Property(e => e.ProductDes).HasColumnType("text");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(2000)
                .IsFixedLength();
            entity.Property(e => e.ProductMaterial)
                .HasMaxLength(550)
                .IsUnicode(false);
            entity.Property(e => e.ProductQuality)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.ProductQuantity)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ProductType).HasColumnType("text");

            entity.HasOne(d => d.Products).WithMany(p => p.ProductDescriptions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductDescription_Products");
        });

        modelBuilder.Entity<ProductDescriptionimage>(entity =>
        {
            entity.HasKey(e => e.ProductDescriptionImagesId).HasName("PK__ProductD__2B5B36E87996C3EB");

            entity.Property(e => e.ProductdesImage1).IsUnicode(false);
        

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDescriptionimages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductDescriptionimages_Products");
        });

        modelBuilder.Entity<CardData>(entity =>
        {
            entity.HasKey(e => e.CardDataId).HasName("PK__CardDatA__2B5B36E87996C3EB");
            entity.Property(e => e.ProductQuantity).IsUnicode(false);
            entity.Property(e => e.ProductPrice).IsUnicode(false);
            entity.Property(e => e.TotalPrice).IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("date");

            entity.HasOne(d => d.ProductVariations).WithMany(p => p.CardData)
                .HasForeignKey(d => d.Variations_Id)
                .HasConstraintName("FK_CardData_ProductVariationS");

            entity.HasOne(d => d.Products).WithMany(p => p.CardData)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CardData_Products");

            entity.HasOne(d => d.ProductBatch).WithMany(p => p.CardData)
              .HasForeignKey(d => d.BatchId)
              .HasConstraintName("FK_CardData_ProductBatch");

            entity.HasOne(d => d.User).WithMany(p => p.CardData)
              .HasForeignKey(d => d.UserId)
              .HasConstraintName("FK_CardData_Users");
        });
        modelBuilder.Entity<OrderDetails>(entity =>
        {
            
            entity.HasKey(e => e.OrderDetailsId).HasName("PK__OrderDetailS__2B5B36E87996C3EB");
            entity.Property(e => e.UserName).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.FatherName).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("date");
            entity.Property(e => e.DeliveredOn).HasColumnType("date");
            entity.Property(e => e.PhoneNo).HasMaxLength(250).HasColumnType("text");
            entity.Property(e => e.OrderStatus).IsUnicode(false);
            entity.Property(e => e.MessageStatus).IsUnicode(false);
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.City).HasMaxLength(250).HasColumnType("text"); 
            entity.Property(e => e.Nationality).HasMaxLength(250).HasColumnType("text");
            entity.Property(e => e.PostalCode).HasMaxLength(250).HasColumnType("text");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.HasOne(d => d.Buyer).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_OrderDetails_Users");

        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderS__2B5B36E87996C3EB");
            entity.Property(e => e.ProductQuantity).IsUnicode(false);
            entity.Property(e => e.TotalPrice).IsUnicode(false);
            entity.Property(e => e.ProductPrice).IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("date");            
            entity.Property(e => e.DeleteOn).HasColumnType("date");
            //entity.Property(e => e.ShippingAddress).HasColumnType("text");
            entity.Property(e => e.IsDeleted).IsUnicode(true);

            entity.HasOne(d => d.ProductBatch).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("FK_Orders_ProductBatcH");

            entity.HasOne(d => d.OrderDetails).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderDetailsId)
                .HasConstraintName("FK_Orders_OrderDetails");

            entity.HasOne(d => d.ProductVariations).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VariationsId)
                .HasConstraintName("FK_Orders_ProductVariationS");

            entity.HasOne(d => d.Products).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Orders_Products");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
              .HasForeignKey(d => d.StoreId)
              .HasConstraintName("FK_Orders_Users");

        });
        modelBuilder.Entity<ProductComments>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__CommentiD__2B5B36E87996C3EB");

            entity.Property(e => e.CommentDescription).HasColumnType("text");

            entity.Property(e => e.Rating).IsUnicode(false);

            entity.Property(e => e.OrderDetailsId).IsUnicode(false);

            entity.Property(e => e.CommentedOn).HasColumnType("date");

            entity.HasOne(d => d.ProductVariations).WithMany(p => p.ProductComments)
               .HasForeignKey(d => d.VariationId)
               .HasConstraintName("FK_ProductComment_ProductVariations");

            entity.HasOne(d => d.OrderDetails).WithMany(p => p.ProductComments)
                .HasForeignKey(d => d.OrderDetailsId)
                .HasConstraintName("FK_ProductComment_OrderDetails");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductComments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductComment_Product");

            entity.HasOne(d => d.User).WithMany(p => p.ProductComments)
              .HasForeignKey(d => d.UserId)
              .HasConstraintName("FK_ProductComment_User");
        });
        //modelBuilder.Entity<Status>(entity =>
        //{
        //    entity.HasKey(e => e.StatusId).HasName("PK__StatuS__2B5B36E87996C3EB");

        //    entity.Property(e => e.StatusName).IsUnicode(false);

        //});

        //modelBuilder.Entity<OrderStatus>(entity =>
        //{
        //    entity.HasKey(e => e.OrderStatusId).HasName("PK__OrderStatuS__2B5B36E87996C3EB");

        //    entity.HasOne(d => d.Orders).WithMany(p => p.OrderStatus)
        //       .HasForeignKey(d => d.OrderId)
        //       .HasConstraintName("FK_OrderStatus_Orders");

        //    entity.HasOne(d => d.Status).WithMany(p => p.OrderStatus)
        //      .HasForeignKey(d => d.StatusId)
        //      .HasConstraintName("FK_OrderStatus_Status");


        //});

        modelBuilder.Entity<Sizes>(entity =>
        {
            entity.HasKey(e => e.Sizeid).HasName("PK__SizeS__2B5B36E87996C3EB");

            entity.Property(e => e.SizeName).IsUnicode(false);

        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.ProductSizeId).HasName("PK__ProductD__2B5B36E87996C3EB");

            //entity.Property(e => e.ProductSizes).IsUnicode(false);


            entity.HasOne(d => d.Product).WithMany(p => p.ProductSize)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductSize_Products");

            entity.HasOne(d => d.Sizes).WithMany(p => p.ProductSize)
              .HasForeignKey(d => d.Sizeid)
              .HasConstraintName("FK_ProductSize_Sizes");
        });

        modelBuilder.Entity<Colour>(entity =>
        {
            entity.HasKey(e => e.ColourId).HasName("PK__ColouR__2B5B36E87996C3EB");

            entity.Property(e => e.ColourName).IsUnicode(false);

        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListid).HasName("PK__WishLisT__2B5B36E87996C3EB");

            entity.HasOne(d => d.Product).WithMany(p => p.WishList)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_WishList_Products");

            entity.HasOne(d => d.User).WithMany(p => p.WishList)
               .HasForeignKey(d => d.UserId)
               .HasConstraintName("FK_WishList_User");
        });


        modelBuilder.Entity<ProductColours>(entity =>
        {
            entity.HasKey(e => e.ProductColourId).HasName("PK__ProductD__2B5B36E87996C3EB");

            //entity.Property(e => e.ProductColour).IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductColours)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductColours_Products");

            entity.HasOne(d => d.Colour).WithMany(p => p.ProductColourses)
               .HasForeignKey(d => d.ColourId)
               .HasConstraintName("FK_ProductColours_Colour");
        });

        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.HasKey(e => e.ProductSpecificationId).HasName("PK__ProductS__BAE405AD472B8D98");

            entity.ToTable("ProductSpecification");

            entity.Property(e => e.CameraQuality)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.Graphics)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.Memory)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.ProcessorCapacity)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.ProductDisplay)
                .HasMaxLength(245)
                .IsUnicode(false);
            entity.Property(e => e.ProductShippingInfo).IsUnicode(false);
            entity.Property(e => e.ProductSpecificationsText).IsUnicode(false);
            entity.Property(e => e.ProductWarrantyInfo).IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSpecifications)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSp__Produ__473C8FC7");
        });

        modelBuilder.Entity<QuestionReply>(entity =>
        {
            entity.HasKey(e => e.QreplyId).HasName("PK__Question__57DF196DDA2AE2EE");

            entity.ToTable("QuestionReply");

            entity.Property(e => e.QreplyId).HasColumnName("QReplyId");
            entity.Property(e => e.ReplyDate).HasColumnType("date");
            entity.Property(e => e.ReplyText).HasColumnType("text");
            entity.Property(e => e.RupdateDate)
                .HasColumnType("date")
                .HasColumnName("RUpdateDate");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionReplies)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__QuestionR__Quest__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.QuestionReplies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__QuestionR__UserI__0B5CAFEA");
        });

        modelBuilder.Entity<QuestionsApp>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACA8E87817");

            entity.ToTable("QuestionsApp");

            entity.Property(e => e.QuestionDate).HasColumnType("date");
            entity.Property(e => e.QuestionText).HasColumnType("text");
            entity.Property(e => e.Questiontype).HasColumnType("text");
            entity.Property(e => e.QupdateDate)
                .HasColumnType("date")
                .HasColumnName("QUpdateDate");

            entity.HasOne(d => d.User).WithMany(p => p.QuestionsApps)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Questions__UserI__0F2D40CE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1ADBAF67DF");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SaleInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__SaleInvo__D796AAB526A08F71");

            entity.ToTable("SaleInvoice");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.PdescriptionId).HasColumnName("PDescriptionId");
            entity.Property(e => e.UpdateOn).HasColumnType("date");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleInvoices)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__SaleInvoi__Produ__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.SaleInvoices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SaleInvoi__UserI__09746778");
        });

        modelBuilder.Entity<SellerAnswer>(entity =>
        {
            entity.HasKey(e => e.SellerAnswerId).HasName("PK__SellerAn__2B8337336CCFD926");

            entity.ToTable("SellerAnswer");

            entity.Property(e => e.SanswerUpdateOn)
                .HasColumnType("date")
                .HasColumnName("SAnswerUpdateOn");
            entity.Property(e => e.SellerAnswer1)
                .HasColumnType("text")
                .HasColumnName("SellerAnswer");
            entity.Property(e => e.SellerAnswerDate).HasColumnType("date");

            entity.HasOne(d => d.BuyerQuestion).WithMany(p => p.SellerAnswers)
                .HasForeignKey(d => d.BuyerQuestionId)
                .HasConstraintName("FK__SellerAns__Buyer__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.SellerAnswers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SellerAns__UserI__0E391C95");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C70963D30");
            entity.Property(e=>e.status).IsUnicode(true);
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.City)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Cnic)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OTP)
              .HasMaxLength(250)
              .IsUnicode(false);
            entity.Property(e => e.UserImage)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A356D197C68");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__RoleI__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__UserI__44FF419A");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__UserRole__349DA5A6303DE993");

            entity.ToTable("UserRoleAccount");

            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoleA__UserI__0880433F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<DarazApp.Models.ForgetPasswordModel> ForgetPasswordModel { get; set; }
}
