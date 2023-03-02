using DarazApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.Collections.Immutable;
using System.Data;
using System.Drawing;
using DarazApp.Entities;
using DarazApp.Services;
using Microsoft.AspNetCore.Http;
using DarazApp.Controllers;
using static DarazApp.Services.UserService;
using static System.Reflection.Metadata.BlobBuilder;
using System.Collections;
using System.IO;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using DarazApp.Helpers;
using System.Security.Claims;
using NuGet.Versioning;
using Azure;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static DarazApp.Services.Program;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO.Pipelines;
using System.Collections.Generic;
using NuGet.Packaging;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.VisualBasic;
using EllipticCurve.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Packaging.Rules;
//using Microsoft.AspNetCore.Authorization;

namespace DarazApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Data()
        {
            return View();
        }
        private readonly CommerceDbContext _commerceDb;
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;
        private readonly ViewModel _model = new ViewModel();
        private readonly SaveData _savedata = new SaveData();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, IUserService userService, CommerceDbContext context, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _commerceDb = context;
            _httpContextAccessor = contextAccessor;

        }
        //[HttpGet]
        public IActionResult IndexData()
        {
            ViewModel model = new ViewModel();
            var data = _commerceDb.Products.ToList();
            var desdata = _commerceDb.ProductDescriptions.ToList();
            model.ProductVm = data;
            model.ProductDescriptionVm = desdata;
            var categ = _commerceDb.Categories.ToList();
            model.categoriesvm = categ;
            return Json(model);

        }
        public IActionResult GetOnlyProducts()
        {
            var product = _commerceDb.Products.Where(x=>x.User.status != false).OrderByDescending(x => x.UploadDate).Select(y=> new ProductSelectView()
            {
                ProductName = y.ProductName,
                ProductImage = y.ProductImage,
                UploadDate = y.UploadDate,
                ProductId = y.ProductId,

                ProductViewModels = new ProductViewModel()
                {
                    //wishList = y.WishList.Where(g=>g.ProductId == y.ProductId).FirstOrDefault(),
                    productVariations = y.ProductVariations.Where(d => d.P_Id == y.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                }
            }).ToList();
            return Json(product);
        }
        [Authorize]
        public IActionResult GetOnlyProductsforWishList()
        {
            var userid = _userService.GetUserId();
            var product = _commerceDb.Products.Where(x=>x.User.status != false).OrderByDescending(x => x.UploadDate).Select(y=> new ProductSelectView()
            {
                ProductName = y.ProductName,
                ProductImage = y.ProductImage,
                UploadDate = y.UploadDate,
                ProductId = y.ProductId,
                wishList = y.WishList.Where(g => g.ProductId == y.ProductId && g.UserId == userid).ToList(),
                ProductViewModels = new ProductViewModel()
                {
                    productVariations = y.ProductVariations.Where(d => d.P_Id == y.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                }
            }).ToList();
            return Json(product);
        }

        public IActionResult GetOnlyCategories()
        {
            List<Category> categories = _commerceDb.Categories.ToList();
            return Json(categories);
        }

        public IActionResult AdminIndexView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoggedinIndex()
        {

            return View();
        }
        [HttpGet]
        public IActionResult GetRoles()
        {
            List<Role> role = _commerceDb.Roles.ToList();
            return Json(role);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserViewModel save)
        {
            if (save.RoleId == ((int)RolesEnum.Store))
            {

                User user = new User();
                string imagespaths = "";
                if (save.file != null)
                {
                    string filepath = Path.GetFileName(save.file.FileName);
                    imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                } 
                user.UserImage = imagespaths;
                user.UserName = save.UserName;
                user.Password = save.Password;
                user.PhoneNo = save.PhoneNo;
                user.FatherName = save.FatherName;
                user.Email = save.Email;
                user.Age = save.Age;
                user.City = save.City;
                user.PostalCode = save.PostalCode;
                user.Cnic = save.Cnic;
                user.Date = DateTime.Now;
                user.Nationality = save.Nationality;
                user.Address = save.Address;
                user.Age = save.Age;
                _commerceDb.Users.Add(user);
                _commerceDb.SaveChanges();
                var latesuserid = user.UserId;

                UserRole userRole = new UserRole();
                userRole.UserId = latesuserid;
                userRole.RoleId = save.RoleId;
                _commerceDb.UserRoles.Add(userRole);
                _commerceDb.SaveChanges();

                Store store = new Store();
                store.StoreName = save.StoreName;
                store.ShippingInfo = save.ShippingInfo;
                store.ShippingAddress = save.ShippingAddress;
                store.Contact = save.Contact;
                store.Email = save.StoreEmail;
                store.Createon = DateTime.Now;
                store.UserId = latesuserid;
                _commerceDb.Store.Add(store);
                _commerceDb.SaveChanges();

            }
            else
            {
                User user = new User();
                string imagespaths = "";
                if (save.file != null)
                {
                    string filepath = Path.GetFileName(save.file.FileName);
                    imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                }
                user.UserImage = imagespaths;
                user.UserName = save.UserName;
                user.Password = save.Password;
                user.PhoneNo = save.PhoneNo;
                user.FatherName = save.FatherName;
                user.Email = save.Email;
                user.Age = save.Age;
                user.City = save.City;
                user.Cnic = save.Cnic;
                user.Date = DateTime.Now;
                user.Nationality = save.Nationality;
                user.Address = save.Address;
                user.Age = save.Age;
                _commerceDb.Users.Add(user);
                _commerceDb.SaveChanges();

                UserRole userRole = new UserRole();
                userRole.UserId = user.UserId;
                userRole.RoleId = save.RoleId;
                _commerceDb.UserRoles.Add(userRole);
                _commerceDb.SaveChanges();

            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateRegister(UserViewModel save)
        {
            User user21 = _commerceDb.Users.Where(x => x.UserId == save.UserId || x.SecretKey == save.secretkey).FirstOrDefault();
            List<User> user1 = _commerceDb.Users.Where(x => x.UserId == save.UserId || x.SecretKey == save.secretkey).ToList();
            int latesuserid = 0;
            foreach (var user in user1)
            {
                string imagespaths = "";
                if (save.file != null)
                {
                    string filepath = Path.GetFileName(save.file.FileName);
                    imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                }
                user.UserImage = imagespaths;
                user.UserName = save.UserName;
                user.Password = save.Password;
                user.PhoneNo = save.PhoneNo;
                user.FatherName = save.FatherName;
                user.Email = save.Email;
                user.Age = save.Age;
                user.City = save.City;
                user.Gender = save.Gender;
                user.PostalCode = save.PostalCode;
                user.Cnic = save.Cnic;
                user.Date = DateTime.Now;
                user.Nationality = save.Nationality;
                user.Address = save.Address;
                user.Age = save.Age;
                _commerceDb.Users.Update(user);
                _commerceDb.SaveChanges();
             
            }
            int user2id = _commerceDb.Users.Where(x => x.UserId == save.UserId && x.SecretKey == save.secretkey).Select(x => x.UserId).FirstOrDefault();
            UserRole role = _commerceDb.UserRoles.Where(x => x.UserId == user2id).FirstOrDefault();
            List<UserRole> userRoles = _commerceDb.UserRoles.Where(x => x.UserId == user2id).ToList();
            foreach (var item in userRoles)
            {
                item.UserId = user2id;
                item.RoleId = save.RoleId;
                _commerceDb.UserRoles.Update(item);
                _commerceDb.SaveChanges();
            }
            return Json(user2id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Firstpage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }
        [Authorize]
        [HttpGet]
        public IActionResult RedirectingPages()
        {
            var userid = _userService.GetUserId();
            int Roleid = _commerceDb.UserRoles.Where(x => x.UserId == userid).Select(x => x.RoleId).FirstOrDefault();
            return Json(Roleid);
        }

        [HttpPost]
        public IActionResult AddStoreChatbyNewGuest(string Email,string UserName,string HelpText,string PhoneNumber,int storeid, string Address)
        {
            char[] charArr = "0123456789".ToCharArray();
            string OTP = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 9; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!OTP.Contains(charArr.GetValue(pos).ToString())) OTP += charArr.GetValue(pos);
                else i--;
            }
            int? secretkey = Convert.ToInt32(OTP);
            //Guest guest = new Guest();
            //guest.G_Name = UserName;
            //guest.G_Email = Email;
            //guest.G_Phone = PhoneNumber;
            //guest.SecretKey = secretkey;
            //guest.CreatedOn = DateAndTime.Now;
            //_commerceDb.Guest.Add(guest);
            //_commerceDb.SaveChanges();
            //int? G_Id = guest.Guestid;

            User user = new User();
            user.UserName = UserName;
            user.Email = Email;
            user.PhoneNo = PhoneNumber;
            user.SecretKey = secretkey;
            user.Date = DateAndTime.Now;
            user.Address = Address;
            user.Password = "UnAssigned";
            _commerceDb.Users.Add(user);
            _commerceDb.SaveChanges();
            var usersIds = user.UserId;
            UserRole userRole = new UserRole();
            userRole.UserId = usersIds;
            userRole.RoleId = (int)RolesEnum.Guest;
            _commerceDb.UserRoles.Add(userRole);
            _commerceDb.SaveChanges();


            Chat chat = new Chat();
            chat.SenderId = usersIds;
            chat.ReceiverId = storeid;
            chat.MessageText = HelpText;
            chat.ChatOn = DateAndTime.Now;
            chat.M_Status = (int)MessageStatus.UnRead;
            _commerceDb.Chat.Add(chat);
            _commerceDb.SaveChanges();
            //GuestandStoreChat guestandStore = new GuestandStoreChat();
            //guestandStore.ChatOn = DateAndTime.Now;
            //guestandStore.Msg_Text = HelpText;
            //guestandStore.StoreId = storeid;
            //guestandStore.GuestId = G_Id;
            //guestandStore.Msg_Status = (int)MessageStatus.UnRead;
            //_commerceDb.GuestandStoreChat.Add(guestandStore);
            //_commerceDb.SaveChanges();
            //var newch = guestandStore.GS_ChatId;
            return Json(secretkey);

        }
        public IActionResult AdminAccountUpdate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSingleBaatch(int VariationId, int pricesval, int quatival)
        {
            ProductBatch batch = new ProductBatch();
            batch.P_VariationsId = VariationId;
            batch.Price = pricesval;
            batch.Quantity = quatival;
            batch.UploadOn = DateAndTime.Now.Date;
            _commerceDb.ProductBatch.Add(batch);
            _commerceDb.SaveChanges();
            var id = batch.BatchId;
            return Json(id);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        ProductDescription productDescription = new ProductDescription();

        [Authorize]
        [HttpGet]
        public ViewResult GetAllProducts()
        {
            var users = _userService.GetAll();
            return View(users);
        }
        public List<CommerceDbContext> commerceDbContexts = new List<CommerceDbContext>();
        //[Authorize]
        [HttpGet]
        public IActionResult GetProductbyIdAdmin()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetProductbyId()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProductbyIdStoreView()
        {
            return View();
        }
        //[Authorize]
        [HttpGet]
        public JsonResult GetProductbyIds(int id)
        {
            Product saved = new Product();
            ViewModel viewModel = new ViewModel();

            var data = _commerceDb.Products.Where(x => x.ProductId == id).ToList();
            var dsrs = _commerceDb.Products.Where(x => x.ProductId != id).ToList();
            var prddata = _commerceDb.ProductDescriptions.Where(x => x.ProductId == id).ToList();
            var kidcatdata = _commerceDb.KidCategories.Where(x => x.KidCategoryId == id).ToList();
            var proddescripimages = _commerceDb.ProductDescriptionimages.Where(x => x.ProductId == id).ToList();
            var productSizes = _commerceDb.ProductSize.Where(x => x.ProductId == id).ToList();
            var sizesids = _commerceDb.ProductSize.Where(x => x.ProductId == id).Select(x => x.Sizeid).ToList();
            var sizis = _commerceDb.Sizes.Where(x => sizesids.Contains(x.Sizeid)).ToList();
            List<User> Userdata = _commerceDb.Products.Where(x => x.ProductId == id).Select(x => x.User).ToList();

            //var sizess = _commerceDb.Sizes.Where(x=>x.Sizeid == sizesids).ToList();
            viewModel.usersvm = Userdata;
            viewModel.Sizesvm = sizis;
            viewModel.ProductVm = data;
            viewModel.similarproducts = dsrs;
            viewModel.ProductDescriptionVm = prddata;
            viewModel.kidCategoriesvm = kidcatdata;
            viewModel.productDescriptionimages = proddescripimages;
            viewModel.productSizesvm = productSizes;
            //viewModel.Sizesvm = sizess;
            return Json(viewModel);
        }
        public IActionResult GetProductsDates()
        {
            return View();
        }
        public List<ProductSize> productSizes = new List<ProductSize>();
        [Authorize]
        public IActionResult ProductSpecification(int id)
        {
            Sizes productSie = new Sizes();
            SaveData saveData = new SaveData();
            ViewModel viewModel = new ViewModel();
            using (var db = new CommerceDbContext())
            {
                productSizes = db.ProductSize.Where(x => x.ProductId == id).ToList();
                saveData.ProductSizes = productSie.SizeName;
                //saveData.ProductId = ConproductSie.ProductId;
            }
            return View(saveData);
        }
        public List<CommerceDbContext> commerceDbs = new List<CommerceDbContext>();
        public List<Product> products = new List<Product>();
        public List<KidCategory> kidcatses = new List<KidCategory>();

        public IActionResult AddBatchView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBatch(BatchAddModel saveData)
        {
           
            
            if (saveData.pquantity != null || saveData.pprice != null)
            {
                var batch = _commerceDb.ProductBatch.Where(x => saveData.VariationId.Contains(x.P_VariationsId)).ToList();
                if (batch.Count == null || batch.Count == 0)
                {
                    CommerceDbContext dbContext = new CommerceDbContext();
                    var val = 0;
                    foreach (var item in saveData.VariationId)
                    {
                        var val1 = val++;
                        ProductBatch productBatch = new ProductBatch();
                        productBatch.P_VariationsId = item;
                        productBatch.Quantity = saveData.pquantity.ElementAtOrDefault(val1);
                        productBatch.RemainingQuantity = saveData.pquantity.ElementAtOrDefault(val1);
                        productBatch.Price = saveData.pprice.ElementAtOrDefault(val1);
                        productBatch.UploadOn = DateTime.Now;
                        dbContext.ProductBatch.Add(productBatch);
                        dbContext.SaveChanges();
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult DashboarView()
        {
            return View();
        }
        [Authorize]
        public IActionResult TotalSalebyCurrentStore()
        {
            SaleCurrentUser saleCurrentUser = new SaleCurrentUser();
            var userid = _userService.GetUserId();
            var MonthlySale = _commerceDb.Orders.Where(x => x.StoreId == userid).ToList();
           
            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            var CurrentMonth = _commerceDb.Orders.Where(x => x.UploadDate > first 
                                        && x.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered 
                                        && x.IsDeleted != true && x.StoreId == userid).Sum(x=>x.Net_Price);
            saleCurrentUser.MonthlySale = CurrentMonth;
             
            var Year = new DateTime(now.Year);
            var CurrentYear = _commerceDb.Orders.Where(x => x.UploadDate > Year
                                                && x.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered 
                                                && x.IsDeleted != true && x.StoreId == userid).Sum(x=>x.Net_Price);
            saleCurrentUser.YearlySale = CurrentYear;

            var TotalSale = _commerceDb.Orders.Where(y => y.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered
                                                && y.IsDeleted != true && y.StoreId == userid).Sum(y => y.Net_Price);
            saleCurrentUser.TotalSale = TotalSale;

            //var PendingOrders = _commerceDb.Orders.Where(z => z.OrderDetails.OrderStatus != (int)ShippingStatus.Delivered 
            //                                                    && z.OrderDetails.OrderStatus != (int)ShippingStatus.Cancelled
            //                                                            && z.IsDeleted == false && z.StoreId == userid).ToList();

            //var PendingOrders = _commerceDb.OrderDetails.Where(z => (z.OrderStatus != (int)ShippingStatus.Delivered && z.Orders.Any(u => u.StoreId == userid)) 
            //                                                                || (z.OrderStatus != (int)ShippingStatus.Cancelled && z.Orders.Any(g => g.StoreId == userid))
            //                                                        || (z.Orders.Any(m => m.IsDeleted == false) && z.Orders.Any(n => n.StoreId == userid))).ToList();
            
            var PendingOrders = _commerceDb.OrderDetails.Where(z => (z.OrderStatus == (int)ShippingStatus.Pending && z.Orders.Any(u => u.StoreId == userid))).ToList();
            saleCurrentUser.Pendings = PendingOrders;

            var DeliveredOrders = _commerceDb.Orders.Where(u => u.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered && u.StoreId == userid).ToList();
            var AllOrders = _commerceDb.Orders.Where(o => o.StoreId == userid).ToList();
            var Progress = (DeliveredOrders.Count  * 100 / AllOrders.Count);

            saleCurrentUser.ProgressSale = Progress;

            var TotalSalewithComission = _commerceDb.Orders.Where(y => y.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered
                                               && y.IsDeleted != true && y.StoreId == userid).Sum(y => y.TotalPrice);
            saleCurrentUser.TotalSaleWithCommision = TotalSalewithComission;

            return Json(saleCurrentUser);
        }


        public IActionResult TotalSale()
        {
            SaleCurrentUser saleCurrentUser = new SaleCurrentUser();
           
            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            var CurrentMonth = _commerceDb.App_Table.Where(x => x.Uploadon > first).Sum(x=>x.ComissionValue);
            saleCurrentUser.MonthlySale = CurrentMonth;

            var Year = new DateTime(now.Year);
            var CurrentYear = _commerceDb.App_Table.Where(x => x.Uploadon > Year).Sum(x => x.ComissionValue);
            saleCurrentUser.YearlySale = CurrentYear;

            var TotalSale = _commerceDb.App_Table.Sum(y => y.ComissionValue);
            saleCurrentUser.TotalSale = TotalSale;

            //var PendingOrders = _commerceDb.Orders.Where(z => z.OrderDetails.OrderStatus != (int)ShippingStatus.Delivered
            //                                                    && z.OrderDetails.OrderStatus != (int)ShippingStatus.Cancelled
            //                                                            && z.IsDeleted == false).ToList();  
            
            var PendingOrders = _commerceDb.Orders.Where(z => z.OrderDetails.OrderStatus == (int)ShippingStatus.Pending).ToList();
            saleCurrentUser.PendingOrders = PendingOrders;

            var DeliveredOrders = _commerceDb.Orders.Where(u => u.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered).ToList();
            var AllOrders = _commerceDb.Orders.ToList();

            var Progress = (DeliveredOrders.Count * 100 / AllOrders.Count);
            saleCurrentUser.ProgressSale = Progress;

            var TotalSalewithComission = _commerceDb.Orders.Where(y => y.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered
                                               && y.IsDeleted != true).Sum(y => y.TotalPrice);

            saleCurrentUser.TotalSaleWithCommision = TotalSalewithComission;
            return Json(saleCurrentUser);
        }
        public IActionResult AllOrders()
        {
            var status = _commerceDb.OrderDetails.ToList();
            return Json(status);
        }

        public IActionResult GetStatus()
        {
            var Status = _commerceDb.OrderDetails.Where(x=>x.OrderStatus == (int)ShippingStatus.Pending).ToList();
            return Json(Status);
        }
        [Authorize]
        public IActionResult GetOrdersbyStatusbyCurrentUser()
        {
            var userid = _userService.GetUserId();
            var dataof = _commerceDb.OrderDetails.Where(x => x.OrderStatus != (int)ShippingStatus.Delivered
                                                        && x.OrderStatus != (int)ShippingStatus.Cancelled && x.Orders.Any(x=>x.StoreId == userid))
           .Select(x => new CheckInSelectView()
           {
               UserId = x.UserId,
               ODEmail = x.Email,
               ODNationality = x.Nationality,
               ODPhoneNo = x.PhoneNo,
               ODUserName = x.UserName,
               Stauts = x.OrderStatus,
               StatusName = ((ShippingStatus)x.OrderStatus).ToString(),
               Address = x.Address,
               UploadDate = x.UploadDate,
               ODOrderdetailId = x.OrderDetailsId,
               OrderHistoryView = _commerceDb.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.StoreId == userid).Select(y => new OrderHistoryView()
               {
                   StoreId = y.Store.UserId,
                   StoreName = y.Store.UserName,
                   StoreEmail = y.Store.Email,
                   StoreImage = y.Store.UserImage,
                   StorePhone = y.Store.PhoneNo,
                   NetPrice = y.Net_Price,
                   ProductId = y.ProductId,
                   ProductQuantity = y.ProductQuantity,
                   TotalPrice = y.TotalPrice,
                   ProductName = y.Products.ProductName,
                   ProductImage = y.Products.ProductImage,
                   ProductPrice = y.Products.ProductPrice,
                   ProductVariations = y.ProductVariations,
                   ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
                   {
                       CommentDescription = z.CommentDescription,
                       Rating = z.Rating,
                   }).ToList(),

               }).ToList(),

           }).ToList();

            return Json(dataof);
        }

        //public IActionResult GetOrdersbyStatus()
        //{
        //    var userid = _userService.GetUserId();
        //    var dataof = _commerceDb.OrderDetails.Where(x => x.OrderStatus != (int)ShippingStatus.Delivered
        //                                                && x.OrderStatus != (int)ShippingStatus.Cancelled)
        //   .Select(x => new CheckInSelectView()
        //   {
        //       UserId = x.UserId,
        //       ODEmail = x.Email,
        //       ODNationality = x.Nationality,
        //       ODPhoneNo = x.PhoneNo,
        //       ODUserName = x.UserName,
        //       Stauts = x.OrderStatus,
        //       StatusName = ((ShippingStatus)x.OrderStatus).ToString(),
        //       Address = x.Address,
        //       UploadDate = x.UploadDate,
        //       ODOrderdetailId = x.OrderDetailsId,
        //       OrderHistoryView = _commerceDb.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.StoreId == userid).Select(y => new OrderHistoryView()
        //       {
        //           StoreId = y.Store.UserId,
        //           StoreName = y.Store.UserName,
        //           StoreEmail = y.Store.Email,
        //           StoreImage = y.Store.UserImage,
        //           StorePhone = y.Store.PhoneNo,
        //           NetPrice = y.Net_Price,
        //           ProductId = y.ProductId,
        //           ProductQuantity = y.ProductQuantity,
        //           TotalPrice = y.TotalPrice,
        //           ProductName = y.Products.ProductName,
        //           ProductImage = y.Products.ProductImage,
        //           ProductPrice = y.Products.ProductPrice,
        //           ProductVariations = y.ProductVariations,
        //           ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
        //           {
        //               CommentDescription = z.CommentDescription,
        //               Rating = z.Rating,
        //           }).ToList(),

        //       }).ToList(),

        //   }).ToList();

        //    return Json(dataof);
        //}

        public IActionResult GetOrdersbyStatus()
        {
            var dataof = _commerceDb.OrderDetails.Where(x => x.OrderStatus != (int)ShippingStatus.Delivered 
                                                        || x.OrderStatus != (int)ShippingStatus.Cancelled || x.Orders.Any(u=>u.IsDeleted == false))
           .Select(x => new CheckInSelectView()
           {
               UserId = x.UserId,
               ODEmail = x.Email,
               ODNationality = x.Nationality,
               ODPhoneNo = x.PhoneNo,
               ODUserName = x.UserName,
               Stauts = x.OrderStatus,
               StatusName =((ShippingStatus) x.OrderStatus).ToString(),
               Address = x.Address,
               UploadDate = x.UploadDate,
               ODOrderdetailId = x.OrderDetailsId,
               OrderHistoryView = _commerceDb.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
               {
                  StoreId = y.Store.UserId,
                  StoreName = y.Store.UserName,
                  StoreEmail = y.Store.Email,
                  StoreImage = y.Store.UserImage,
                  StorePhone = y.Store.PhoneNo,
                  NetPrice = y.Net_Price,
                   ProductId = y.ProductId,
                   ProductQuantity = y.ProductQuantity,
                   TotalPrice = y.TotalPrice,
                   ProductName = y.Products.ProductName,
                   ProductImage = y.Products.ProductImage,
                   ProductPrice = y.Products.ProductPrice,
                   ProductVariations = y.ProductVariations,
                   ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
                   {
                       CommentDescription = z.CommentDescription,
                       Rating = z.Rating,
                   }).ToList(),

               }).ToList(),

           }).ToList();

            return Json(dataof);
        }
        [Authorize]
        public IActionResult GetOrdersbyStatusbyUserId()
        {
            var userid = _userService.GetUserId();
            //var dataof = _commerceDb.OrderDetails.Where(x => (x.OrderStatus != (int)ShippingStatus.Delivered && x.Orders.Any(u=>u.StoreId == userid))
            //                                                        || (x.OrderStatus != (int)ShippingStatus.Cancelled && x.Orders.Any(u => u.StoreId == userid)) 
            //                                                                || (x.Orders.Any(u => u.IsDeleted != false)  && x.Orders.Any(u => u.StoreId == userid))) 
            var dataof = _commerceDb.OrderDetails.Where(x => (x.OrderStatus == (int)ShippingStatus.Pending && x.Orders.Any(u=>u.StoreId == userid)))
           .Select(x => new CheckInSelectView()
           {
               UserId = x.UserId,
               ODEmail = x.Email,
               ODNationality = x.Nationality,
               ODPhoneNo = x.PhoneNo,
               ODUserName = x.UserName,
               Stauts = x.OrderStatus,
               StatusName =((ShippingStatus) x.OrderStatus).ToString(),
               Address = x.Address,
               UploadDate = x.UploadDate,
               ODOrderdetailId = x.OrderDetailsId,
               OrderHistoryView = _commerceDb.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.IsDeleted != false).Select(y => new OrderHistoryView()
               {
                  StoreId = y.Store.UserId,
                  StoreName = y.Store.UserName,
                  StoreEmail = y.Store.Email,
                  StoreImage = y.Store.UserImage,
                  StorePhone = y.Store.PhoneNo,
                  NetPrice = y.Net_Price,
                   ProductId = y.ProductId,
                   ProductQuantity = y.ProductQuantity,
                   RemainingQuantity = y.RemainingQuantity,
                   TotalPrice = y.TotalPrice,
                   ProductName = y.Products.ProductName,
                   ProductImage = y.Products.ProductImage,
                   ProductPrice = y.Products.ProductPrice,
                   ProductVariations = y.ProductVariations,
                   ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId 
                                                                       && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
                   {
                       CommentDescription = z.CommentDescription,
                       Rating = z.Rating,
                   }).ToList(),

               }).ToList(),

           }).ToList();

            return Json(dataof);
        }

        public IActionResult CurrentMonthSale()
        {
            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);
            var CurrentMonth = _commerceDb.Orders.Where(x => now > x.UploadDate && x.UploadDate > first && x.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered).ToList();
            return Json(CurrentMonth);
        }

        public IActionResult CurrentYearSale()
        {
            var now = DateTime.Now;
            var Year = new DateTime(now.Year);
            var CurrentYear = _commerceDb.Orders.Where(x => now > x.UploadDate && x.UploadDate > Year && x.OrderDetails.OrderStatus == (int)ShippingStatus.Delivered).ToList();
            return Json(CurrentYear);
        }

        public IActionResult AddProductsView()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProducts(SaveData saveData)
        {
            CommerceDbContext dbContext = new CommerceDbContext();
            var userid = (_httpContextAccessor.HttpContext.Items["User"] as User).UserId;

            KidCategory kidcat = new KidCategory();
            ViewModel view = new ViewModel();
            User user = new User();
          
                KidCategory kids = new KidCategory();
                var kidsis = dbContext.KidCategories.Where(x => x.KidCategoryName == saveData.KidCategoryName).FirstOrDefault().KidCategoryId;

                Product prod = new Product();
                prod.UserId = Convert.ToInt32(userid);
                string proimage = Path.GetFileName(saveData.file.FileName);
                string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "\\images", proimage);
                prod.ProductName = saveData.ProductName;
                //prod.ProductPrice = saveData.ProductPrice;
                prod.ProductImage = uploadfilepath;
                prod.KidCategoryId = kidsis;
                prod.UploadDate = DateTime.Now;
                dbContext.Products.Add(prod);
                dbContext.SaveChanges();
                var latestProductId = prod.ProductId;

                   int val = 0;
                  foreach (var item in saveData.Texts)
                  {
                     int val1 = val++;
                     ProductVariations productVariations = new ProductVariations();
                    productVariations.P_Id = latestProductId;
                    productVariations.P_VariationsName = item;
                    //productVariations.P_VAriationLabel = saveData.Labels.ElementAtOrDefault(val1);
                    productVariations.UploadOn = DateTime.Now;
                    dbContext.ProductVariations.Add(productVariations);
                    dbContext.SaveChanges();
                  }


            ProductDescription description = new ProductDescription();

            description.ProductId = latestProductId;
            description.ProductBrand = saveData.ProductBrand;
            description.ProductMaterial = saveData.ProductMaterial;
            description.ProductQuality = saveData.ProductQuality;
            description.ProductDes = saveData.ProductDes;
            description.ProductType = saveData.ProductType;
            dbContext.ProductDescriptions.AddRange(description);
            dbContext.SaveChanges();


            foreach (var images in saveData.savedimages)
            {
                ProductDescriptionimage productDescriptionimage = new ProductDescriptionimage();
                string filepath = Path.GetFileName(images.FileName);
                string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                productDescriptionimage.ProductdesImage1 = imagespaths;
                productDescriptionimage.ProductId = latestProductId;
                dbContext.AddRange(productDescriptionimage);
                dbContext.SaveChanges();
            }

            ProductSpecification productSpecification = new ProductSpecification();
            productSpecification.ProductId = latestProductId;
            productSpecification.CameraQuality = saveData.CameraQuality;
            productSpecification.ProductDisplay = saveData.ProductDisplay;
            productSpecification.ProductWarrantyInfo = saveData.ProductWarrantyInfo;
            productSpecification.ProductShippingInfo = saveData.ProductShippingInfo;
            productSpecification.Graphics = saveData.Graphics;
            productSpecification.Memory = saveData.Memory;
            productSpecification.ProcessorCapacity = saveData.ProcessorCapacity;
            productSpecification.ProductSpecificationsText = saveData.ProductSpecificationsText;
            dbContext.Add(productSpecification);
            dbContext.SaveChanges();

            //    int val = 0;
            //    foreach (var item in saveData.Labels)
            //    {
            //        MoreSepcifications more = new MoreSepcifications();
            //        more.Label = item;
            //        more.ProductId = latestProductId;

            //        int val1 = val++;
            //        more.SpecificationsText = saveData.Texts.ElementAtOrDefault(val1);
            //        dbContext.MoreSepcifications.AddRange(more);
            //        dbContext.SaveChanges();
            //    }
            return Json(latestProductId);
        }

        [HttpPost]
        public IActionResult AddVariat(int ProductId, IList<string> Texts)
        {
           
            foreach (var item in Texts)
            {
                ProductVariations productVariations = new ProductVariations();
                productVariations.P_Id = ProductId;
                productVariations.P_VariationsName = item;
                productVariations.UploadOn = DateTime.Now.Date;
                _commerceDb.ProductVariations.Add(productVariations);
                _commerceDb.SaveChanges();
            }
            var Variations = _commerceDb.ProductVariations.Where(x => x.P_Id == ProductId).ToList();
           return Json(Variations);
        }
       
        public List<ProductVariations> GetProductVariationsbyId(int id)
        {
            List<ProductVariations> variations = _commerceDb.ProductVariations.Where(x => x.P_Id == id).Select(y=> new ProductVariations()
            {
                P_VariationsID = y.P_VariationsID,
                P_VariationsName = y.P_VariationsName,
                //P_VAriationLabel = y.P_VAriationLabel,
            }).ToList();
            return variations;
        }
        //[HttpGet("forget-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [ HttpPost("forget-password")]
        public IActionResult ForgotPasswords(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        public IActionResult SellerDashBoard()
        {
            return View();
        }

        public IActionResult EarningOverviewChart()
        {
            Orders orders = new Orders();
            var consdat = DateTime.Now;
            
            DateTime dt;
            var str = "112023";
            var Jan = DateTime.TryParse(str, out dt);
            MonthlySaleChartData monthlysale = new MonthlySaleChartData();

            monthlysale.January = _commerceDb.App_Table.Where(x => x.Uploadon >= Convert.ToDateTime("1/1/2023")
            && consdat >= x.Uploadon).Sum(x => x.ComissionValue);

            monthlysale.Febrary= _commerceDb.App_Table.Where(x => x.Uploadon>= Convert.ToDateTime("06/01/2023")
          && (Convert.ToDateTime("10/01/2023") >= x.Uploadon)).Sum(x => x.ComissionValue);

            monthlysale.March= _commerceDb.App_Table.Where(x => x.Uploadon >= Convert.ToDateTime("11/01/2023")
         && (Convert.ToDateTime("15/01/2023") >= x.Uploadon)).Sum(x => x.ComissionValue);

            monthlysale.April = _commerceDb.App_Table.Where(x => x.Uploadon>= Convert.ToDateTime("20/01/2023")
         && (Convert.ToDateTime("25/01/2023") >= x.Uploadon)).Sum(x => x.ComissionValue);

            monthlysale.May = _commerceDb.App_Table.Where(x => x.Uploadon>= Convert.ToDateTime("26/01/2023")
         && (Convert.ToDateTime("30/01/2023") >= x.Uploadon)).Sum(x => x.ComissionValue);

            monthlysale.June = _commerceDb.App_Table.Where(x => x.Uploadon>= Convert.ToDateTime("01/02/2023")
         && (Convert.ToDateTime("05/02/2023") >= x.Uploadon)).Sum(x=>x.ComissionValue);
            monthlysale.July = 0;
            monthlysale.August = 0;
            monthlysale.September = 0;
            monthlysale.October = 0;
            monthlysale.November = 0;
            monthlysale.December = 0;

            
            return Json(monthlysale);
        }
        [Authorize]
        public IActionResult EarningOverviewChartCurrentStore()
        {
            var userid = _userService.GetUserId();
            Orders orders = new Orders();
            var consdat = DateTime.Now;

            DateTime dt;
            var str = "112023";
            var Jan = DateTime.TryParse(str, out dt);
            MonthlySaleChartData monthlysale = new MonthlySaleChartData();

            monthlysale.January = _commerceDb.Orders.Where(x => x.UploadDate >= Convert.ToDateTime("1/1/2023")
            && consdat >= x.UploadDate && x.StoreId == userid).Sum(x => x.Net_Price);

            monthlysale.Febrary = _commerceDb.Orders.Where(x => x.UploadDate>= Convert.ToDateTime("06/01/2023")
            && (Convert.ToDateTime("10/01/2023") >= x.UploadDate) && x.StoreId == userid).Sum(x => x.Net_Price);

            monthlysale.March = _commerceDb.Orders.Where(x => x.UploadDate>= Convert.ToDateTime("11/01/2023")
            && (Convert.ToDateTime("15/01/2023") >= x.UploadDate) && x.StoreId == userid).Sum(x => x.Net_Price);

            monthlysale.April = _commerceDb.Orders.Where(x => x.UploadDate>= Convert.ToDateTime("20/01/2023")
            && (Convert.ToDateTime("25/01/2023") >= x.UploadDate) && x.StoreId == userid).Sum(x => x.Net_Price);

            monthlysale.May = _commerceDb.Orders.Where(x => x.UploadDate>= Convert.ToDateTime("26/01/2023")
            && (Convert.ToDateTime("30/01/2023") >= x.UploadDate) && x.StoreId == userid).Sum(x => x.Net_Price);

            monthlysale.June = _commerceDb.Orders.Where(x => x.UploadDate>= Convert.ToDateTime("01/02/2023")
            && (Convert.ToDateTime("05/02/2023") >= x.UploadDate)).Sum(x => x.Net_Price);

            monthlysale.July = 0;
            monthlysale.August = 0;
            monthlysale.September = 0;
            monthlysale.October = 0;
            monthlysale.November = 0;
            monthlysale.December = 0;


            return Json(monthlysale);
        }

        //          if (saveData.ProductId >= 1)
        //            {
        //                if (saveData.psizes.Count > saveData.pcolours.Count )
        //                {



        //                int priceval = 0;
        //        int quanval = 0;

        //                foreach (var item in saveData.psizes)
        //                {

        //                    ProductBatch productBatch = new ProductBatch();
        //        var size = _commerceDb.Colour.Where(x => x.ColourName == item).Select(x => x.ColourId).FirstOrDefault();
        //        productBatch.SizeId = size;

        //                    int colval = 0;
        //        int colors = colval++;
        //        var colid = saveData.pcolours.ElementAtOrDefault(colors);
        //        var colour = _commerceDb.Colour.Where(x => x.ColourName == colid).Select(x => x.ColourId).FirstOrDefault();
        //        productBatch.ColourId = colour;

        //                    int pric = priceval++;
        //        productBatch.Price = saveData.pprice.ElementAtOrDefault(pric);

        //                    int quan = quanval++;
        //        productBatch.Quantity = saveData.pquantity.ElementAtOrDefault(quan);

        //                    productBatch.P_Id = saveData.ProductId;

        //                    dbContext.ProductBatch.Add(productBatch);
        //                    dbContext.SaveChanges();

        //                }
        //}
        //                else
        //{
        //    int priceval = 0;
        //    int quanval = 0;

        //    foreach (var item in saveData.pcolours)
        //    {

        //        ProductBatch productBatch = new ProductBatch();
        //        var colo = _commerceDb.Colour.Where(x => x.ColourName == item).Select(x => x.ColourId).FirstOrDefault();
        //        productBatch.ColourId = colo;
        //        int sizval = 0;
        //        int siz = sizval++;
        //        var sizid = saveData.psizes.ElementAtOrDefault(siz);
        //        var sizeses = _commerceDb.Sizes.Where(x => x.SizeName == sizid).Select(x => x.Sizeid).FirstOrDefault();
        //        productBatch.SizeId = sizeses;

        //        int pric = priceval++;
        //        productBatch.Price = saveData.pprice.ElementAtOrDefault(pric);

        //        int quan = quanval++;
        //        productBatch.Quantity = saveData.pquantity.ElementAtOrDefault(quan);

        //        productBatch.P_Id = saveData.ProductId;

        //        dbContext.ProductBatch.Add(productBatch);
        //        dbContext.SaveChanges();

        //    }

        //}
        //return Json(saveData.ProductId);
        //            }
        //            else
        //{


        //if (saveData.pcolours.Count > saveData.psizes.Count)
        //{

        //    int priceval = 0;
        //    int quanval = 0;

        //    foreach (var item in saveData.pcolours)
        //    {

        //        ProductBatch productBatch = new ProductBatch();
        //        var colo = _commerceDb.Colour.Where(x => x.ColourName == item).Select(x => x.ColourId).FirstOrDefault();
        //        productBatch.ColourId = colo;
        //        int sizval = 0;
        //        int siz = sizval++;
        //        var sizid = saveData.psizes.ElementAtOrDefault(siz);
        //        var sizeses = _commerceDb.Sizes.Where(x => x.SizeName == sizid).Select(x => x.Sizeid).FirstOrDefault();
        //        productBatch.SizeId = sizeses;

        //        int pric = priceval++;
        //        productBatch.Price = saveData.pprice.ElementAtOrDefault(pric);

        //        int quan = quanval++;
        //        productBatch.Quantity = saveData.pquantity.ElementAtOrDefault(quan);

        //        productBatch.P_Id = latestProductId;

        //        dbContext.ProductBatch.Add(productBatch);
        //        dbContext.SaveChanges();

        //    }
        //}
        //else
        //{
        //    int colval = 0;
        //    int priceval1 = 0;
        //    int quanval1 = 0;

        //    foreach (var item in saveData.psizes)
        //    {

        //        ProductBatch productBatch = new ProductBatch();
        //        var size = _commerceDb.Sizes.Where(x => x.SizeName == item).Select(x => x.Sizeid).FirstOrDefault();
        //        productBatch.SizeId = size;

        //        int colors = 0;
        //        var colid = saveData.pcolours.ElementAtOrDefault(colors);
        //        var colour = _commerceDb.Colour.Where(x => x.ColourName == colid).Select(x => x.ColourId).FirstOrDefault();
        //        productBatch.ColourId = colour;

        //        int pric = priceval1++;
        //        productBatch.Price = saveData.pprice.ElementAtOrDefault(pric);

        //        int quan = quanval1++;
        //        productBatch.Quantity = saveData.pquantity.ElementAtOrDefault(quan);

        //        productBatch.P_Id = latestProductId;

        //        dbContext.ProductBatch.Add(productBatch);
        //        dbContext.SaveChanges();

        //    }
        //}
        //foreach (var colours in saveData.pcolours)
        //{
        //    ProductColours colours1 = new ProductColours();
        //    var colour = _commerceDb.Colour.Where(x => x.ColourName == colours).Select(x => x.ColourId).FirstOrDefault();
        //    colours1.ColourId = colour;
        //    colours1.ProductId = latestProductId;
        //    dbContext.ProductColours.Add(colours1);
        //    dbContext.SaveChanges();
        //}

        //foreach (var siz in saveData.psizes)
        //{
        //    ProductSize sizes1 = new ProductSize();
        //    var sizeid = _commerceDb.Sizes.Where(x => x.SizeName == siz).Select(x => x.Sizeid).FirstOrDefault();
        //    sizes1.Sizeid = sizeid;
        //    sizes1.ProductId = latestProductId;
        //    dbContext.ProductSize.Add(sizes1);
        //    dbContext.SaveChanges();
        //}

        //Console.WriteLine("", item, i + 1);
        //saveData.Texts.Add(tex);
        //saveData.Texts.Items.Add(item);

        //List<ProductSize> productSize = new List<ProductSize> {
        //new ProductSize() { ProductSizes = saveData.ProductSizes, ProductId = latestProductId },
        //new ProductSize() { ProductSizes = saveData.ProductSizes, ProductId = latestProductId },
        //new ProductSize() { ProductSizes = saveData.ProductSizes, ProductId = latestProductId },
        //};
        //dbContext.AddRange(productSize);
        //dbContext.SaveChanges();

        //string prodescriptionimage1 = Path.GetFileName(saveData.DesImg1.FileName);
        ////string prodescriptionimagepath1 = Path.Combine(Directory.GetCurrentDirectory(), "\\images", prodescriptionimage1);

        //string prodescriptionimage2 = Path.GetFileName(saveData.DesImg2.FileName    );
        //string prodescriptionimagepath2 = Path.Combine(Directory.GetCurrentDirectory(), "\\images", prodescriptionimage2);

        //string prodescriptionimage3 = Path.GetFileName(saveData.DesImg3.FileName);
        //string prodescriptionimagepath3 = Path.Combine(Directory.GetCurrentDirectory(), "\\images", prodescriptionimage3);

        //string prodescriptionimage4 = Path.GetFileName(saveData.DesImg4.FileName);
        //string prodescriptionimagepath4 = Path.Combine(Directory.GetCurrentDirectory(), "\\images", prodescriptionimage4);

        //string prodescriptionimage5 = Path.GetFileName(saveData.DesImg5.FileName);
        //string prodescriptionimagepath5 = Path.Combine(Directory.GetCurrentDirectory(), "\\images", prodescriptionimage5);

        //List<ProductDescriptionimage> pidi = new List<ProductDescriptionimage> {
        ////new ProductDescriptionimage() { ProductdesImage1 = prodescriptionimagepath1, ProductId = latestProductId },
        //new ProductDescriptionimage() { ProductdesImage1 = prodescriptionimagepath2, ProductId = latestProductId },
        //new ProductDescriptionimage() { ProductdesImage1 = prodescriptionimagepath3, ProductId = latestProductId },
        //new ProductDescriptionimage() { ProductdesImage1 = prodescriptionimagepath4, ProductId = latestProductId },
        //new ProductDescriptionimage() { ProductdesImage1 = prodescriptionimagepath5, ProductId = latestProductId },
        //};

        //dbContext.AddRange(pidi);
        //dbContext.SaveChanges();

        //[Authorize]
        [HttpGet]
        public IActionResult AddProducts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetCategoies()
        {
            ViewModel view = new ViewModel();
            var cate = _commerceDb.Categories.ToList();
            view.categoriesvm = cate;
            return View(view);
        }
        public IActionResult GetPErCate(int Id)
        {
            var data = _commerceDb.PerCategories.Where(x => x.CategoryId == Id).ToList();
            return Json(data);
        }

        public IActionResult GetKiDCateAdminView()
        {
            return View();
        }

        public IActionResult GetKiDCate()
        {
            return View();
        }
       
        public IActionResult GetCatebyId(int Id)
        {
            var data = _commerceDb.PerCategories.Where(z => z.CategoryId == Id).Select(z=>z.PerCategoryId).ToList();
            var data12 =_commerceDb.KidCategories.Where(u => data.Contains(u.PerCategoryId)).Select(u => u.KidCategoryId).ToList();
            var view = _commerceDb.Products.Where(a => data12.Contains(a.KidCategoryId)).OrderByDescending(a=>a.UploadDate).Select(y => new ProductSelectView()
            {
                ProductName = y.ProductName,
                ProductImage = y.ProductImage,
                UploadDate = y.UploadDate,
                ProductId = y.ProductId,
                wishList=y.WishList.ToList(),
                ProductViewModels = new ProductViewModel()
                {
                    productVariations = y.ProductVariations.Where(d => d.P_Id == y.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                }
            }).ToList();

            return Json(view);
        }

        public IActionResult GetKidCateeg(int Id)
        {
            var data = _commerceDb.KidCategories.Where(x => x.PerCategoryId == Id).ToList();
            return Json(data);
        }

        public IActionResult GetPreoKidCAte(int Id)
        {
            var data12 = _commerceDb.KidCategories.Where(u => u.PerCategoryId == Id).Select(u => u.KidCategoryId).ToList();
            var view = _commerceDb.Products.Where(a => data12.Contains(a.KidCategoryId)).OrderByDescending(a => a.UploadDate).Select(y => new ProductSelectView()
            {
                ProductName = y.ProductName,
                ProductImage = y.ProductImage,
                UploadDate = y.UploadDate,
                ProductId = y.ProductId,
                wishList = y.WishList.ToList(),
                ProductViewModels = new ProductViewModel()
                {
                    productVariations = y.ProductVariations.Where(d => d.P_Id == y.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                }
            }).ToList();

            return Json(view);
        }
        [HttpGet]
        public IActionResult GetCategoiesbyIDAdminView(int Id)
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetCategoiesbyID(int Id)
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetPerCategoiesbyID(int Id)
        {
            ViewModel view = new ViewModel();
            var perdata = _commerceDb.KidCategories.Where(y => y.PerCategoryId == Id).ToList();
            view.kidCategoriesvm = perdata;
            return View(view);
        }
        [HttpGet]
        public IActionResult GetProductbyKidIdView()
        {
            return View();
        }
        public IActionResult GetProductsView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProductsKidCat(int id)
        {
           var products = _commerceDb.Products.Where(x => x.KidCategoryId == id).Select(y => new ProductSelectView()
            {
                ProductName = y.ProductName,
                ProductImage = y.ProductImage,
                UploadDate = y.UploadDate,
                ProductId = y.ProductId,
                wishList = y.WishList.ToList(),
                ProductViewModels = new ProductViewModel()
                {
                    productVariations = y.ProductVariations.Where(d => d.P_Id == y.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                }
            }).ToList();
            return Json(products);
        }
    [HttpPost]
    public JsonResult GetProductsbyKidId(int Id)
    {
        SearchViewModel view = new SearchViewModel();

        var prodsize = _commerceDb.Sizes.ToList();
        var prodcolour = _commerceDb.Colour.ToList();
        var proddesc = _commerceDb.ProductDescriptions.Select(x => x.ProductBrand).Distinct().ToList();
        //var disticbrands = proddesc.Distinct();
        var products = _commerceDb.Products.ToList();
        view.Products = products;
        view.Sizesvm = prodsize;
        view.Coloursvm = prodcolour;
        view.ProductDescriptionVm = proddesc;
        return Json(view);
    }
        public IActionResult GetProductbyKidId()
        {
           return View();
           
        }
        public List<Product> ProductbyKidId (int Id)
        {
            List<Product> products = _commerceDb.Products.Where(y => y.KidCategoryId == Id).ToList();
            return products;
        }
        public IActionResult GetProductKidIdAdminView()
        {
            return View();
        }
        public IActionResult GetProductKidIdBuerView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetImageDesbyImagesId(string Id)
        {
            ViewModel view = new ViewModel();
            ProductDescriptionimage descriptionimage = new ProductDescriptionimage();
            var perdata = _commerceDb.ProductDescriptionimages.Where(y => y.ProductdesImage1 == Id).ToList();
            view.productDescriptionimages = perdata;
            return View(view);
        }
        public List<User> user = new List<User>();

        public IActionResult SearchAccount(string username, string Password1, string Password2)
        {
            SaveData saveData = new SaveData();
            User users = new User();
            if (string.IsNullOrWhiteSpace(username) || username != users.UserName)
            {
                return View("Please Enter Valid User Name");
            }
            using (var db = new CommerceDbContext())
            {
                user = db.Users.Where(x => x.UserName == username).ToList();
                users.UserId = saveData.UserId;
                users.Password = saveData.Password;
                db.Users.Update(users);
                db.SaveChanges();
                return View();
            }
        }
        [HttpPost]
        public IActionResult UPdateAccount(SaveData saved)
        {
            SaveData saveData = new SaveData();
            User users = new User();
            if (string.IsNullOrWhiteSpace(saved.UserName))
            {
                return View("Please Enter Valid User Name");
            }
            using (var db = new CommerceDbContext())
            {
                user = db.Users.Where(x => x.UserName == saved.UserName).ToList();
                users.UserId = saved.UserId;
                return View(saved);
            }
        }

        [HttpPost]
        public IActionResult UpdatePassword(SaveData saved)
        {
            SaveData save = new SaveData();
            User user = new User();
            if (string.IsNullOrEmpty(saved.Password) || string.IsNullOrWhiteSpace(saved.Password1))
            {
                return View("Null Values Cannot be Saved");
            }
            else
            {
                if (saved.Password != saved.Password1)
                {
                    return View("Password and Confirm Password Should have Same Values");
                }
                using (var db = new CommerceDbContext())
                {
                    db.Users.Where(x => x.UserName == saved.UserName).ToList();

                    user.UserId = saved.UserId;
                    user.Password = saved.Password;
                    db.Users.Update(user);
                    db.SaveChanges();
                    return View();
                }
            }
        }
        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdateAccount()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangeMainCategory(Category obj)
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteMainCategory(Category obj)
        {
            Category mainCategory = new Category();
            using (var db = new CommerceDbContext())
            {
                Category main = new Category();
                main.CategoryName = obj.CategoryName;
                main.CategoryIcon = obj.CategoryIcon;
                db.Categories.Remove(obj);
                db.SaveChanges();
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult DeleteMainCategory()
        {
            return View();
        }

        public IActionResult GetAllRolesName()
        {
            var roles = _commerceDb.Users.Select(x=>x.status).ToList();
            return Json (roles);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserData()
        {
            
            var userid = _userService.GetUserId();
            var data = _commerceDb.Users.Where(x => x.UserId == userid)
                .Select(x => new UsersAdminModel()
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    UserImage = x.UserImage,
                    PhoneNo = x.PhoneNo,
                    Email = x.Email,
                    Address = x.Address,
                    FatherName = x.FatherName,
                    City = x.City,
                    Age = x.Age,
                    Gender = x.Gender,
                    Cnic = x.Cnic,
                    Nationality = x.Nationality,
                    PostalCode = x.PostalCode,
                    Password = x.Password,
                    RoleName = x.UserRoles.Where(y => y.UserId == x.UserId).Select(y => y.Role.RoleName).FirstOrDefault(),
                    RoleId = x.UserRoles.Where(y => y.UserId == x.UserId).Select(y => y.Role.RoleId).FirstOrDefault(),
                }).FirstOrDefault();
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetUserDatabyId(int Id)
        {
            //var userid = _userService.GetUserId();
            var data = _commerceDb.Users.Where(x => x.UserId == Id)
                .Select(x => new UsersAdminModel()
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    UserImage = x.UserImage,
                    PhoneNo = x.PhoneNo,
                    Email = x.Email,
                    Address = x.Address,
                    FatherName = x.FatherName,
                    City = x.City,
                    Age = x.Age,
                    Gender = x.Gender,
                    Cnic = x.Cnic,
                    Nationality = x.Nationality,
                    PostalCode = x.PostalCode,
                    Password = x.Password,
                    Status = x.status,
                    //Date = x.Date,
                    RoleName = x.UserRoles.Where(y => y.UserId == x.UserId).Select(y => y.Role.RoleName).FirstOrDefault(),

                }).FirstOrDefault();
            return Json(data);
        }
        public IActionResult UpdateUserDatabyId(SaveData saveData)
        {
            User user12 = _commerceDb.Users.Where(x => x.UserId == saveData.UserId).FirstOrDefault();
            List<User> users = _commerceDb.Users.Where(x => x.UserId == saveData.UserId).ToList();
                foreach (var item in users)
                {
                    item.UserName = saveData.UserName;
                    item.FatherName = saveData.FatherName;
                    item.Cnic = saveData.Cnic;
                    item.Email = saveData.Email;
                    item.PhoneNo = saveData.PhoneNo;
                    item.Nationality = saveData.Nationality;
                    item.City = saveData.City;
                    item.Address = saveData.Address;
                    item.PostalCode = saveData.PostalCode;
                    item.Age = saveData.Age;
                    item.Gender = saveData.Gender;
                if (saveData.file == null)
                {
                    item.UserImage = user12.UserImage;

                }
                else
                {
                    string imagepath = Path.GetFileName(saveData.file.FileName);
                    string imagedes = Path.Combine(Directory.GetCurrentDirectory(), "\\images", imagepath);
                    item.UserImage = imagedes;
                }
                    item.Password = saveData.Password;
                    item.status = saveData.userstatus;
                    _commerceDb.Users.Update(item);
                    _commerceDb.SaveChanges();
                }
                return Json(user12);
        }

       public IActionResult UpdateUserAccountAdminView()
        {
            return View();
        }
        public IActionResult UpdateUserAccount()
        {
            return View();
        }
        public IActionResult UpdateSellerAccount()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult UpdateUserData(SaveData saveData)
        {
            var userid = _userService.GetUserId();
            User user1 = _commerceDb.Users.Where(x => x.UserId == userid).FirstOrDefault();
            List<User> users = _commerceDb.Users.Where(x => x.UserId == userid).ToList();

            foreach (var item in users)
            {
                item.UserName = saveData.UserName;
                item.FatherName = saveData.FatherName;
                item.Cnic = saveData.Cnic;
                item.Email = saveData.Email;
                item.PhoneNo = saveData.PhoneNo;
                item.Nationality = saveData.Nationality;
                item.City = saveData.City;
                item.Address = saveData.Address;
                item.PostalCode = saveData.PostalCode;
                item.Age = saveData.Age;
                item.Gender = saveData.Gender;
               
                
                item.Password = saveData.Password;
                if (saveData.file == null)
                {
                    item.UserImage = user1.UserImage;
                }
                else
                {
                    string imagepath = Path.GetFileName(saveData.file.FileName);
                    string imagedes = Path.Combine(Directory.GetCurrentDirectory(), "\\images", imagepath);
                    item.UserImage = imagedes;
                }

                _commerceDb.Users.Update(item);
                _commerceDb.SaveChanges();

            }
            return Json(userid);
        }
        [HttpGet]
        public IActionResult GetAllKidCategories()
        {
           List<KidCategory> kidCategories = _commerceDb.KidCategories.ToList();
            return Json(kidCategories);
        }
        [HttpGet]
        public IActionResult TextBoxSearchProducts(string ProductName)
        {

            List<Product> allsearch = _commerceDb.Products.Where(x => x.ProductName.Contains(ProductName)).Select(x => new Product
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,

            }).ToList();
            return new JsonResult(allsearch);
        }
        [Authorize]
        [HttpGet]
        public IActionResult TextBoxSearchProductsbyStoreID(string ProductName)
        {
            var userid = _userService.GetUserId();
            List<Product> allsearch = _commerceDb.Products.Where(x => x.ProductName.Contains(ProductName) && x.UserId == userid).Select(x => new Product
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,

            }).ToList();
            return new JsonResult(allsearch);
        }
        public IActionResult FilterTextBoxSearchView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult FilterTextBoxSearch(string ProductName, int ProductPrice, string Colour, string Size, string Brand)
        {
            //var finalresult = (from p in products.Where(z=>z.ProductName == ProductName && z.ProductPrice == ProductPrice) 
            //                   select new ProductSelectView
            //                   {
            //                       ProductId = p.ProductId,

            //                   })
            //List<ProductDescription> productDescriptions = _commerceDb.ProductDescriptions.Where(d => d.Equals(ProductName)).ToList();
            //var prods = _commerceDb.Products.Where(p => p.ProductName == ProductName && p.ProductPrice == ProductPrice && p.ProductDescriptions.Where(y => y.ProductBrand == Brand) && p.ProductSize.Where(s => s.Sizes.SizeName == Size) && (p.ProductColours.Where(c => c.Colour.ColourName == Colour)).ToList();
            var prodid = _commerceDb.Products.Where(p => p.ProductName == ProductName && p.ProductPrice == ProductPrice).Select(p => new ProductSelectView()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                UploadDate = p.UploadDate,
                ProductViewModels = new ProductViewModel()
                {
                    ProductDescriptions = p.ProductDescriptions.Where(q => q.ProductBrand == Brand && q.ProductId == p.ProductId).Select(q => new ProductDescription()
                    {
                        ProductBrand = q.ProductBrand,
                        ProductMaterial = q.ProductMaterial,
                        ProductDes = q.ProductDes,

                    }).ToList(),
                    Size = p.ProductSize.Where(n => n.ProductId == p.ProductId).Select(s => s.Sizes).Where(s => s.SizeName == Size).Single(),

                    Colour = p.ProductColours.Where(m => m.ProductId == p.ProductId).Select(c => c.Colour).Where(c => c.ColourName == Colour).Single(),
                    ProductSpecifications = p.ProductSpecifications.Where(z => z.ProductId == p.ProductId && z.ProductId == p.ProductId).ToList(),
                }

            }).ToList();
            if (prodid.Count == null)
            {
                return BadRequest();
            }
            else
            {
                return Json(prodid);
            }
        }

        [HttpGet]
        public IActionResult GetUserId()
        {
            var userids = _userService.GetUserId();
            return Json(userids);
        }

        [Authorize]
        public IActionResult GetAllAdmins()
        {
            var usreid = _userService.GetUserId();
            List<User> Users = _commerceDb.Users.Where(x => x.UserRoles.Any(y => (y.RoleId == (int)RolesEnum.Admin)) 
                                            || (x.UserRoles.Any(y => y.RoleId == (int)RolesEnum.SuperAdmin))).ToList();
            return Json(Users);
        }

        public IActionResult GetRoomAssignStatus(int id)
        {
            int? status = _commerceDb.Room.Where(x => x.R_Id == id).Select(x => x.AssignedTo).FirstOrDefault();
            User user = _commerceDb.Users.Where(x => x.UserId == status).FirstOrDefault();
            return Json(user);
        }

        public IActionResult AssignedTasksbyuserId()
        {
            return View();
        }

        public IActionResult SellerAccountView()
        {
            return View();
        }

        public IActionResult AdminView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAdminsData()
        {
            List<User> Users = _commerceDb.Users.Select(x => new User()
            {
                UserId = x.UserId,
                UserName = x.UserName,
                UserImage = x.UserImage,
                Email = x.Email,
                Cnic = x.Cnic,
                Age = x.Age,
                PhoneNo = x.PhoneNo,
                Gender = x.Gender,
                City = x.City,
                Nationality = x.Nationality,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Password = x.Password,
                Date = x.Date,
                status = x.status,
                UserRoles = x.UserRoles.Select(y => new UserRole()
                {
                    Role = y.Role,
                }).ToList(),

            }).ToList();
            return Json(Users);
        }

        public IActionResult AdminIndexPageView()
        {
            return View();
        }
        public IActionResult AllUsersData()
        {
            return View();
        }
        public IActionResult GetAllCategories()
        {
            List<Category> categories = _commerceDb.Categories.ToList();
            return Json(categories);
        }
        [HttpGet]
        public IActionResult GetPerCategories(int CategoryId)
        {
            List<PerCategory> perCategories = _commerceDb.PerCategories.Where(x => x.CategoryId == CategoryId).ToList();
            return Json(perCategories);
        }
        public IActionResult GetKidCategories(int PerCategoryId)
        {
            List<KidCategory> kidCategories = _commerceDb.KidCategories.Where(x => x.PerCategoryId == PerCategoryId).ToList();
            return Json(kidCategories);
        }
        [HttpPost]
        public IActionResult SelectedSearch(FilterSearchModel filter)
        {

            if (filter.ProductName == null && filter.MinimumPrice == 0 &&filter.MaximumPrice == 0 && filter.ColourIds == null && filter.SizeIds == null && filter.Brands == null)
            {
                var result = _commerceDb.Products.ToList();
                return Json(result);
            }
            else
            {
                IQueryable<Product> Test1 = _commerceDb.Products.AsQueryable<Product>();

                //if (filter.KidId != null && filter.KidId != 0)
                //{
                //    Test1 = Test1.Where(p => p.KidCategoryId == filter.KidId);
                //};
                if (filter.ProductName != null)
                {
                    Test1 = Test1.Where(p => p.ProductName == filter.ProductName);
                };
                if (filter.MinimumPrice != 0 || filter.MaximumPrice != 0)
                {
                    Test1 = Test1.Where(c=> filter.MaximumPrice >= c.ProductPrice && c.ProductPrice >= filter.MinimumPrice);
                };
                if (filter.Brands != null)
                {
                    Test1 = Test1.Where(p => (p.ProductDescriptions.Any(b => filter.Brands.Contains(b.ProductBrand))));
                };
                if (filter.ColourIds != null)
                {
                    Test1 = Test1.Where(p => p.ProductColours.Any(c => filter.ColourIds.Contains(c.ColourId)));
                };
                if (filter.SizeIds != null)
                {
                    Test1 = Test1.Where(p => p.ProductSize.Any(s => filter.SizeIds.Contains(s.Sizeid)));
                }
             
                var result = Test1.Select(p => new Product()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,
                    ProductPrice = p.ProductPrice,
                }).ToList();

                return Json(result);
            }

        }
        public IActionResult SellerAccountUpdate()
        {
            return View();
        }
        public IActionResult AddPoductsWithSpecifications()
        {
            return View();
        }
        [HttpGet]
        public List<Colour>  GetAllColoures()
        {
            List<Colour> colours = _commerceDb.Colour.ToList();
            return colours;
        }
        [HttpGet]
        public List<Sizes> GetAllSizes()
        {
            List<Sizes> sizes = _commerceDb.Sizes.ToList();
            return sizes;
        }

        [HttpPost]
        public IActionResult GetProductbyIdForBatch(int id)
        {
            Product product = _commerceDb.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return Json(product);
        }
        public IActionResult GetHelpType()
        {
           List<HelpType> types = _commerceDb.HelpType.ToList();
            return Json(types);
        }

        public IActionResult HelppageView()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddHelp(AddHelpModal helpModal)
        {
            int type = _commerceDb.HelpType.Where(y => y.TypeId == helpModal.type).Select(y=>y.TypeId).SingleOrDefault();
            var userid = _userService.GetUserId();
            int? roomId = _commerceDb.Room.Where(z => z.UserId == userid).Select(z => z.R_Id).FirstOrDefault();
            var ROOMS = _commerceDb.Room.Where(x => x.UserId == userid).ToList();
            if (ROOMS.Count >= 1)
            {
                AdminChat adminChat = new AdminChat();
                adminChat.MessageText = helpModal.HelpText;
                adminChat.MessageStatus = (int)MessageStatus.UnRead;
                adminChat.MessageOn = DateTime.Now;
                adminChat.RoomId = roomId;

                _commerceDb.AdminChat.Add(adminChat);
                _commerceDb.SaveChanges();
                var newadminchatid = adminChat.A_ChatId;
                return Json(newadminchatid);
            }
            Room help = new Room();
            User userses = _commerceDb.Users.Where(x => x.UserId == userid).FirstOrDefault();
            help.RoomText = helpModal.HelpText;
            help.RoomOn = DateTime.Now;
            help.UserId = userid;
            help.UserName = userses.UserName;
            help.PhoneNo = userses.PhoneNo;
            help.Email = userses.Email;
            help.H_Id = type;
            help.RoomStatus = (int)RoomStatus.UnAssigned;
            _commerceDb.Room.Add(help);
            _commerceDb.SaveChanges();
            var id = help.H_Id;
            return Json(id);
        }
        [HttpPost]
        public IActionResult GetG_SChatbySecretKey(int secretkey, int storeid)
        {
            int? guestid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            var chats = _commerceDb.Chat.Where(x=>(x.SenderId == storeid && x.ReceiverId == guestid) || (x.SenderId== guestid && x.ReceiverId == storeid))
             .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                Id = storeid,
                Userimage = z.SenderChat.UserImage,
                UserName = z.SenderChat.UserName,

            }).ToList();
            return Json(chats);
        }
        
        [HttpPost]
        public IActionResult GetG_SChatbyonlySecretKey(int secretkey)
        {
            
            int? guestid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            var receiverid = _commerceDb.Chat.Where(x => x.SenderId == guestid).Select(x => x.ReceiverId).FirstOrDefault();
            var chats = _commerceDb.Chat.Where(x=>(x.ReceiverId == guestid) || (x.SenderId == guestid))
             .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                Id = receiverid,
                Userimage = z.SenderChat.UserImage,
                UserName = z.SenderChat.UserName,

            }).ToList();
            return Json(chats);
        } 
        
        [Authorize]
        public IActionResult GetChatCurrentUser()
        {
            var userid = _userService.GetUserId();
            var chats = _commerceDb.Chat.Where(x=>(x.ReceiverId == userid) || (x.SenderId == userid))
             .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                
                Id = userid,
                Userimage = z.SenderChat.UserImage,
                UserName = z.SenderChat.UserName,

            }).ToList();
            return Json(chats);
        }



        public IActionResult ChatwithStorebySecretKey(int secretkey, string Message, int storeid)
        {
            var userid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            Chat chat = new Chat();
            chat.SenderId = userid;
            chat.ReceiverId = storeid;
            chat.ChatOn = DateAndTime.Now;
            chat.MessageText = Message;
            chat.M_Status = (int)MessageStatus.UnRead;
            _commerceDb.Chat.Add(chat);
            _commerceDb.SaveChanges();
            var chatid = chat.Id;
            return Json(chatid);
        }


        [HttpPost]
        public IActionResult AddHelpsNotLogedIn(AddHelpModal helpModal)
        {
            var r_id = _commerceDb.Room.Where(x => x.SecretKey == helpModal.secretkey).ToList();
            int? Room_Id = _commerceDb.Room.Where(x =>x.SecretKey == helpModal.secretkey).Select(x=>x.R_Id).FirstOrDefault();
            User user1 = _commerceDb.Users.Where(y => y.SecretKey == helpModal.secretkey).FirstOrDefault();
            if ((Room_Id == null && user1 != null) || (Room_Id == 0 && user1 != null))
            {
                int type = _commerceDb.HelpType.Where(y => y.TypeId == helpModal.type).Select(y => y.TypeId).SingleOrDefault();
                Room rom = new Room();
                rom.UserName = user1.UserName;
                rom.Email = user1.Email;
                rom.PhoneNo = user1.PhoneNo;
                rom.SecretKey = user1.SecretKey;
                rom.H_Id = type;
                rom.RoomOn = DateAndTime.Now;
                rom.RoomText = helpModal.HelpText;
                rom.RoomStatus = (int)MessageStatus.UnRead;
                rom.UserId = user1.UserId;
                _commerceDb.Room.Add(rom);
                _commerceDb.SaveChanges();
                var newroomid = rom.UserId;
                return Json(newroomid);

            }

            if (r_id.Count >= 1)
            {
                AdminChat adminChat = new AdminChat();
                adminChat.MessageText = helpModal.HelpText;
                adminChat.MessageStatus = (int)MessageStatus.UnRead;
                adminChat.MessageOn = DateTime.Now;
                adminChat.RoomId = Room_Id;
               
                _commerceDb.AdminChat.Add(adminChat);
                _commerceDb.SaveChanges();
                var newadminchatid = adminChat.A_ChatId;
                return Json(newadminchatid);
            }
            else 
            {
                char[] charArr = "0123456789".ToCharArray();
                string OTP = string.Empty;
                Random objran = new Random();
                for (int i = 0; i < 9; i++)
                {
                    int pos = objran.Next(1, charArr.Length);
                    if (!OTP.Contains(charArr.GetValue(pos).ToString())) OTP += charArr.GetValue(pos);
                    else i--;
                }
            int? secretkey = Convert.ToInt32(OTP);
                User user = new User();
                user.Email = helpModal.Email;
                user.UserName = helpModal.UserName;
                user.PhoneNo = helpModal.PhoneNumber;
                user.SecretKey = helpModal.secretkey;
                user.Address = helpModal.Address;
                user.Password = "UnAssigned";
                _commerceDb.Users.Add(user);
                _commerceDb.SaveChanges();
                var newuserid = user.UserId;

            int type = _commerceDb.HelpType.Where(y => y.TypeId == helpModal.type).Select(y => y.TypeId).SingleOrDefault();
            Room help = new Room();
            help.RoomText = helpModal.HelpText;
            help.RoomOn = DateTime.Now;
            help.H_Id = type;
            help.UserId = newuserid;
            help.UserName = helpModal.UserName;
            help.SecretKey = secretkey;
            help.Email = helpModal.Email;
            help.PhoneNo = helpModal.PhoneNumber;
            help.RoomStatus = (int)RoomStatus.UnAssigned;
            _commerceDb.Room.Add(help);
            _commerceDb.SaveChanges();
            return Json(OTP);
            }
        }
        [HttpGet]
        public IActionResult GetAllHelps()
        {
            List<Room> help = _commerceDb.Room.Where(x => x.AdminChat.Any(n=>n.MessageStatus == (int)MessageStatus.UnRead)).Select(y => new Room()
            {
                H_Id = y.H_Id,
                UserName = y.UserName,
                Email = y.Email,
                PhoneNo = y.PhoneNo,
                RoomText = y.RoomText,
                HelpType = y.HelpType,
                Users =y.Users,
                AdminChat = y.AdminChat,
            }).ToList();
            return Json(help); 

        }
        [Authorize]
        public IActionResult SaveREplybyRoomId(int id, string Message)
        {

            var userid = _userService.GetUserId();
            AdminChat adminChat = new AdminChat();
            adminChat.RoomId = id;
            adminChat.MessageBy = userid;
            adminChat.MessageText = Message;
            adminChat.MessageOn = DateTime.Now;
            adminChat.MessageStatus = (int)MessageStatus.UnRead;
            _commerceDb.AdminChat.Add(adminChat);
            _commerceDb.SaveChanges();
            var latestid = adminChat.A_ChatId;
            return Json(id);

        }
        public IActionResult GetHelpDesbyId(int id)
        {
            int? RoomId = _commerceDb.AdminChat.Where(y => y.A_ChatId == id).Select(y => y.RoomId).FirstOrDefault();
            var help = _commerceDb.AdminChat.Where(x => x.RoomId == RoomId).Select(z => new RoomViewModel()
            {
                UserId = z.User.UserId,
                RoomId = z.RoomId,
                HelpType = z.Room.HelpType,
                Room = z.Room,
                A_ChatId = z.A_ChatId,
                MessageText = z.MessageText,
                MessageBy = z.MessageBy,
                MessageOn = z.MessageOn,
                MessageStatus = z.MessageStatus,
          
            }).ToList();
            return Json(help);
        }

        public IActionResult GetHelpDesbyRoomId(int id)
        {
            var help = _commerceDb.AdminChat.Where(x => x.RoomId == id).Select(z => new RoomViewModel()
            {
                UserId = z.User.UserId,
                RoomId = z.RoomId,
                HelpType = z.Room.HelpType,
                Room = z.Room,
                A_ChatId = z.A_ChatId,
                MessageText = z.MessageText,
                MessageBy = z.MessageBy,
                MessageOn = z.MessageOn,
                MessageStatus = z.MessageStatus,

            }).ToList();
            return Json(help);
        }
        [HttpPost]
        public IActionResult ChangeHelpStatusbyId(int id, string ResponseText)
        {
            //Room helps = _commerceDb.Room.Where(x => x.H_Id == id).FirstOrDefault();
            //List<Room> helps1 = _commerceDb.Room.Where(y=>y.H_Id == id).ToList();
            List<AdminChat> adminChats = _commerceDb.AdminChat.Where(x => x.A_ChatId == id).ToList();
            AdminChat chat = _commerceDb.AdminChat.Where(y => y.A_ChatId == id).FirstOrDefault();
            foreach (var admin in adminChats)
            {              
                admin.MessageBy = chat.MessageBy;
                admin.MessageOn = chat.MessageOn;
                admin.MessageStatus = (int)MessageStatus.Read;
                admin.MessageText = chat.MessageText;
                admin.RoomId = chat.RoomId;
                admin.A_ChatId = chat.A_ChatId;
                _commerceDb.AdminChat.Update(admin);
                _commerceDb.SaveChanges();
            }
            //foreach (var item in helps1)
            //{
                //item.H_Id = helps.H_Id;
                //item.HelpText = helps.HelpText;
                //item.UploadOn = helps.UploadOn;
                //item.UserName = helps.UserName;
                //item.Email = helps.Email;
                //item.PhoneNo = helps.PhoneNo;
                //item.ResponseStatus = (int)ResponseStatus.No;
                ////item.H_Type = helps.H_Type;
                //item.UserId = helps.UserId;
                //item.R_Date = DateTime.Now.Date;
                //item.ResponseText = ResponseText;
                //_commerceDb.Help.Update(item);
                //_commerceDb.SaveChanges();
            //}
            return Json(chat);
        }

        [Authorize]
        public IActionResult GetHelpChatbyToken()
        {
            var userid = _userService.GetUserId();
            var roo = _commerceDb.Room.Where(x => x.UserId == userid).Select(y=> new Room(){
                RoomText = y.RoomText,
                UserName = y.UserName,
                UserId = y.UserId,
                AdminChat = y.AdminChat,
                

            }).ToList();
            return Json(roo);
        }

        public IActionResult GetHelpChatbySecretKey(int SecretKey)
        {
                 var roo = _commerceDb.Room.Where(x => x.SecretKey == SecretKey).Select(y => new Room()
                 {
                     UserName = y.UserName,
                     UserId = y.UserId,
                     RoomText = y.RoomText,
                     AdminChat = y.AdminChat,


                 }).ToList();
                return Json(roo);
        }
        [Authorize]
        public IActionResult GetAllChatUsers()
        {
            var usereid = _userService.GetUserId();
            var room = _commerceDb.Room.Select(x => new AllChatsUserViewModel()
            {
                RoomOn = x.RoomOn,
                RoomStatus = x.RoomStatus,
                RoomText = x.RoomText,
                R_Id = x.R_Id,
                //AdminChat = x.AdminChat,
                AdminId = x.AdminId,
                AssignedTo = x.AssignedTo,
                AssignedOn = x.AssignedOn,
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNo = x.PhoneNo,
                SecretKey = x.SecretKey,
                UserImage = x.Users.UserImage,
                RoleId = x.Users.UserRoles.Select(y => y.RoleId).FirstOrDefault(),
                H_Id = x.H_Id,
                Users = x.Users,

            }).ToList();
            return Json(room);
        }
        
        public IActionResult AddNewAdmin()
        {
            return View();
        }

        [Authorize]
        public IActionResult GetRoomId()
        {
            var userid = _userService.GetUserId();
            int? ids = _commerceDb.AdminChat.Where(x => x.MessageBy == userid).Select(x=>x.A_ChatId).FirstOrDefault();
            return Json(ids);
        }
        public IActionResult GetAllHelpsbyStatuse()
        {
            var adminChat = _commerceDb.AdminChat.Where(x => x.MessageStatus == (int)MessageStatus.UnRead && x.MessageBy == null).Select(z=> new RoomViewModel()
            {
                UserId =z.User.UserId,
                RoomId = z.RoomId,
                HelpType = z.Room.HelpType,
                Room =z.Room,
                A_ChatId = z.A_ChatId,
                MessageText = z.MessageText,
                MessageBy = z.MessageBy,
                MessageOn = z.MessageOn,
                MessageStatus = z.MessageStatus,
              
            }).ToList();
            return Json(adminChat);
        }
        public IActionResult ChangeAllDataonUserBlock()
        {
            var orders = _commerceDb.Orders.Where(x => (!(bool)x.Store.status || !(bool)x.OrderDetails.Buyer.status) && x.OrderDetails.OrderStatus != (int)ShippingStatus.Delivered).ToList();
            foreach (var item in orders)
            {
                OrderDetails ordetails = _commerceDb.OrderDetails.Where(x => x.OrderDetailsId == item.OrderDetailsId).FirstOrDefault();
                List<OrderDetails> orderDetails = _commerceDb.OrderDetails.Where(x => x.OrderDetailsId == item.OrderDetailsId).ToList();
                foreach (var od in orderDetails)
                {
                    od.UserName = ordetails.UserName;
                    od.FatherName = ordetails.FatherName;
                    od.Address = ordetails.Address;
                    od.City = ordetails.City;
                    od.Email = ordetails.Email;
                    od.Nationality = ordetails.Nationality;
                    od.Message = ordetails.Message;
                    od.PhoneNo = ordetails.PhoneNo;
                    od.PostalCode = ordetails.PostalCode;
                    od.OrderStatus = (int)ShippingStatus.Cancelled;
                    _commerceDb.OrderDetails.Update(od);
                    _commerceDb.SaveChanges();
                }
            }
            _commerceDb.Orders.RemoveRange(orders);
            _commerceDb.SaveChanges();

            var wishtlist = _commerceDb.WishList.Where(y => y.Product.User.status == false).ToList();
            _commerceDb.WishList.RemoveRange(wishtlist);
            _commerceDb.SaveChanges();

            var Cards = _commerceDb.CardData.Where(z => z.Products.User.status == false).ToList();
            _commerceDb.CardData.RemoveRange(Cards);
            _commerceDb.SaveChanges();
            return View();
        }
        [Authorize]
        public IActionResult GetUnreadMessages()
        {
            int userid = _userService.GetUserId();
            var orderdetails = _commerceDb.Chat.Where(x => x.ReceiverId == userid && x.M_Status == (int)MessageStatus.UnRead)
            .Select(z=> new NewNotification()
            {
               Sender =z.SenderChat,
               Reveiver =z.ReceiverChat,
               Message = z.MessageText,
               ChatOn =z.ChatOn,

            }).ToList(); 
            return Json(orderdetails);
        }
        public IActionResult ChatsHistory()
        {
            return View();
        }

        public IActionResult ChatsHistoryBuyerView()
        {
            return View();
        }

        public IActionResult GetImagebyProductImageId(int id)
        {
            var imageid = _commerceDb.ProductDescriptionimages.Where(x => x.ProductDescriptionImagesId == id).FirstOrDefault();
            return Json(imageid);
        }

        public IActionResult GetChatHistorybySecretKey(int secretkey)
        {

            var userid = _userService.GetUserId();
            string userimage = _commerceDb.Users.Where(x => x.UserId == secretkey).Select(x => x.UserImage).FirstOrDefault();
            string usernme = _commerceDb.Users.Where(x => x.UserId == secretkey).Select(x => x.UserName).FirstOrDefault();
            var chats123 = _commerceDb.Chat.Where(x => (x.SenderId == userid && x.ReceiverId == secretkey) || (x.SenderId == secretkey && x.ReceiverId == userid))
            .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                Id = secretkey,
                Userimage = userimage,
                UserName = usernme,

            }).ToList();
            return Json(chats123);
        }
        //public IActionResult ChatsHistoryBuyerView()
        //{
        //    return View();
        //}
        public IActionResult GetUserDatabySecretkey(int id)
        {
            User user = _commerceDb.Users.Where(x => x.SecretKey == id).FirstOrDefault();
            return Json(user);
        }

        public IActionResult ChatsHistoyAdminView()
        {
            return View();
        }
        [Authorize]
        public IActionResult GetAllChatsbyCurrentUser()
        {
            int userid = _userService.GetUserId();
            string CurrnetName1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserName).FirstOrDefault();
            string CurrentImage1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserImage).FirstOrDefault();
            var senderids = _commerceDb.Chat.Where(u => u.ReceiverId == userid).Select(u => u.SenderId).ToList();
            var newsenderids = senderids.Distinct().ToArray();
            var receiverids = _commerceDb.Chat.Where(r => r.SenderId == userid).Select(r => r.ReceiverId).ToList();
            var newreceiverids = receiverids.Distinct().ToArray();
            var chat = _commerceDb.Users.Where(u => newsenderids.Contains(u.UserId) || newreceiverids.Contains(u.UserId))
            .Select(q => new AllUsersChat()
            {
                UserId = q.UserId,
                UserName = q.UserName,
                UserImage = q.UserImage,
                CurrentName = CurrnetName1,
                CurrentImage = CurrentImage1,
                CurrentId = userid,

            }).ToList();
            return Json(chat);
        }

        public IActionResult GetAllChatsbySecretKey(int secretkey)
        {
            int userid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            string CurrnetName1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserName).FirstOrDefault();
            string CurrentImage1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserImage).FirstOrDefault();
            var senderids = _commerceDb.Chat.Where(u => u.ReceiverId == userid).Select(u => u.SenderId).ToList();
            var newsenderids = senderids.Distinct().ToArray();
            var receiverids = _commerceDb.Chat.Where(r => r.SenderId == userid).Select(r => r.ReceiverId).ToList();
            var newreceiverids = receiverids.Distinct().ToArray();
            var chat = _commerceDb.Users.Where(u => newsenderids.Contains(u.UserId) || newreceiverids.Contains(u.UserId))
            .Select(q => new AllUsersChat()
            {
                UserId = q.UserId,
                UserName = q.UserName,
                UserImage = q.UserImage,
                CurrentName = CurrnetName1,
                CurrentImage = CurrentImage1,
                CurrentId = userid,

            }).ToList();
            return Json(chat);
        }

        [Authorize]
        public IActionResult GetAllChatsbyCurrentUserAssigned()
        {
            int userid = _userService.GetUserId();
            //string CurrnetName1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserName).FirstOrDefault();
            //string CurrentImage1 = _commerceDb.Users.Where(x => x.UserId == userid).Select(x => x.UserImage).FirstOrDefault();
            //var senderids = _commerceDb.Chat.Where(u => u.ReceiverId == userid).Select(u => u.SenderId).ToList();
            //var newsenderids = senderids.Distinct().ToArray();
            //var receiverids = _commerceDb.Chat.Where(r => r.SenderId == userid).Select(r => r.ReceiverId).ToList();
            //var newreceiverids = receiverids.Distinct().ToArray();
            //var newreceiverid
            //var chat = _commerceDb.Users.Where(u => newsenderids.Contains(u.UserId) || newreceiverids.Contains(u.UserId))
            //.Select(q => new AllUsersChat()
            //{
            //    UserId = q.UserId,
            //    UserName = q.UserName,
            //    UserImage = q.UserImage,
            //    CurrentName = CurrnetName1,
            //    CurrentImage = CurrentImage1,
            //    CurrentId = userid,

            //}).ToList();
            return View();
        }

        [Authorize]
        public IActionResult GetChatbytoken()
        {
            var userid = _userService.GetUserId();

            int? chat = _commerceDb.Chat.Where(x => x.SenderId == userid).Select(x => x.ReceiverId).FirstOrDefault();
            int? chat1 = _commerceDb.Chat.Where(x => x.ReceiverId == userid).Select(x => x.SenderId).FirstOrDefault();
            string UserName = _commerceDb.Users.Where(x =>(x.UserId == chat1)||(x.UserId == chat)).Select(x => x.UserName).FirstOrDefault();
            string UserImage = _commerceDb.Users.Where(x => (x.UserId == chat1) || (x.UserId == chat)).Select(x=>x.UserImage).FirstOrDefault();
            if (chat == 0)
            {
                var cha = _commerceDb.Chat.Where(y=>(y.ReceiverId == chat && y.SenderId == userid ) 
                                                        || (y.SenderId == chat && y.ReceiverId == userid))
                        .Select(z => new AllChats()
                        {
                            MessgeId = z.Id,
                            ChatOn = z.ChatOn,
                            MessageText = z.MessageText,
                            Sender = z.SenderChat,
                            Id = chat,
                            UserName = UserName,
                            Userimage = UserImage,
                        }).ToList();
                return Json(cha);
            }
            var cha1 = _commerceDb.Chat.Where(y => (y.ReceiverId == chat1 && y.SenderId == userid)
                                                       || (y.SenderId == chat1 && y.ReceiverId == userid))
                       .Select(z => new AllChats()
                       {
                           ChatOn = z.ChatOn,
                           MessageText = z.MessageText,
                           MessgeId = z.Id,
                           Sender = z.SenderChat,
                           Id = chat,
                           UserName = UserName,
                           Userimage = UserImage,
                       }).ToList();
            return Json(cha1);
          
        }
        //[Authorize]
        //public IActionResult AssignTobyId()
        //{
        //    var userid = _userService.GetUserId();
        //    Room room = _commerceDb.Room.Where(x=>x.)
        //}
        //public IActionResult GetAllRooms()
        //{
        //    var rooms = _commerceDb.Room.Select(x=> new RoomViewModel()
        //    {
        //        UserId = z.User.UserId,
        //        RoomId = z.RoomId,
        //        HelpType = z.Room.HelpType,
        //        Room = z.Room,
        //        A_ChatId = z.A_ChatId,
        //        MessageText = z.MessageText,
        //        MessageBy = z.MessageBy,
        //        MessageOn = z.MessageOn,
        //        MessageStatus = z.MessageStatus,
        //    }).ToList();
        //}

        [Authorize]
        public IActionResult ChangeMessageStatusandSave(int id)
        {
            int userids = _userService.GetUserId();
            int? chat = _commerceDb.Chat.OrderBy(u=>u.ChatOn).Where(u => (u.SenderId == userids && u.ReceiverId == id) || (u.SenderId == id && u.ReceiverId == userids))
                                                                                     .Select(u => u.SenderId).LastOrDefault();
            if(userids == chat)
            {
                return Ok();
            }
            List<Chat> chats = _commerceDb.Chat.Where(x => (x.SenderId == userids && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == userids)).ToList();
            var chat1 =_commerceDb.Chat.Where(x => (x.SenderId == userids && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == userids))
                .Select(x=>x.Id).ToList();
            var chat123 = 0;
            foreach (var item in chats)
            {
                var val = chat123++;
                int chatval = chat1.ElementAtOrDefault(val);
                Chat chat3 = _commerceDb.Chat.Where(z => z.Id == chatval).FirstOrDefault();
                item.SenderId = chat3.SenderId;
                item.ReceiverId = chat3.ReceiverId;
                item.MessageText = chat3.MessageText;
                item.ChatOn = chat3.ChatOn;
                item.M_Status = (int)MessageStatus.Read;
                item.ViewOn = DateTime.Now.Date;
                _commerceDb.Chat.Update(item);
                _commerceDb.SaveChanges();
            }
            return Ok(); 
        }
        [Authorize]
        public IActionResult UpdateAssigningStatus(int RoomId, int AssignedTo)
        {
            var userid = _userService.GetUserId();
            Room room = _commerceDb.Room.Where(x => x.R_Id == RoomId).FirstOrDefault();
            List<Room> rooms = _commerceDb.Room.Where(x => x.R_Id == RoomId).ToList();
            foreach (var item in rooms)
            {
                item.UserName = room.UserName;
                item.Email = room.Email;
                item.PhoneNo = room.PhoneNo;
                item.RoomText = room.RoomText;
                item.H_Id = room.H_Id;
                item.AssignedTo = AssignedTo;
                item.AdminId = userid;
                item.AssignedOn = DateTime.Now.Date;
                item.SecretKey = room.SecretKey;
                item.RoomOn = room.RoomOn;
                item.UserId = room.UserId;
                item.RoomStatus = room.RoomStatus;
                _commerceDb.Room.Update(item);
                _commerceDb.SaveChanges();
            }
            return Json(RoomId);
        }


        [Authorize]
        public IActionResult SaveReplyChat(int receiverid, string Message)
        {
            int senderid = _userService.GetUserId();
            Chat chat1 = new Chat();
            chat1.SenderId = senderid;
            chat1.ReceiverId = receiverid;
            chat1.ChatOn = DateTime.Now.Date;
            chat1.MessageText = Message;
            chat1.M_Status = (int)MessageStatus.UnRead;
            _commerceDb.Chat.Add(chat1);
            _commerceDb.SaveChanges();
            return Json(receiverid);
        }

        public IActionResult SaveReplyChatbySecretKey(int receiverid, string Message, int secretkey)
        {
            int senderid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            Chat chat1 = new Chat();
            chat1.SenderId = senderid;
            chat1.ReceiverId = receiverid;
            chat1.ChatOn = DateTime.Now.Date;
            chat1.MessageText = Message;
            chat1.M_Status = (int)MessageStatus.UnRead;
            _commerceDb.Chat.Add(chat1);
            _commerceDb.SaveChanges();
            return Json(receiverid);
        }

        [Authorize]
        public IActionResult GetChatbyUserIdandStoreId(int id, int storeid)
        {
            int userid = _userService.GetUserId();
            
            string userimage = _commerceDb.Users.Where(x => x.UserId == id).Select(x => x.UserImage).FirstOrDefault();
            string usernme = _commerceDb.Users.Where(x => x.UserId == id).Select(x => x.UserName).FirstOrDefault();
            var chats123 = _commerceDb.Chat.Where(x => (x.SenderId == userid && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == userid))
            .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                Id = id,
                Userimage = userimage,
                UserName = usernme,

            }).ToList();
                return Json(chats123);
        }

        [HttpPost]
        public IActionResult GetChatbySecretKeyandStoreId(int id, int secretkey)
        {
            int userid = _commerceDb.Users.Where(x => x.SecretKey == secretkey).Select(x => x.UserId).FirstOrDefault();
            string userimage = _commerceDb.Users.Where(x => x.UserId == id).Select(x => x.UserImage).FirstOrDefault();
            string usernme = _commerceDb.Users.Where(x => x.UserId == id).Select(x => x.UserName).FirstOrDefault();
            var chats123 = _commerceDb.Chat.Where(x => (x.SenderId == userid && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == userid))
            .Select(z => new AllChats()
            {
                ChatOn = z.ChatOn,
                MessageText = z.MessageText,
                MessgeId = z.Id,
                Sender = z.SenderChat,
                Id = id,
                Userimage = userimage,
                UserName = usernme,

            }).ToList();
                return Json(chats123);
        }
        [Authorize]
        public IActionResult UserAndBuyerChats(int id, int storeid, string MessageText)
        {
            var userid = _userService.GetUserId();
            List<Chat> chat = _commerceDb.Chat.Where(x => x.ReceiverId == id && x.SenderId == userid).ToList();
            if (chat.Count == null)
            {
                Chat chat1 = new Chat();
                chat1.ReceiverId = id;
                chat1.SenderId = userid;
                chat1.MessageText = MessageText;
                chat1.ChatOn = DateTime.Now.Date;
                _commerceDb.Chat.Add(chat1);
                _commerceDb.SaveChanges();   
            }
            else
            {
                foreach(var item in chat)
                {
                    item.ReceiverId = id;
                    item.SenderId = userid;
                    item.MessageText = MessageText;
                    item.ChatOn = DateTime.Now.Date;
                    _commerceDb.Chat.Add(item);
                    _commerceDb.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAdminChatbyId(int id)
        {
            var chat = _commerceDb.AdminChat.Where(x => x.A_ChatId == id).FirstOrDefault();
            _commerceDb.AdminChat.Remove(chat);
            _commerceDb.SaveChanges();
            return Json(id);
        }

        public IActionResult UpdateMsgbyAdminChatId(int msgid, string Text)
        {
            AdminChat chat = _commerceDb.AdminChat.Where(x => x.A_ChatId == msgid).FirstOrDefault();
            List<AdminChat> chats = _commerceDb.AdminChat.Where(y => y.A_ChatId == msgid).ToList();

            foreach (var item in chats)
            {
                item.MessageText = Text;
                item.MessageBy = chat.MessageBy;
                item.MessageStatus = chat.MessageStatus;
                item.MessageOn = chat.MessageOn;
                //item.MessageText = chat.MessageText;
                item.RoomId = chat.RoomId;
                _commerceDb.AdminChat.Update(item);
                _commerceDb.SaveChanges();

            }
            return Json(msgid);
        }


        [HttpDelete]
        public IActionResult DeleteChatbyId(int id)
        {
            var chat = _commerceDb.Chat.Where(x => x.Id == id).FirstOrDefault();
            _commerceDb.Chat.Remove(chat);
            _commerceDb.SaveChanges();
            return Json(id);
        }

        public IActionResult UpdateMsgbyId(int msgid, string Text) 
        {
            Chat chat = _commerceDb.Chat.Where(x => x.Id == msgid).FirstOrDefault();
            List<Chat> chats = _commerceDb.Chat.Where(y => y.Id == msgid).ToList();

            foreach (var item in chats)
            {
                item.MessageText = Text;
                item.SenderId = chat.SenderId;
                item.ReceiverId = chat.ReceiverId;
                item.M_Status = chat.M_Status;
                item.ViewOn = chat.ViewOn;
                _commerceDb.Chat.Update(item);
                _commerceDb.SaveChanges();
                
            }
            return Json(msgid);
        }

        public IActionResult UpdateChatbyId(int ChatId, string Text)
        {
            return Json(ChatId, Text);  
        }

        public IActionResult DeleteRoombyId(int id)
        {
            AdminChat room = _commerceDb.AdminChat.Where(x => x.A_ChatId == id).FirstOrDefault();
            _commerceDb.AdminChat.Remove(room);
            _commerceDb.SaveChanges();
            return Json(id);
        }
        public IActionResult UpdateAdminChatbyId(int Id, string Text)
        {
            AdminChat adminChat = _commerceDb.AdminChat.Where(x => x.A_ChatId == Id).FirstOrDefault();
            List<AdminChat> admins = _commerceDb.AdminChat.Where(y => y.A_ChatId == Id).ToList();

            foreach (var item in admins)
            {
                item.MessageBy = adminChat.MessageBy;
                item.MessageOn = adminChat.MessageOn;
                item.MessageStatus = adminChat.MessageStatus;
                item.RoomId = adminChat.RoomId;
                item.MessageText = Text;
                _commerceDb.AdminChat.Update(item);
                _commerceDb.SaveChanges();
            }
            return Json(Id);
        }

       public IActionResult AssignPages()
        {
            return View();
        }

        [Authorize]
        public IActionResult Assigners(int id)
        {
            var userid = _userService.GetUserId();
            if (id == 1)
            {
                var room = _commerceDb.Room.Where(x => x.AssignedTo == userid).Select(y=> new RoomAllAssigner()
                {
                    userId = y.UserId,
                    adminId = y.AdminId,
                    secretKey = y.SecretKey,
                    assignedTo = y.AssignedTo,
                    assignedOn = y.AssignedOn,

                    admins= y.Admins,
                    users = y.Users,
                    assign = y.Assign,
                    userName = y.UserName,
                    phoneNo = y.PhoneNo,
                    email = y.Email,
                    h_Id = y.H_Id,
                    helpType = y.HelpType.TypeText,
                    roomText = y.RoomText,
                    roomOn = y.RoomOn,
                    roomStatus = y.RoomStatus,
                    r_Id = y.R_Id,
                }).ToList();
                return Json(room);
            }
            else 
            {
                var room = _commerceDb.Room.Where(x => x.AdminId == userid).Select(y => new RoomAllAssigner()
                {
                    userId = y.UserId,
                    adminId = y.AdminId,
                    secretKey = y.SecretKey,
                    assignedTo = y.AssignedTo,
                    assignedOn = y.AssignedOn,
                    admins = y.Admins,
                    users = y.Users,
                    assign = y.Assign,
                    userName = y.UserName,
                    phoneNo = y.PhoneNo,
                    email = y.Email,
                    h_Id = y.H_Id,
                    helpType = y.HelpType.TypeText,
                    roomText = y.RoomText,
                    roomOn = y.RoomOn,
                    roomStatus = y.RoomStatus,
                    r_Id = y.R_Id,
                }).ToList();
                return Json(room);
            }   
        }
        // [HttpGet]
        // public IActionResult SelectedSearch(int KidId, int Price, string Brand, int SizeId, int ColourId, string ProductName)
        // {

        // if (ProductName == null && Price == 0 && ColourId == 0 && SizeId == 0 && Brand == null)
        //    {
        //         var result = _commerceDb.Products.Where(p => p.KidCategoryId == KidId).ToList();
        //         return Json(result);
        //    }
        //else
        //   {
        //     IQueryable<Product> Test1 = _commerceDb.Products.AsQueryable<Product>();

        //     if (KidId != null && KidId != 0)
        //     {
        //         Test1 = Test1.Where(p => p.KidCategoryId == KidId);
        //     };
        //     if (ProductName != null)
        //     {
        //         Test1 = Test1.Where(p => p.ProductName == ProductName);
        //     };
        //     if (Price != 0 && Price != null)
        //     {
        //         Test1 = Test1.Where(p => p.ProductPrice == Price);
        //     };
        //     if (Brand != null)
        //     {
        //         Test1 = Test1.Where(p => p.ProductDescriptions.Any(y=>y.ProductBrand == Brand));
        //     };
        //     if (ColourId != 0 && (ColourId != null))
        //     {
        //         Test1 = Test1.Where(p => p.ProductColours.Any(c => c.Colour.ColourId == ColourId) );
        //     };
        //     if ((SizeId != null) && SizeId != 0)
        //     {
        //         Test1 = Test1.Where(p => p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId));
        //     }
        //     if (ProductName != null && Price != 0 && ColourId != 0 && SizeId != 0 && Brand != null)
        //     {
        //         Test1 = Test1.Where(p => p.KidCategoryId == KidId && p.ProductName.Contains(ProductName) && p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));
        //     }

        //     var result = Test1.Select(p => new Product()
        //     {
        //         ProductId = p.ProductId,
        //         ProductName = p.ProductName,
        //         ProductImage = p.ProductImage,
        //         ProductPrice = p.ProductPrice,
        //     }).ToList();
        //         return Json(result);
        //      }

        //     }
        //if (ProductName == null  && Price == 0 && ColourId == 0 && SizeId == 0 && Brand == null)
        //{
        //    Test1 = Test1.Where(p => p.KidCategoryId == KidId);
        //}
        ////if (KidId == 0 || KidId == null)
        ////{
        ////   Test1= Test1.Where(p =>p.KidCategoryId == KidId || p.ProductName.Contains(ProductName) || p.ProductPrice == Price || p.ProductDescriptions.Any(d => d.ProductBrand == Brand) || p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) || p.ProductColours.Any(c => c.Colour.ColourId == ColourId));

        ////};
        //if (string.IsNullOrWhiteSpace(ProductName))
        //{
        //    Test1=Test1.Where(p => p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));

        //};
        //if (Price == 0 || Price == null)
        //{
        //    Test1 = Test1.Where(p => p.KidCategoryId == KidId || p.ProductName.Contains(ProductName) && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));
        //};
        //if (string.IsNullOrWhiteSpace(Brand))
        //{
        //    Test1 = Test1.Where(p => p.KidCategoryId == KidId || p.ProductName.Contains(ProductName) && p.ProductPrice == Price && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));
        //};
        //if (ColourId == 0 || ColourId == null)
        //{
        //    Test1= Test1.Where(p => p.ProductPrice == Price && p.ProductName.Contains(ProductName) && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId));
        //};
        //if (SizeId == null || SizeId == 0)
        //{
        //    Test1=Test1.Where(p => p.KidCategoryId == KidId || p.ProductName.Contains(ProductName) && p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductPrice == Price && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));
        //}
        //if (ProductName != null && Price != 0 && ColourId != 0 && SizeId != 0 && Brand != null)
        //{
        //    Test1 = Test1.Where(p => p.KidCategoryId == KidId && p.ProductName.Contains(ProductName) && p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId));
        //}

        //var result= Test1.Select(p => new ProductSelectView()
        //{
        //    ProductId = p.ProductId,
        //    ProductName = p.ProductName,
        //    ProductImage = p.ProductImage,
        //    ProductPrice = p.ProductPrice,
        //}).ToList();
        //if (ProductId == 0 || ProductId == null)
        //{


        //    var products = _commerceDb.Products.Where(p => p.KidCategoryId == KidId && p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId))
        //   .Select(p => new ProductSelectView()
        //   {
        //       ProductId = p.ProductId,
        //       ProductName = p.ProductName,
        //       ProductImage = p.ProductImage,
        //       ProductPrice = p.ProductPrice,
        //   }).ToList();

        //}
        //else
        //{

        //    var products = _commerceDb.Products.Where(p => p.KidCategoryId == KidId && p.ProductId == ProductId && p.ProductPrice == Price && p.ProductDescriptions.Any(d => d.ProductBrand == Brand) && p.ProductSize.Any(s => s.Sizes.Sizeid == SizeId) && p.ProductColours.Any(c => c.Colour.ColourId == ColourId))
        //      .Select(p => new ProductSelectView()
        //      {
        //          ProductId = p.ProductId,
        //          ProductName = p.ProductName,
        //          ProductImage = p.ProductImage,
        //          ProductPrice = p.ProductPrice,
        //      }).ToList();
        //}
        //if (products.Count == null || products.Count == 0)
        //{
        //    return BadRequest();
        //}
        //else
        //{
        //    return Json(products);
        //}
        //}

        //    [Authorize]
        //    [HttpPost]
        //    public IActionResult SellerLogin(StoreViewModel storeViewModel)
        //    {

        //        Store store = _commerceDb.Stores.FirstOrDefault();
        //        if (storeViewModel.StoreName != store.StoreName || storeViewModel.StorePassword != store.StorePassword)
        //        {
        //            return Error();
        //        }
        //        else
        //        {
        //            return View();
        //        } 
        //    }

        //    public IActionResult RegisterSellerView()
        //    {
        //        return View();
        //    }

        //    [HttpPost]
        //    public IActionResult RegisterSeller(StoreViewModel save)
        //    {
        //        var userid = _userService.GetUserId();
        //        Store store = new Store();
        //        store.StoreName = save.StoreName;
        //        store.StoreEmail = save.StoreEmail;
        //        store.StoreContact = save.StoreContact;
        //        store.StorePassword = save.StorePassword;
        //        store.ShippingAddress = save.ShippingAddress;

        //        store.CreatedOn = DateTime.Now;

        //        string filepath = Path.GetFileName(save.file.FileName);
        //        string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
        //        store.StoreImage = imagespaths;
        //        _commerceDb.Stores.Add(store);
        //        _commerceDb.SaveChanges();
        //        return View();
        //    }

    }
}