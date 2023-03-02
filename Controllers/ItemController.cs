using DarazApp.Entities;
using DarazApp.Helpers;
using DarazApp.Models;
using DarazApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using static DarazApp.Models.JoinandViewModle;
using static DarazApp.Services.Program;
using static NuGet.Packaging.PackagingConstants;

namespace DarazApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly CommerceDbContext _commerceDbContext = new CommerceDbContext();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        public ItemController(IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _httpContextAccessor = contextAccessor;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductsCard()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddtoCart(AddToCartModel saveData)
        {
            var userid = _userService.GetUserId();
            int? batch= _commerceDbContext.ProductBatch.Where(g => g.P_VariationsId == saveData.VariationId && g.BatchId == saveData.BatchId).Select(g=>g.RemainingQuantity).FirstOrDefault();
            if (saveData.Quantity > batch)
            {
                return BadRequest();
            }
            //int productBatch = _commerceDbContext.ProductBatch.Where(u => u.P_VariationsId == saveData.VariationId && u.Price == saveData.ProductPrice).Select(n => n.BatchId).FirstOrDefault();
            var userRole = _commerceDbContext.UserRoles.Where(x => x.UserId == userid).Select(x => x.Role.RoleId).FirstOrDefault();
            if (userRole == (int)RolesEnum.Admin || userRole == (int)RolesEnum.Buyer)
            {

                User user = new User();
                Product product = new Product();
                CardData cord = _commerceDbContext.CardData.Where(x => x.UserId == userid && x.ProductId == saveData.ProductId && x.Variations_Id == saveData.VariationId && x.BatchId == saveData.BatchId).FirstOrDefault();
                if (cord is null)
                {
                    CardData cards = new CardData();
                    cards.ProductId = saveData.ProductId;
                    cards.Variations_Id = saveData.VariationId;
                    cards.UserId = userid;
                    cards.BatchId = saveData.BatchId;
                    cards.ProductQuantity = saveData.Quantity;
                    cards.ProductPrice = saveData.ProductPrice;
                    cards.TotalPrice = (cards.ProductQuantity) * (saveData.ProductPrice);
                    cards.UploadDate = DateTime.Now;
                    _commerceDbContext.CardData.Add(cards);
                    _commerceDbContext.SaveChanges();
                }
                else
                {
                    cord.ProductId = saveData.ProductId;
                    cord.UserId = userid;
                    cord.Variations_Id = saveData.VariationId;
                    cord.BatchId = saveData.BatchId;
                    cord.ProductQuantity += (saveData.Quantity);
                    cord.ProductPrice = saveData.ProductPrice;
                    cord.TotalPrice = (cord.ProductQuantity) * (saveData.ProductPrice);
                    cord.UploadDate = DateTime.Now;
                    _commerceDbContext.SaveChanges();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteCartData(SaveData saveData)
        {
            var userid = _userService.GetUserId();
            if (saveData.CardDataId == 0)
            {
                List<CardData> cardDatas = _commerceDbContext.CardData.Where(x => x.UserId == userid).ToList();
                _commerceDbContext.CardData.RemoveRange(cardDatas);
                _commerceDbContext.SaveChanges();
                return Ok();
            }
            else
            {
                List<CardData> carddata = _commerceDbContext.CardData.Where(x => x.CardDataId == saveData.CardDataId).ToList();
                _commerceDbContext.CardData.RemoveRange(carddata);
                _commerceDbContext.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        //[Authorize]
        //[HttpGet]
        public IActionResult CardView()
        {
            var userid = _userService.GetUserId();
            SaveData saveData = new SaveData();
            Sizes sizes321 = new Sizes();
            Product saved = new Product();
            var data = _commerceDbContext.Products.Where(x => x.UserId == userid)
                .Select(x => new ProductSelectView
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    ProductPrice = x.ProductPrice,

                    ProductViewModels = new ProductViewModel()
                    {
                        Sizes = x.ProductSize.Select(x => x.Sizes).ToList(),
                        Colours = x.ProductColours.Select(x => x.Colour).ToList(),
                        ProductDescriptions = x.ProductDescriptions.Select(x => new ProductDescription()
                        {
                            //P = x.P,
                            PdescriptionId = x.PdescriptionId,
                            ProductBrand = x.ProductBrand,
                            ProductDes = x.ProductDes,
                            ProductMaterial = x.ProductMaterial,
                            ProductQuality = x.ProductQuality,
                            ProductQuantity = x.ProductQuantity,
                            ProductType = x.ProductType,
                        }).ToList(),
                        ProductDescriptionimages = x.ProductDescriptionimages.Select(x => new ProductDescriptionimage()
                        {
                            ProductdesImage1 = x.ProductdesImage1,
                            ProductDescriptionImagesId = x.ProductDescriptionImagesId,
                        }).ToList(),
                        ProductSpecifications = x.ProductSpecifications.Select(x => new ProductSpecification()
                        {
                            ProductSpecificationId = x.ProductSpecificationId,
                            ProcessorCapacity = x.ProcessorCapacity,
                            ProductDisplay = x.ProductDisplay,
                            ProductShippingInfo = x.ProductShippingInfo,
                            ProductSpecificationsText = x.ProductSpecificationsText,
                            ProductWarrantyInfo = x.ProductWarrantyInfo,
                            CameraQuality = x.CameraQuality,
                            Graphics = x.Graphics,
                            Memory = x.Memory,
                        }).ToList(),
                        ProductSize = x.ProductSize.Select(x => new ProductSize()
                        {
                            ProductSizeId = x.ProductSizeId,
                        }).ToList(),
                        ProductColours = x.ProductColours.Select(x => new ProductColours()
                        {
                            ProductColourId = x.ProductColourId,
                        }).ToList(),
                    }
                }).FirstOrDefault();
            return Json(data);
        }
        [Authorize]
        [HttpGet]
        public IActionResult OrdersView()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult OrdersData()
        {
            var userid = _userService.GetUserId();
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Record()
        {
            var userid = _userService.GetUserId();
            var data = _commerceDbContext.CardData.Where(x => x.UserId == userid)
                .Select(x => new ItemCardViewModel()
                {
                    CardDataId = x.CardDataId,
                    ProductPrice = x.ProductPrice,
                    ProductQuantity = x.ProductQuantity,
                    TotalPrice = x.TotalPrice,
                    ProductName = x.Products.ProductName,
                    ProductImage = x.Products.ProductImage,
                    ProductId = x.ProductId,
                    StoreId =_commerceDbContext.Products.Where(c=>c.ProductId== x.ProductId ).Select(c=>c.UserId).FirstOrDefault(),
                    ProductVariations = x.ProductVariations,
                    ProductBatch = x.ProductBatch,

                }).ToList();
            return Json(data);
        }
        public IActionResult QuantityError()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddtoOrders(SaveData saveData)
        {
            var userid = _userService.GetUserId();
             if (saveData.CardDataId <= 0)
            {
                double? Commisionpercent = _commerceDbContext.Products.Where(c => c.ProductId == saveData.ProductId).Select(n => n.KidCategory.Comission).FirstOrDefault();

                var prodquantity = _commerceDbContext.ProductBatch.Where(x => x.ProductVariations.P_Id == saveData.ProductId && x.BatchId == saveData.BatchId && x.P_VariationsId == saveData.VariationId).Select(x => x.RemainingQuantity).FirstOrDefault();
                if (saveData.Quantity > prodquantity)
                {
                    return BadRequest();
                }
               OrderDetails orderDetails = new OrderDetails();

                orderDetails.UserName = saveData.UserName;
                orderDetails.FatherName = saveData.FatherName;
                orderDetails.Address = saveData.Address;
                orderDetails.City = saveData.City;
                orderDetails.PhoneNo = saveData.PhoneNo;
                orderDetails.Email = saveData.Email;
                orderDetails.Message = saveData.Message;
                orderDetails.Nationality = saveData.Nationality;
                orderDetails.UploadDate = DateTime.Now;
                orderDetails.PostalCode = saveData.PostalCode;
                orderDetails.MessageStatus = (int)MessageStatus.UnRead;
                orderDetails.UserId = userid;
                orderDetails.OrderStatus = (int)ShippingStatus.Waiting;

                _commerceDbContext.OrderDetails.Add(orderDetails);
                _commerceDbContext.SaveChanges();

                var sellerid = _commerceDbContext.Products.Where(x => x.ProductId == saveData.ProductId).Select(x => x.User.UserId).FirstOrDefault();
                Chat chat = new Chat();
                chat.SenderId = userid;
                chat.ReceiverId = sellerid;
                chat.MessageText = saveData.Message;
                chat.ChatOn = DateTime.Now.Date;
                chat.M_Status = (int)MessageStatus.UnRead;
                _commerceDbContext.Chat.Add(chat);
                _commerceDbContext.SaveChanges();

                var latestorderdetailId = orderDetails.OrderDetailsId;
                Orders orders = new Orders();

                orders.OrderDetailsId = latestorderdetailId;
                orders.ProductId = saveData.ProductId;
                orders.ProductQuantity = saveData.Quantity;
                orders.ProductPrice = saveData.ProductPrice;
                orders.TotalPrice = (saveData.ProductPrice) * (orders.ProductQuantity);
                var GrossSalt = (saveData.ProductPrice) * (orders.ProductQuantity);
                int? netmoney = (GrossSalt * Convert.ToInt16(Commisionpercent)) / 100;
                orders.Net_Price = (GrossSalt - netmoney);
                orders.UploadDate = DateTime.Now;
                orders.StoreId = saveData.StoreId;
                orders.VariationsId = saveData.VariationId;
                orders.RemainingQuantity = saveData.ProductQuantity;
                orders.IsDeleted = false;
                orders.DeleteOn = DateTime.Now.Date;
                orders.RemainingQuantity = saveData.Quantity;
                orders.BatchId = saveData.BatchId;
                _commerceDbContext.Orders.Add(orders);
                _commerceDbContext.SaveChanges();
                var NewOrderId = orders.OrderId;
                 ProductBatch productBatch = _commerceDbContext.ProductBatch.Where(x => x.ProductVariations.P_Id == saveData.ProductId && x.P_VariationsId == saveData.VariationId && x.BatchId == saveData.BatchId).FirstOrDefault();
                 productBatch.RemainingQuantity -= saveData.Quantity;
                productBatch.SaleOn = DateTime.Now.Date;
                _commerceDbContext.SaveChanges();

                App_Table app_Table = new App_Table();
                app_Table.ComissionValue = netmoney;
                app_Table.P_Commision = Commisionpercent;
                app_Table.OrderId = NewOrderId;
                app_Table.Product_Id = saveData.ProductId;
                app_Table.Uploadon = DateTime.Now.Date;
                _commerceDbContext.App_Table.Add(app_Table);
                _commerceDbContext.SaveChanges();
                return Json(latestorderdetailId);
             }
            else
            {
                double? Commisionpercent = _commerceDbContext.Products.Where(c => c.ProductId == saveData.ProductId).Select(n => n.KidCategory.Comission).FirstOrDefault();
                List<CardData> cardDatas1 = _commerceDbContext.CardData.Include(x=>x.Products).Where(x=>x.User.UserId == userid).ToList();
                    var Cards = cardDatas1.GroupBy(x => x.Products.UserId);
               if (Cards != null)
               {
                  
                    foreach (var cardData in Cards) 
                    {
                            OrderDetails orderDetails1 = new OrderDetails();
                            orderDetails1.UserName = saveData.UserName;
                            orderDetails1.FatherName = saveData.FatherName;
                            orderDetails1.Address = saveData.Address;
                            orderDetails1.City = saveData.City;
                            orderDetails1.PhoneNo = saveData.PhoneNo;
                            orderDetails1.Email = saveData.Email;
                            orderDetails1.Message = saveData.Message;
                            orderDetails1.Nationality = saveData.Nationality;
                            orderDetails1.UploadDate = DateTime.Now;
                            orderDetails1.PostalCode = saveData.PostalCode;
                            orderDetails1.UserId = userid;
                            orderDetails1.OrderStatus = (int)ShippingStatus.Waiting;
                            orderDetails1.MessageStatus = (int)MessageStatus.UnRead;
                            _commerceDbContext.OrderDetails.Add(orderDetails1);
                            _commerceDbContext.SaveChanges();
                            var latestorderdetailId = orderDetails1.OrderDetailsId;

                            foreach (var cardData12 in cardData)
                            {

                                Orders orders1 = new Orders();
                                orders1.ProductId = cardData12.ProductId;
                                orders1.ProductPrice = cardData12.ProductPrice;
                                orders1.ProductQuantity = cardData12.ProductQuantity;
                                orders1.RemainingQuantity = cardData12.ProductQuantity;
                                orders1.TotalPrice = (cardData12.ProductPrice) * (cardData12.ProductQuantity);
                                var GrossSalt1 = (cardData12.ProductPrice) * (cardData12.ProductQuantity);
                                int? netmoney = (GrossSalt1 * Convert.ToInt16(Commisionpercent)) / 100;
                                orders1.Net_Price = (GrossSalt1 - netmoney);
                                orders1.VariationsId = cardData12.Variations_Id;
                                orders1.BatchId = cardData12.BatchId;
                                orders1.IsDeleted = false;
                                orders1.DeleteOn = DateTime.Now.Date;
                            //orders1.ColourId = cardData12.ColourId;
                            //orders1.SizeId = cardData12.SizeId;
                                orders1.StoreId = saveData.StoreId;
                                orders1.UploadDate = DateTime.Now;
                                orders1.OrderDetailsId = latestorderdetailId;
                                _commerceDbContext.Orders.AddRange(orders1);
                                _commerceDbContext.SaveChanges();

                            var newOrderId = orders1.OrderId;
                            ProductBatch productBatch = _commerceDbContext.ProductBatch.Where(x => x.ProductVariations.P_Id == cardData12.ProductId && x.P_VariationsId == cardData12.Variations_Id && x.BatchId == cardData12.BatchId).FirstOrDefault();
                            productBatch.Quantity -= saveData.Quantity;
                            _commerceDbContext.SaveChanges();

                            App_Table app_Table = new App_Table();
                            app_Table.ComissionValue = netmoney;
                            app_Table.P_Commision = Commisionpercent;
                            app_Table.OrderId = newOrderId;
                            app_Table.OrderDetails_Id = latestorderdetailId;
                            app_Table.Product_Id = saveData.ProductId;
                            app_Table.Uploadon = DateTime.Now.Date;
                            _commerceDbContext.App_Table.Add(app_Table);
                            _commerceDbContext.SaveChanges();
                        }
                    }
               }
                    List<CardData> card = _commerceDbContext.CardData.Where(x => x.UserId == userid).ToList();
                    _commerceDbContext.CardData.RemoveRange(card);
                    _commerceDbContext.SaveChanges();
                    return Json(card);
             }
          
        }

        [Authorize]
        public IActionResult GetWishListLength()
        {
            var userid = _userService.GetUserId();
            List<WishList> wishList = _commerceDbContext.WishList.Where(x => x.UserId == userid).ToList();
            return Json(wishList);
        }

        //Orders orders1 = new Orders();
        //orders1.ProductId = cardData.;
        //orders1.ProductPrice = orders1.ProductPrice;
        //orders1.ProductQuantity = orders1.ProductQuantity;
        //orders1.TotalPrice = (orders1.ProductPrice) * (orders1.ProductQuantity);
        //orders1.ColourId = orders1.ColourId;
        //orders1.SizeId = orders1.SizeId;
        //orders1.UserId = userid;
        //orders1.UploadDate = DateTime.Now;
        //orders1.OrderDetailsId = latestorderdetailId;
        //_commerceDbContext.Orders.AddRange(orders1);
        //_commerceDbContext.SaveChanges();

        //orderDetails.UserName = saveData.UserName;
        //    orderDetails.FatherName = saveData.FatherName;
        //    orderDetails.Address = saveData.Address;
        //    orderDetails.City = saveData.City;
        //    orderDetails.PhoneNo = saveData.PhoneNo;
        //    orderDetails.Email = saveData.Email;
        //    orderDetails.Message = saveData.Message;
        //    orderDetails.Nationality = saveData.Nationality;
        //    orderDetails.UploadDate = DateTime.Now;
        //    orderDetails.PostalCode = saveData.PostalCode;
        //    orderDetails.UserId = userid;
        //    _commerceDbContext.OrderDetails.Add(orderDetails);
        //    _commerceDbContext.SaveChanges();
        //     var latestorderdetailId = orderDetails.OrderDetailsId;

        //    foreach (var item in cardDatas1)
        //    {

        //        Orders orders1 = new Orders();
        //        orders1.ProductId = item.ProductId;
        //        orders1.ProductPrice = item.ProductPrice;
        //        orders1.ProductQuantity = item.ProductQuantity;
        //        orders1.TotalPrice = (item.ProductPrice) * (item.ProductQuantity);
        //        orders1.ColourId = item.ColourId;
        //        orders1.SizeId = item.SizeId;
        //        orders1.UserId = userid;
        //        orders1.UploadDate = DateTime.Now;
        //        orders1.OrderDetailsId = latestorderdetailId;
        //        _commerceDbContext.Orders.AddRange(orders1);
        //        _commerceDbContext.SaveChanges();

        //    }





        //    orderDetails.UserName = saveData.UserName;
        //orderDetails.FatherName = saveData.FatherName;
        //orderDetails.Address = saveData.Address;
        //orderDetails.City = saveData.City;
        //orderDetails.PhoneNo = saveData.PhoneNo;
        //orderDetails.Email = saveData.Email;
        //orderDetails.Message = saveData.Message;
        //orderDetails.Nationality = saveData.Nationality;
        //orderDetails.UploadDate = DateTime.Now;
        //orderDetails.PostalCode = saveData.PostalCode;
        //orderDetails.UserId = userid;
        //_commerceDbContext.OrderDetails.Add(orderDetails);
        //_commerceDbContext.SaveChanges();
        //var latestorderdetailId = orderDetails.OrderDetailsId;
        //List<CardData> cardDatas = _commerceDbContext.CardData.Where(x => x.UserId == userid).ToList();
        //foreach (var item in cardDatas)
        //{

        //    Orders orders1 = new Orders();
        //    orders1.ProductId = item.ProductId;
        //    orders1.ProductPrice = item.ProductPrice;
        //    orders1.ProductQuantity = item.ProductQuantity;
        //    orders1.TotalPrice = (item.ProductPrice)*(item.ProductQuantity);
        //    orders1.ColourId = item.ColourId;
        //    orders1.SizeId = item.SizeId;
        //    orders1.UserId = userid;
        //    orders1.UploadDate = DateTime.Now;
        //    orders1.OrderDetailsId = latestorderdetailId;
        //    _commerceDbContext.Orders.AddRange(orders1);
        //    _commerceDbContext.SaveChanges();

        //}




        //[Authorize]
        public IActionResult CheckInPageView()
        {
            return View("CheckInPage");
        }
        [Authorize]
        [HttpPost]
        public JsonResult CheckInPage()
        {
            var userid = _userService.GetUserId();
            var shippedstatus = 0;
            var data = _commerceDbContext.OrderDetails.OrderBy(x => x.UploadDate).Where(x=>x.UserId == userid && x.Orders.Any(g=>g.IsDeleted != true)).Select(x=>x.OrderDetailsId).LastOrDefault();
            //var orderid = _commerceDbContext.Orders.Where(x => x.OrderDetails.UserId == userid && x.IsDeleted == false 
            //                                                                               && x.OrderDetails.OrderStatus != (int)ShippingStatus.Cancelled)
            //                                                                               .Select(x => x.OrderDetailsId).FirstOrDefault();
            var dataof = _commerceDbContext.OrderDetails.Where(x => x.OrderDetailsId == data)
            .Select(x => new CheckInSelectView()
            {
                ODOrderdetailId = x.OrderDetailsId,
                ODPhoneNo = x.PhoneNo,
                odUserImage = x.Buyer.UserImage,
                ODUserName = x.UserName,
                ODEmail = x.Email,
                ODNationality = x.Nationality,
                ShippingAddress = x.Address,
                UserName = x.Buyer.UserName,
                Stauts = x.OrderStatus,
                StatusName = ((ShippingStatus)x.OrderStatus).ToString(),

                UserId = x.Buyer.UserId,
                Address = x.Buyer.Address,
                Email = x.Buyer.Email,
                PhoneNo = x.Buyer.PhoneNo,
                PostalCode = x.Buyer.PostalCode,
                OrderId = x.Orders.Select(x => x.OrderId).FirstOrDefault(),
                UploadDate = x.Orders.Select(x => x.UploadDate).FirstOrDefault(),
                DeliveredOn = x.DeliveredOn,
                TotalPrice = x.Orders.Sum(g=>g.TotalPrice),

                OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.IsDeleted != true).Select(y => new OrderHistoryView()
                {
                    OrderId = y.OrderId,
                    ProductId = y.ProductId,
                    ProductQuantity = y.ProductQuantity,
                    RemainingQuantity = y.RemainingQuantity,
                    TotalPrice = y.TotalPrice,
                    NetPrice= y.Net_Price,
                    ProductName = y.Products.ProductName,
                    ProductImage = y.Products.ProductImage,
                    ProductPrice = y.Products.ProductPrice,
                    ProductVariations = y.ProductVariations,
                    ProductBatches = y.ProductBatch,
                    ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.Buyer.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID)
                    .Select(z => new ProductComments()
                    {
                        CommentDescription = z.CommentDescription,
                        Rating = z.Rating,
                    }).ToList(),

                }).ToList(),

            }).ToList();
            return Json(dataof);
        }
        public  IActionResult GetAllOrders()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult AllOrders()
        { 
            var usersid = _userService.GetUserId();
            int? rolelid = _commerceDbContext.UserRoles.Where(x => x.UserId == usersid).Select(x => x.RoleId).FirstOrDefault();
            var dataof = _commerceDbContext.OrderDetails.Where(x => x.UserId == usersid && x.Orders.Any(g => g.IsDeleted != true))
            .Select(x => new CheckInSelectView()
            {
                UserId = x.UserId,
                ODEmail = x.Email,
                ODNationality = x.Nationality,
                PhoneNo = x.PhoneNo,
                ODUserName = x.UserName,
                Stauts = x.OrderStatus,
                StatusName = ((ShippingStatus)x.OrderStatus).ToString(),
                Address = x.Address,
                UploadDate = x.UploadDate,
                DeliveredOn = x.DeliveredOn,
                ODOrderdetailId = x.OrderDetailsId,
                TotalPrice = x.Orders.Sum(g => g.TotalPrice),
                OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.IsDeleted != true).Select(y => new OrderHistoryView()
                {
                    OrderId = y.OrderId,
                    ProductId = y.ProductId,
                    ProductQuantity = y.ProductQuantity,
                    RemainingQuantity = y.RemainingQuantity,
                    TotalPrice = y.TotalPrice,
                    NetPrice = y.Net_Price,
                    ProductName = y.Products.ProductName,
                    ProductImage = y.Products.ProductImage,
                    ProductPrice = y.Products.ProductPrice,
                    ProductVariations = y.ProductVariations,
                    ProductBatches = y.ProductBatch,
                    ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
                    {
                        CommentDescription = z.CommentDescription,
                        Rating = z.Rating,
                    }).ToList(),


                }).ToList(),

            }).ToList();
            return Json (dataof);
        }

        [HttpGet]
        [Authorize]
        public IActionResult All_Ordersbyid(int id)
        {
            int? rolelid = _commerceDbContext.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).FirstOrDefault();
            
            var dataof = _commerceDbContext.OrderDetails.Where((rolelid == (int)RolesEnum.Buyer) ? x => x.UserId == id && x.Orders.Any(g => g.IsDeleted != true) : (rolelid ==(int)RolesEnum.Store) ? x => x.Orders.Any(x => x.StoreId == id) && x.Orders.Any(g => g.IsDeleted != true) : x=>x.UserId == id && x.Orders.Any(g=>g.IsDeleted != true))
            .Select(x => new CheckInSelectView()
            {
                UserId = x.UserId,
                ODEmail = x.Email,
                ODNationality = x.Nationality,
                PhoneNo = x.PhoneNo,
                ODUserName = x.UserName,
                Stauts = x.OrderStatus,
                StatusName = ((ShippingStatus)x.OrderStatus).ToString(),
                Address = x.Address,
                UploadDate = x.UploadDate,
                DeliveredOn = x.DeliveredOn,
                ODOrderdetailId = x.OrderDetailsId,
                TotalPrice = x.Orders.Sum(g => g.TotalPrice),
                OrderHistoryView = x.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.IsDeleted != true).Select(y => new OrderHistoryView()
                {
                    OrderId = y.OrderId,
                    ProductId = y.ProductId,
                    ProductQuantity = y.ProductQuantity,
                    RemainingQuantity = y.RemainingQuantity,
                    TotalPrice = y.TotalPrice,
                    NetPrice = y.Net_Price,
                    ProductName = y.Products.ProductName,
                    ProductImage = y.Products.ProductImage,
                    ProductPrice = y.Products.ProductPrice,
                    ProductVariations = y.ProductVariations,
                    ProductBatches = y.ProductBatch,
                    ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
                    {
                        CommentDescription = z.CommentDescription,
                        Rating = z.Rating,
                    }).ToList(),
                }).ToList(),
            }).ToList();
            return Json(dataof);
            //: (rolelid == (int)RolesEnum.Store) ? _commerceDbContext.OrderDetails.Where(x => x.Orders.Any(x => x.StoreId == id) && x.Orders.Any(g => g.IsDeleted != true))

            //.Select(x => new CheckInSelectView()
            //{
            //    UserId = x.UserId,
            //    ODEmail = x.Email,
            //    ODNationality = x.Nationality,
            //    PhoneNo = x.PhoneNo,
            //    ODUserName = x.UserName,
            //    Stauts = x.OrderStatus,
            //    StatusName = ((ShippingStatus)x.OrderStatus).ToString(),
            //    Address = x.Address,
            //    UploadDate = x.UploadDate,
            //    DeliveredOn = x.DeliveredOn,
            //    ODOrderdetailId = x.OrderDetailsId,
            //    TotalPrice = x.Orders.Sum(g => g.TotalPrice),
            //    OrderHistoryView = x.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId && y.IsDeleted != true).Select(y => new OrderHistoryView()
            //    {
            //        OrderId = y.OrderId,
            //        ProductId = y.ProductId,
            //        ProductQuantity = y.ProductQuantity,
            //        TotalPrice = y.TotalPrice,
            //        NetPrice = y.Net_Price,
            //        ProductName = y.Products.ProductName,
            //        ProductImage = y.Products.ProductImage,
            //        ProductPrice = y.Products.ProductPrice,
            //        ProductVariations = y.ProductVariations,
            //        ProductBatches = y.ProductBatch,
            //        ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId && z.OrderDetailsId == x.OrderDetailsId && z.VariationId == y.ProductVariations.P_VariationsID).Select(z => new ProductComments()
            //        {
            //            CommentDescription = z.CommentDescription,
            //            Rating = z.Rating,
            //        }).ToList(),


            //    }).ToList(),

            //}).ToList() : null;

        }

        [Authorize]
        [HttpPost]
        public IActionResult ReturnProductbyOrderId(int ProductId, int OrderId, int VariationId, int BatchId, int ReturnNumber,string Reason, int ReturnQuantity, IFormFile Image)
        {
            int? orders2 = _commerceDbContext.Orders.Where(x => x.OrderId == OrderId).Select(x => x.ProductQuantity).FirstOrDefault();
            if (ReturnQuantity > orders2)
            {
                return BadRequest();
            }

            int? OrderdetailId = _commerceDbContext.Orders.Where(x => x.OrderId == OrderId).Select(x => x.OrderDetailsId).FirstOrDefault();
            double? Commisionpercent = _commerceDbContext.Products.Where(x => x.ProductId == ProductId).Select(x => x.KidCategory.Comission).FirstOrDefault();
            var userid = _userService.GetUserId();
            List<Orders> orders = _commerceDbContext.Orders.Where(x => x.OrderId == OrderId).ToList();
            Orders orders1 = _commerceDbContext.Orders.Where(y => y.OrderId == OrderId).FirstOrDefault();
            foreach (var item in orders)
            {
                item.ProductId = orders1.ProductId;
                item.ProductPrice = orders1.ProductPrice;
                item.ProductQuantity = orders1.ProductQuantity;
                item.RemainingQuantity = (item.RemainingQuantity - ReturnQuantity);
                item.OrderDetailsId = orders1.OrderDetailsId;
                item.ProductPrice = orders1.ProductPrice;
                item.TotalPrice = (item.RemainingQuantity * item.ProductPrice);
               
                var GrossSalt = (orders1.ProductPrice) * (item.RemainingQuantity);
                int? netmoney = (GrossSalt * Convert.ToInt16(Commisionpercent)) / 100;
                item.Net_Price = (GrossSalt - netmoney);
                item.VariationsId = orders1.VariationsId;
                item.BatchId = orders1.BatchId;
                item.DeleteOn = orders1.DeleteOn;
                if (item.RemainingQuantity == 0)
                {
                    item.IsDeleted = true;
                }
                item.StoreId = orders1.StoreId;
                item.UploadDate = orders1.UploadDate;
                _commerceDbContext.Orders.Update(item);
                _commerceDbContext.SaveChanges();

                ProductBatch batch = _commerceDbContext.ProductBatch.Where(x => x.BatchId == BatchId).FirstOrDefault();
                List<ProductBatch> batches = _commerceDbContext.ProductBatch.Where(x => x.BatchId == BatchId).ToList();
                foreach (var batch1 in batches)
                {
                    batch1.Price = batch.Price;
                    batch1.Quantity = batch.Quantity;
                    batch1.RemainingQuantity += ReturnQuantity;
                    batch1.UploadOn = batch.UploadOn;
                    batch1.SaleOn = batch.SaleOn;
                    batch1.P_VariationsId = batch.P_VariationsId;
                    _commerceDbContext.ProductBatch.Update(batch1);
                    _commerceDbContext.SaveChanges();
                }

                int Order_detailsId = _commerceDbContext.Orders.Where(p => p.OrderId == OrderId).Select(p => p.OrderDetailsId).FirstOrDefault();

                ReturnItems return1 = new ReturnItems();
                
                return1.Ret_By = userid;
                return1.Ret_Quantity = ReturnQuantity;
                return1.R_Number = ReturnNumber;
                return1.P_Id = ProductId;
                return1.Var_Id = VariationId;
                return1.B_id = BatchId;
                return1.Order_Id = Order_detailsId;
                return1.OrderItems_Id = OrderId;
                return1.R_On = DateTime.Now.Date;
                return1.Ret_Reason = Reason;
                string filepath = Path.GetFileName(Image.FileName);
                string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                return1.ProductImage = imagespaths;
                _commerceDbContext.ReturnItems.Add(return1);
                _commerceDbContext.SaveChanges();

                App_Table app_ = _commerceDbContext.App_Table.Where(x => x.OrderId == OrderId && x.OrderDetails_Id == OrderdetailId).FirstOrDefault();
                List<App_Table> tables = _commerceDbContext.App_Table.Where(x=>x.OrderId == OrderId && x.OrderDetails_Id == OrderdetailId).ToList();
                foreach (var apptable in tables)
                {
                    apptable.ComissionValue = netmoney;
                    apptable.P_Commision = Commisionpercent;
                    apptable.OrderId = OrderId;
                    apptable.OrderDetails_Id = OrderdetailId;
                    apptable.Product_Id = app_.Product_Id;
                    apptable.Uploadon = DateTime.Now.Date;
                    _commerceDbContext.App_Table.Update(apptable);
                    _commerceDbContext.SaveChanges();
                }
            }
          
            return Json(orders2);
        }

        [HttpGet]
        public IActionResult GetProductsbyOrderDId(int id)
        {
            //Product product = _commerceDbContext.Products.Where(y => y.ProductId == id).FirstOrDefault();
            Product orders = _commerceDbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return Json(orders);
        }

        [HttpPost]
        public IActionResult AllOrdersbyUserId(int id)
        {
            var roleid = _commerceDbContext.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).FirstOrDefault();
            if (roleid == ((int)RolesEnum.Store)) 
            { 
            var orderid = _commerceDbContext.Orders.Where(x => x.StoreId == id).OrderBy(x => x.UploadDate).Select(x => x.OrderDetailsId).LastOrDefault();
            var dataof = _commerceDbContext.OrderDetails.Where(x => x.Orders.Any(y=>y.Products.UserId==id ))
            .Select(x => new CheckInSelectView()
            {
                ODOrderdetailId = x.OrderDetailsId,
                ODPhoneNo = x.PhoneNo,
                odUserImage = x.Buyer.UserImage,
                ODUserName = x.UserName,
                ODEmail = x.Email,
                ODNationality = x.Nationality,
                ShippingAddress = x.Address,
                UserName = x.Buyer.UserName,
                Stauts = x.OrderStatus,
                //UserImage = x.User.UserImage,
                UserId = x.Buyer.UserId,
                Address = x.Buyer.Address,
                Email = x.Buyer.Email,
                PhoneNo = x.Buyer.PhoneNo,
                PostalCode = x.Buyer.PostalCode,
                OrderId = x.Orders.Select(x => x.OrderId).FirstOrDefault(),
                UploadDate = x.Orders.Select(x => x.UploadDate).FirstOrDefault(),

                OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
                {
                    ProductId = y.ProductId,
                    ProductQuantity = y.ProductQuantity,
                    TotalPrice = y.TotalPrice,
                    ProductName = y.Products.ProductName,
                    ProductImage = y.Products.ProductImage,
                    ProductPrice = y.Products.ProductPrice,
                    ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.Buyer.UserId).Select(z => new ProductComments()
                    {
                        CommentDescription = z.CommentDescription,
                        Rating = z.Rating,
                    }).ToList(),

                }).ToList(),

            }).ToList();
                return Json(dataof);
            }
            else
            {
                var orderid = _commerceDbContext.Orders.Where(x => x.StoreId == id).Select(x => x.OrderDetailsId).FirstOrDefault();
                var dataof = _commerceDbContext.OrderDetails.Where(x => x.OrderDetailsId == orderid)
                .Select(x => new CheckInSelectView()
                {
                    ODOrderdetailId = x.OrderDetailsId,
                    ODPhoneNo = x.PhoneNo,
                    odUserImage = x.Buyer.UserImage,
                    ODUserName = x.UserName,
                    ODEmail = x.Email,
                    ODNationality = x.Nationality,
                    ShippingAddress = x.Address,
                    UserName = x.Buyer.UserName,
                    Stauts = x.OrderStatus,
                    //UserImage = x.User.UserImage,
                    UserId = x.Buyer.UserId,
                    Address = x.Buyer.Address,
                    Email = x.Buyer.Email,
                    PhoneNo = x.Buyer.PhoneNo,
                    PostalCode = x.Buyer.PostalCode,
                    OrderId = x.Orders.Select(x => x.OrderId).FirstOrDefault(),
                    UploadDate = x.Orders.Select(x => x.UploadDate).FirstOrDefault(),

                    OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
                    {
                        ProductId = y.ProductId,
                        ProductQuantity = y.ProductQuantity,
                        TotalPrice = y.TotalPrice,
                        ProductName = y.Products.ProductName,
                        ProductImage = y.Products.ProductImage,
                        ProductPrice = y.Products.ProductPrice,
                        ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.Buyer.UserId).Select(z => new ProductComments()
                        {
                            CommentDescription = z.CommentDescription,
                            Rating = z.Rating,
                        }).ToList(),

                    }).ToList(),

                }).ToList();
                return Json(dataof);
            }   
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteOrder(SaveData saveData)
        {
            Orders orders = new Orders();
            var orderdelete = _commerceDbContext.Orders.Where(x => x.OrderDetailsId == saveData.OrderDetailsId).ToList();
            _commerceDbContext.Orders.RemoveRange(orderdelete);
            _commerceDbContext.SaveChanges();
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAddress(SaveData saveData)
        {
            var userid = _userService.GetUserId();
            Orders orders = new Orders();
            var orderdelete = _commerceDbContext.Orders.Where(x => x.OrderId == saveData.OrderId).ToList();
            _commerceDbContext.Orders.RemoveRange(orderdelete);
            _commerceDbContext.SaveChanges();
            return View();
        }
        public IActionResult AddInvoice()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetsCardData()
        {
            var userid = _userService.GetUserId();
            var cards = _commerceDbContext.CardData.Where(x => x.UserId == userid)
                .Select(x=> new CardItemsView() 
                { 
                   ProductName = x.Products.ProductName,
                   ProductImage =x.Products.ProductImage,
                   ProductPrice =x.Products.ProductPrice,

                }).ToList();
            return Json(cards);
        }
        public IActionResult AddColour()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddColour(string id)
        {
            List<Colour> colour1 = _commerceDbContext.Colour.Where(x=>x.ColourName.Contains(id)).ToList();
            if (colour1.Count >= 1)
            {
                return BadRequest();
            }
            else
            {
                Colour colour = new Colour();
                colour.ColourName = id;
                _commerceDbContext.Colour.Add(colour);
                _commerceDbContext.SaveChanges();
                return Ok();

            }
        }
        public IActionResult AddSize()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSizes(string id)
        {
            List<Sizes> sizes1 = _commerceDbContext.Sizes.Where(x => x.SizeName.Contains(id)).ToList();
            if (sizes1.Count >= 1)
            {
                return BadRequest();
            }
            else
            {
                Sizes sizes = new Sizes();
                sizes.SizeName = id;
                _commerceDbContext.Sizes.Add(sizes);
                _commerceDbContext.SaveChanges();
                return Ok();
            }
        }
        public IActionResult AddtoWishList()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddtoWishtLists(int id)
        {
            var userid = _userService.GetUserId();
            List<WishList> wish = _commerceDbContext.WishList.Where(x => x.UserId == userid && x.ProductId == id).ToList();
            if (wish.Count >= 1)
            {
                WishList list = _commerceDbContext.WishList.Where(x => x.UserId == userid && x.ProductId == id).FirstOrDefault();
                _commerceDbContext.WishList.Remove(list);
                _commerceDbContext.SaveChanges();
            }
            else
            {
            WishList wishList = new WishList();
            wishList.ProductId = id;
            wishList.UserId = userid;
            wishList.Date = DateTime.Now;
            _commerceDbContext.WishList.Add(wishList);
            _commerceDbContext.SaveChanges();
            }
            return Json(wish);
        }
        public IActionResult GetWishListView()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetWishtList()
        {
            var userid = _userService.GetUserId();
            List <WishList> wishLists =_commerceDbContext.WishList.ToList();
            var wish = _commerceDbContext.WishList.Where(x=>x.UserId == userid).Select(x => new CheckInSelectView()
            {
                WishListId =x.WishListid,
                ProductId = x.ProductId,
                UploadDate =x.Date,
                UserName = x.User.UserName,
                UserImage = x.User.UserImage,
                ProductName = x.Product.ProductName,
                ProductImage = x.Product.ProductImage,
                ProductPrice = x.Product.ProductPrice,
                ProductDescription = x.Product.ProductDescriptions.Where(y=>y.ProductId == x.ProductId).ToList(),
            }).ToList();
            return Json (wish);
        }
      
        public IActionResult DeleteWishListss(int id)
        {
            var wishList = _commerceDbContext.WishList.Where(x => x.WishListid == id).ToList();
            _commerceDbContext.WishList.RemoveRange(wishList);
            _commerceDbContext.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public IActionResult DeleteWishList(int id)
        {
            var userid = _userService.GetUserId();
            var wishList = _commerceDbContext.WishList.Where(x => x.WishListid == id && x.UserId == userid).ToList();
            _commerceDbContext.WishList.RemoveRange(wishList);
            _commerceDbContext.SaveChanges();
            return View();
        }
        //[HttpPost]
        public IActionResult GetProductsbyUserIdAdminView()
        {
            return View();
        }
        public IActionResult GetProductsbyUserId()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetProductsbySellerId(int id)
        {
            List<Product> product = _commerceDbContext.Products.Where(x => x.UserId == id).ToList();   
            return Json(product);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetProductbyLogedInUserId()
        {
            var userid = _userService.GetUserId();
            var product = _commerceDbContext.Products.Where(x => x.User.status != false && x.UserId == userid).OrderByDescending(x => x.UploadDate).Select(y => new ProductSelectView()
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
            return Json(product);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetProdSellerView()
        {
            var userids = _userService.GetUserId();
            List<Product> product = _commerceDbContext.Products.Where(x => x.UserId == userids).ToList();
            return Json(product);
        }
         public IActionResult GetStatuesInput()
            {
            var directions = from ShippingStatus d in Enum.GetValues(typeof(ShippingStatus))
                             select new { Id = (int)d, Name = d.ToString() };
            return Json(directions);
          
            }
        [HttpPost]
        public IActionResult ChangeShippingStatus(StatusModel save)
        {
            var ord = _commerceDbContext.OrderDetails.Where(y => y.OrderDetailsId == save.OrderDetailsId).FirstOrDefault();
            var orderDetails = _commerceDbContext.OrderDetails.Where(x => x.OrderDetailsId == save.OrderDetailsId).ToList();
            foreach (var item in orderDetails)
            {
                item.OrderDetailsId = ord.OrderDetailsId;
                item.UserName = ord.UserName;
                item.UploadDate = ord.UploadDate;
                item.Address = ord.Address;
                item.Email = ord.Email;
                item.PhoneNo = ord.PhoneNo;
                item.Nationality = ord.Nationality;
                item.FatherName = ord.FatherName;
                item.UserId = ord.UserId;
                item.DeliveredOn = DateTime.Now.Date;
                item.OrderStatus = save.StatusName;
                item.PostalCode = ord.PostalCode;
                item.UploadDate = ord.UploadDate;
                item.City = save.City;

                _commerceDbContext.OrderDetails.Update(item);
                _commerceDbContext.SaveChanges();
            }
            
            return Json(ord);
        }
        public IActionResult GetOrdersSellerView()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetOrderSellerbyLogIn()
        {
            var userids = _userService.GetUserId();
            var roleid = _commerceDbContext.UserRoles.Where(x => x.UserId == userids).Select(y => y.Role.RoleId).FirstOrDefault();
            var dataof = _commerceDbContext.OrderDetails.Where(x => x.Orders.Any(y => y.StoreId == userids))
                .Select(x => new OrdersSelectView()
                {
                    ODOrderdetailId = x.OrderDetailsId,
                    ODPhoneNo = x.PhoneNo,
                    ODEmail = x.Email,
                    ODNationality = x.Nationality,
                    ShippingAddress = x.Address,
                    UserName = x.UserName,
                    Stauts = x.OrderStatus,
                    StatusName=((ShippingStatus) x.OrderStatus).ToString(),
                    UploadDate = x.UploadDate,

                    OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
                    {            
                        OrderId = y.OrderId,
                           ProductId = y.Products.ProductId,
                           ProductName =y.Products.ProductName,
                           ProductImage = y.Products.ProductImage,
                           ProductQuantity = y.ProductQuantity,
                           RemainingQuantity = y.RemainingQuantity,
                           TotalPrice =y.TotalPrice,
                           NetPrice = y.Net_Price,
                           ProductComments = y.Products.ProductComments.Where(c=>c.ProductId == y.Products.ProductId &&c.VariationId == y.ProductVariations.P_VariationsID && c.OrderDetailsId == x.OrderDetailsId)
                           .Select(p=> new ProductComments()
                           {
                               CommentDescription = p.CommentDescription,
                               Rating =p.Rating,
                               CommentedOn = p.CommentedOn,
                           }).ToList(),
                    }).ToList(),
                }).ToList();
            return Json(dataof);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetOrdersSellerViews(int Id)
        {
            var userids = _userService.GetUserId();
            var roleid = _commerceDbContext.UserRoles.Where(x => x.UserId == Id).Select(y => y.Role.RoleId).FirstOrDefault();

            if (roleid == (int)RolesEnum.Store)
            {

                var dataof = _commerceDbContext.Orders.Where(x => x.Products.UserId == Id)
           .Select(x => new OrdersSelectView()
           {
               ODOrderdetailId = x.OrderDetails.OrderDetailsId,
               ODPhoneNo = x.OrderDetails.PhoneNo,
               ODEmail = x.OrderDetails.Email,
               ODNationality = x.OrderDetails.Nationality,
               ShippingAddress = x.OrderDetails.Address,
               UserName = x.OrderDetails.UserName,
               Stauts = x.OrderDetails.OrderStatus,
               UploadDate = x.OrderDetails.UploadDate,
               OrderId = x.OrderId,
               OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
               {
                   ProductId = y.ProductId,
                   ProductQuantity = y.ProductQuantity,
                   RemainingQuantity = y.RemainingQuantity,
                   TotalPrice = y.TotalPrice,
                   ProductName = y.Products.ProductName,
                   ProductImage = y.Products.ProductImage,
                   ProductPrice = y.Products.ProductPrice,
                   ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == Id).Select(z => new ProductComments()
                   {
                       CommentDescription = z.CommentDescription,
                       Rating = z.Rating,
                   }).ToList(),

               }).ToList(),

           }).ToList();
                return Json(dataof);
            }
            else
            {
                var dataof = _commerceDbContext.OrderDetails.Where(x => x.UserId == Id)
                .Select(x => new CheckInSelectView()
                {
                    UserId = x.UserId,
                    ODEmail = x.Email,
                    ODNationality = x.Nationality,
                    ODPhoneNo = x.PhoneNo,
                    ODUserName = x.UserName,
                    Stauts = x.OrderStatus,
                    Address = x.Address,
                    UploadDate = x.UploadDate,
                    ODOrderdetailId = x.OrderDetailsId,
                    OrderHistoryView = _commerceDbContext.Orders.Where(y => y.OrderDetailsId == x.OrderDetailsId).Select(y => new OrderHistoryView()
                    {
                        ProductId = y.ProductId,
                        ProductQuantity = y.ProductQuantity,
                        RemainingQuantity = y.RemainingQuantity,
                        TotalPrice = y.TotalPrice,
                        ProductName = y.Products.ProductName,
                        ProductImage = y.Products.ProductImage,
                        ProductPrice = y.Products.ProductPrice,
                        ProductComments = y.Products.ProductComments.Where(z => z.ProductId == y.ProductId && z.UserId == x.UserId).Select(z => new ProductComments()
                        {
                            CommentDescription = z.CommentDescription,
                            Rating = z.Rating,
                        }).ToList(),

                    }).ToList(),

                }).ToList();
                return Json(dataof);
            }
        }
        public IActionResult GOrdersSellerView()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetOrdersbySellers()
        {
            var userid = _userService.GetUserId();
            var userRoles = _commerceDbContext.UserRoles.Where(y => y.RoleId == (int)RolesEnum.Store).Select(z => z.User.UserId).ToList();
            int sellerroleid = 1;
            int adminroleid = 3;
            var variations = _commerceDbContext.Orders.Select(g => g.VariationsId).ToList();
            var obj = _commerceDbContext.Orders.Where(x => userRoles.Contains(x.StoreId)).Select(p => new OrdersSelllerView
            {
                StoreId = p.Store.UserId,
                StoreName = p.Store.UserName,
                StoreAddress = p.Store.Address,
                StoreCity = p.Store.City,
                StoreEmail = p.Store.Email,
                StoreCnic = p.Store.Cnic,
                StoreNationality = p.Store.Nationality,
                StorePhoneNo = p.Store.PhoneNo,
                PostalCode = p.Store.PostalCode,
                PhoneNo = p.Store.PhoneNo,
                OrdersSellerViewChild = p.Store.Products.Where(u => u.UserId == p.Store.UserId && u.ProductId == p.ProductId).Select(y => new OrdersSellerViewChild()
                {

                    ProductName = y.ProductName,
                    ProductImage = y.ProductImage,
                    productVariationModels = y.ProductVariations.Where(v => v.P_Id == p.ProductId && v.Product.UserId == p.Store.UserId).Select(u => new ProductVariationModel()
                    {
                        OrderDetails = u.Orders.Where(d => d.VariationsId == p.VariationsId).Select(o => o.OrderDetails).ToList(),
                        P_VariationsID = u.P_VariationsID,
                        P_VariationsName = u.P_VariationsName,
                        ProductComments = p.ProductVariations.ProductComments.ToList(),
                    }).ToList(),

                }).ToList(),

            }).ToList();
            return Json(obj);
        }
        public IActionResult GetProductsbyProductName()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetProductName(string Id)
        {
                var products = _commerceDbContext.Products.Where(x => x.ProductName == Id).Select(y => new ProductSelectView()
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
                }).ToList(); ;
                return Json(products);   
        }
        [HttpGet]
        public IActionResult GetAllSearchResults(string Id)
        {
            if (Id == null)
            {
                List<Product> products1 = _commerceDbContext.Products.ToList();
                return Json(products1);
            }
            else
            {
                var products = _commerceDbContext.Products.Where(y => y.ProductName == Id).Select(x => new ProductSelectView
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    ProductPrice = x.ProductPrice,
                    wishList= x.WishList.ToList(),
                    ProductViewModels = new ProductViewModel()
                    {
                        productVariations = x.ProductVariations.Where(d => d.P_Id == x.ProductId).Select(z => new ProductVariations()
                        {
                            P_VariationsName = z.P_VariationsName,
                            P_VariationsID = z.P_VariationsID,
                            ProductBatches = z.ProductBatches,

                        }).ToList(),

                        ProductDescriptions = x.ProductDescriptions.Select(x => new ProductDescription()
                        {
                            //P = x.P,
                            PdescriptionId = x.PdescriptionId,
                            ProductBrand = x.ProductBrand,
                            ProductDes = x.ProductDes,
                            ProductMaterial = x.ProductMaterial,
                            ProductQuality = x.ProductQuality,
                            ProductQuantity = x.ProductQuantity,
                            ProductType = x.ProductType,
                        }).ToList(),
                      
                        ProductSpecifications = x.ProductSpecifications.Select(x => new ProductSpecification()
                        {
                            ProductSpecificationId = x.ProductSpecificationId,
                            ProcessorCapacity = x.ProcessorCapacity,
                            ProductDisplay = x.ProductDisplay,
                            ProductShippingInfo = x.ProductShippingInfo,
                            ProductSpecificationsText = x.ProductSpecificationsText,
                            ProductWarrantyInfo = x.ProductWarrantyInfo,
                            CameraQuality = x.CameraQuality,
                            Graphics = x.Graphics,
                            Memory = x.Memory,
                        }).ToList(),
                     
                    }
                }).ToList();
                return Json(products);
            }
        }
        public IActionResult AllReturnViews()
        {
            return View();
        }

        [Authorize]
        public IActionResult AllReturnOrders()
        {
            var userid = _userService.GetUserId();
            var dataof = _commerceDbContext.ReturnItems.Where(x => x.Products.UserId == userid).Select(y => new ReturnItems()
            {
                Ret_Quantity = y.Ret_Quantity,
                Order_Id = y.Order_Id,
                User = y.User,
                OrderItems_Id = y.OrderItems_Id,
                ProductImage = y.ProductImage,
                Ret_By = y.Ret_By,
                Ret_Reason = y.Ret_Reason,
                R_On = y.R_On,
                R_Number = y.R_Number,
                P_Id = y.P_Id,
            }).ToList();
            
            return Json(dataof);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllSearchResultsbyStoreId(string Id)
        {
            var userid = _userService.GetUserId();
            if (Id == null)
            {
                List<Product> products1 = _commerceDbContext.Products.Where(x=>x.UserId == userid).ToList();
                return Json(products1);
            }
            else
            {
                var products = _commerceDbContext.Products.Where(y => y.ProductName == Id && y.UserId == userid).Select(x => new ProductSelectView
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    ProductPrice = x.ProductPrice,
                    wishList = x.WishList.ToList(),
                    ProductViewModels = new ProductViewModel()
                    {
                        productVariations = x.ProductVariations.Where(d => d.P_Id == x.ProductId).Select(z => new ProductVariations()
                        {
                            P_VariationsName = z.P_VariationsName,
                            P_VariationsID = z.P_VariationsID,
                            ProductBatches = z.ProductBatches,

                        }).ToList(),

                        //Sizes = x.ProductSize.Select(x => x.Sizes).ToList(),
                        //Colours = x.ProductColours.Select(x => x.Colour).ToList(),
                        ProductDescriptions = x.ProductDescriptions.Select(x => new ProductDescription()
                        {
                            //P = x.P,
                            PdescriptionId = x.PdescriptionId,
                            ProductBrand = x.ProductBrand,
                            ProductDes = x.ProductDes,
                            ProductMaterial = x.ProductMaterial,
                            ProductQuality = x.ProductQuality,
                            ProductQuantity = x.ProductQuantity,
                            ProductType = x.ProductType,
                        }).ToList(),
                        //ProductDescriptionimages = x.ProductDescriptionimages.Select(x => new ProductDescriptionimage()
                        //{
                        //    ProductdesImage1 = x.ProductdesImage1,
                        //    ProductDescriptionImagesId = x.ProductDescriptionImagesId,
                        //}).ToList(),
                        ProductSpecifications = x.ProductSpecifications.Select(x => new ProductSpecification()
                        {
                            ProductSpecificationId = x.ProductSpecificationId,
                            ProcessorCapacity = x.ProcessorCapacity,
                            ProductDisplay = x.ProductDisplay,
                            ProductShippingInfo = x.ProductShippingInfo,
                            ProductSpecificationsText = x.ProductSpecificationsText,
                            ProductWarrantyInfo = x.ProductWarrantyInfo,
                            CameraQuality = x.CameraQuality,
                            Graphics = x.Graphics,
                            Memory = x.Memory,
                        }).ToList(),
                        //ProductSize = x.ProductSize.Select(x => new ProductSize()
                        //{
                        //    ProductSizeId = x.ProductSizeId,
                        //}).ToList(),
                        //ProductColours = x.ProductColours.Select(x => new ProductColours()
                        //{
                        //    ProductColourId = x.ProductColourId,
                        //}).ToList(),
                    }
                }).ToList();
                return Json(products);
            }
        }

        public IActionResult GetReviewandRating()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddReviewandRating(RatingModel ratingModel)
        {
            var userid = _userService.GetUserId();

            //var product = _commerceDbContext.Products.Where(x => x.ProductId == ratingModel.ProductId && x.UserId == userid && x.Orders.Any(y => y.OrderDetailsId == ratingModel.OrderDetailsId)).Select(z => z.ProductId).FirstOrDefault();
            int productCommentsId = _commerceDbContext.ProductComments.Where(x => x.ProductId == ratingModel.ProductId && x.UserId == userid && x.OrderDetailsId == ratingModel.OrderDetailsId && x.VariationId == ratingModel.VariationsId).Select(y => y.CommentId).FirstOrDefault();
            if (productCommentsId != 0)
            {
                ProductComments productComments = new ProductComments();
                productComments.CommentId = productCommentsId;
                productComments.VariationId = ratingModel.VariationsId;
                productComments.UserId = userid;
                productComments.CommentedOn = DateTime.Now;
                productComments.Rating = ratingModel.Rating;
                productComments.CommentDescription = ratingModel.CommentDescription;
                productComments.ProductId = ratingModel.ProductId;
                productComments.OrderDetailsId = ratingModel.OrderDetailsId;

                _commerceDbContext.ProductComments.Update(productComments);
                _commerceDbContext.SaveChanges();
            }
            else
            {
                ProductComments productComments = new ProductComments();
                productComments.UserId = userid;
                productComments.VariationId = ratingModel.VariationsId;
                productComments.CommentedOn = DateTime.Now;
                productComments.Rating = ratingModel.Rating;
                productComments.CommentDescription = ratingModel.CommentDescription;
                productComments.ProductId = ratingModel.ProductId;
                productComments.OrderDetailsId = ratingModel.OrderDetailsId;
                
                _commerceDbContext.ProductComments.Add(productComments);
                _commerceDbContext.SaveChanges();
            }
            return Ok();
        }
        public IActionResult GetReviewbyProductId(int id)
        {
            List<ProductComments> comments = _commerceDbContext.ProductComments.Where(x => x.ProductId == id).Select(y => new ProductComments()
            {
                CommentId = y.CommentId,
                Rating = y.Rating,
                CommentDescription = y.CommentDescription,
                User = y.User,
            }).ToList();

            return Json(comments);
        }
    }
}