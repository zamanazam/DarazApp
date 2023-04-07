using DarazApp.Entities;
using DarazApp.Helpers;
using DarazApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Data;
using System.Linq;
using static DarazApp.Models.JoinandViewModle;
using static DarazApp.Services.Program;

namespace DarazApp.Controllers
{
    public class TableController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CommerceDbContext _commerceDbContext = new CommerceDbContext();
        private IUserService _userService;
        public TableController(CommerceDbContext commerceDb, IUserService userService, IWebHostEnvironment hostEnvironment)
        {
            _hostingEnvironment = hostEnvironment;
            _commerceDbContext = commerceDb;
            _userService = userService;
        }

        public IActionResult TestingView()
        {
            return View();
        }
        public List<Product> products = new List<Product>();
        public List<Category> categories = new List<Category>();
        public List<PerCategory> perCategories = new List<PerCategory>();
        public List<KidCategory> kidCategories = new List<KidCategory>();
        public List<User> users = new List<User>();
        public List<Sizes> sizes = new List<Sizes>();
        public List<ProductSpecification> specifications = new List<ProductSpecification>();
        public List<ProductDescriptionimage> descriptionimages = new List<ProductDescriptionimage>();
        public List<ProductDescription> descriptions = new List<ProductDescription>();
        public List<ViewModel> views = new List<ViewModel>();
        public List<ProductColours> productcolours = new List<ProductColours>();
        public List<ProductSize> productsizes = new List<ProductSize>();
        public List<Colour> colours = new List<Colour>();
        //[Authorize]
        public IActionResult Product()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public List<Product> ProdcutsTable()
        {
            User user = new User();
            using (var db = new CommerceDbContext())
            {
                var userd = _userService.GetUserId();

                if (userd == 0)
                {
                    return null;
                }
                else
                {
                    var std = (from p in _commerceDbContext.Products
                               join k in _commerceDbContext.KidCategories on
                               p.KidCategoryId equals k.KidCategoryId into table1
                               from k in table1.DefaultIfEmpty()
                               join u in _commerceDbContext.Users on
                               p.UserId equals u.UserId into table2
                               from u in table2.DefaultIfEmpty()
                               join pd in _commerceDbContext.ProductDescriptions on
                               p.ProductId equals pd.ProductId into table3
                               from pd in table3.DefaultIfEmpty()
                               select new Product
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   ProductPrice = p.ProductPrice,
                                   UploadDate=p.UploadDate,
                                   KidCategoryId = p.KidCategoryId,
                                   KidCategory = k,
                                   UserId = p.UserId,
                                   User = u,
                                   ProductDescriptions = p.ProductDescriptions,
                               }).ToList();
                    return std.Where(x=>x.UserId==userd).ToList();
                }
            }
        }
        public IActionResult AllUserProductsAdminView()
        {
            return View();
        }
        public IActionResult AllUsersProducts()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public List<Product> AllProductsUSer()
        {
            User user = new User();
            using (var db = new CommerceDbContext())
            {
                var userd = _userService.GetUserId();
                int RolesIds = _commerceDbContext.UserRoles.Where(x =>x.UserId == userd).Select(x=>x.RoleId).FirstOrDefault();
                   
                if (RolesIds == ((int)RolesEnum.Store))
                {
                    var std = (from p in _commerceDbContext.Products
                               join k in _commerceDbContext.KidCategories on
                               p.KidCategoryId equals k.KidCategoryId into table1
                               from k in table1.DefaultIfEmpty()
                               join u in _commerceDbContext.Users on
                               p.UserId equals u.UserId into table2
                               from u in table2.DefaultIfEmpty()
                               join pd in _commerceDbContext.ProductDescriptions on
                               p.ProductId equals pd.ProductId into table3
                               from pd in table3.DefaultIfEmpty()
                               select new Product
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   ProductPrice = p.ProductPrice,
                                   UploadDate = p.UploadDate,
                                   KidCategoryId = p.KidCategoryId,
                                   KidCategory = k,
                                   UserId = p.UserId,
                                   User = u,
                               
                                   ProductDescriptions = p.ProductDescriptions,
                               }).Where(p=>p.UserId == userd).OrderByDescending(p=>p.UploadDate).ToList();
                    return std;
                }
                else
                {
                    var std = (from p in _commerceDbContext.Products
                               join k in _commerceDbContext.KidCategories on
                               p.KidCategoryId equals k.KidCategoryId into table1
                               from k in table1.DefaultIfEmpty()
                               join u in _commerceDbContext.Users on
                               p.UserId equals u.UserId into table2
                               from u in table2.DefaultIfEmpty()
                               join pd in _commerceDbContext.ProductDescriptions on
                               p.ProductId equals pd.ProductId into table3
                               from pd in table3.DefaultIfEmpty()
                               select new Product
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   ProductImage = p.ProductImage,
                                   ProductPrice = p.ProductPrice,
                                   UploadDate = p.UploadDate,
                                   KidCategoryId = p.KidCategoryId,
                                   KidCategory = k,
                                   UserId = p.UserId,
                                   User = u,
                                   ProductDescriptions = p.ProductDescriptions,
                               }).ToList();
                    return std;
                }
            }
        }

        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewCategory(string CategoryName, IFormFile file)
        {
            Category category = new Category();
            category.CategoryName = CategoryName;
            string filepath = Path.GetFileName(file.FileName);
            string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
            string currentDirectory = _hostingEnvironment.WebRootPath;
            EnsureFolder(Path.Combine(currentDirectory, "\\images" ?? "Common"));
            using FileStream stream = new FileStream(currentDirectory + imagespaths, FileMode.Create);
            {
                file.CopyTo(stream);
                stream.Dispose();
            }
            category.CategoryIcon = imagespaths;
            _commerceDbContext.Categories.Add(category);
            _commerceDbContext.SaveChanges();
            var LatestCategoryId = category.CategoryId;
            return Json(LatestCategoryId);
        }
        private void EnsureFolder(string path)
        {
            var exists = Directory.Exists(path);
            if (!exists)
                Directory.CreateDirectory(path);
        }
        [HttpPost]
        public IActionResult AddNewSubCategory(IList<string>SubCategoryName ,IList<IFormFile>formFiles, int CategoryId)
        {
            int val = 0;
            foreach (var item in formFiles)
            {
                int val1 = val++;
                string filepath = Path.GetFileName(item.FileName);
                string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                string currentDirectory = _hostingEnvironment.WebRootPath;
                EnsureFolder(Path.Combine(currentDirectory, "\\images" ?? "Common"));
                using FileStream stream = new FileStream(currentDirectory + imagespaths, FileMode.Create);
                {
                    item.CopyTo(stream);
                    stream.Dispose();
                }
                PerCategory perCategory = new PerCategory();
                perCategory.PerCategoryName = SubCategoryName.ElementAtOrDefault(val1);
                perCategory.PerCategoriesIcon = imagespaths;
                perCategory.CategoryId = CategoryId;
                _commerceDbContext.PerCategories.Add(perCategory);
                _commerceDbContext.SaveChanges();
            }
            var Catid = _commerceDbContext.PerCategories.Where(x => x.CategoryId == CategoryId).ToList();
            return Json(Catid);
        }
        [HttpPost]
        public IActionResult AddNewKidCategory(IList<string> KidCategoryName, IList<IFormFile> formkidFiles, int PerCategoryId , IList<double> KidCategoryComission)
        {
            var latedkidid = 0; 
            int val = 0;
            foreach (var item in formkidFiles)
            {
                int val1 = val++;
                KidCategory kid = new KidCategory();
                string filepath = Path.GetFileName(item.FileName);
                string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                string currentDirectory = _hostingEnvironment.WebRootPath;
                EnsureFolder(Path.Combine(currentDirectory, "\\images" ?? "Common"));
                using FileStream stream = new FileStream(currentDirectory + imagespaths, FileMode.Create);
                {
                    item.CopyTo(stream);
                    stream.Dispose();
                }
                kid.KidCategoryName = KidCategoryName.ElementAtOrDefault(val1);
                kid.Comission = KidCategoryComission.ElementAtOrDefault(val1);
                kid.KidCategoryIcon = imagespaths;
                kid.PerCategoryId = PerCategoryId;
                _commerceDbContext.KidCategories.Add(kid);
                _commerceDbContext.SaveChanges();
                latedkidid = kid.KidCategoryId;  
            }
            return Json(latedkidid);
        }
        [Authorize]
        [HttpGet]
        public List<KidCategory> AllKidCategories()
        {
            kidCategories = _commerceDbContext.KidCategories.ToList();
            return kidCategories;
        }
        [Authorize]
        public IActionResult ProductsDesc()
        {
            return View();
        }

        public List<ViewModel> models = new List<ViewModel>();
        //[Authorize]
        //[HttpGet]
        //public IQueryable<ProductSize> ProductDetails()
        //{
        //    ProductDescription description = new ProductDescription();
        //    SaveData save = new SaveData();
        //    ViewModel viewModel = new ViewModel();
        //    ProductSize productSize = new ProductSize();
        //    ProductDescriptionimage descriptionimage = new ProductDescriptionimage();
        //    ProductSpecification specification = new ProductSpecification();
        //    var data = _commerceDbContext.ProductSize.AsQueryable().Include(Convert.ToString(products));
        //    return data;
        //}

        [Authorize]
        [HttpGet]
        public List<ProductDescriptionimage> ProductDescImage()
        {
            var prodesima = (from pdim in _commerceDbContext.ProductDescriptionimages
                             join p in _commerceDbContext.Products on
                             pdim.ProductId equals p.ProductId into desim1
                             from p in desim1.DefaultIfEmpty()
                             select new ProductDescriptionimage
                             {
                                 ProductId = pdim.ProductId,
                                 Product = p,
                             }).ToList();
            return prodesima;
        }
        [Authorize]
        [HttpGet]
        public List<ProductSpecification> ProductSpecific()
        {
            var prdspec = (from pspe in _commerceDbContext.ProductSpecifications
                           join p in _commerceDbContext.Products on
                           pspe.ProductId equals p.ProductId into pros1
                           from p in pros1.DefaultIfEmpty()
                           select new ProductSpecification
                           {
                               ProductId = pspe.ProductId,
                               Product = p,
                           }).ToList();
            return prdspec;
        }
        [Authorize]
        public List<Sizes> ProdSize(int id)
        {
            var sizesids = _commerceDbContext.ProductSize.Where(x => x.ProductId == id).Select(x => x.Sizeid).ToList();
           var sizis  = _commerceDbContext.Sizes.Where(x => sizesids.Contains(x.Sizeid)).ToList();

            return sizis;
        }
        [Authorize]
        public List<Colour> prodcolor(int id)
        {
            var Coloursids = _commerceDbContext.ProductColours.Where(x => x.ProductId== id).Select(x => x.ColourId).ToList();
            var colis = _commerceDbContext.Colour.Where(x => Coloursids.Contains(x.ColourId)).ToList();
            return colis;
        }
        [Authorize]
        public List<ProductDescriptionimage> ProdDesImages(int id)
        {
            var desimages = _commerceDbContext.ProductDescriptionimages.Where(x => x.ProductId == id).ToList();
            return desimages;
        }
        public List<ProductDescription> ProDescription(int id)
        {
            var descriptions = _commerceDbContext.ProductDescriptions.Where(x => x.ProductId == id).ToList();
            return descriptions;
        }
        [Authorize]
        [HttpGet]
        public List<KidCategory> GetKidCategories()
        {
            var descriptions = _commerceDbContext.KidCategories.ToList();
            return descriptions;
        }
        [Authorize]
        public List<Product> Prod(int id)
        {
            var descriptions = _commerceDbContext.Products.Where(x => x.ProductId == id).ToList();
            return descriptions;
        }
        public IActionResult ProductbyIdSellerView()
        {
            return View();
        }
  
        public IActionResult SimmilarItems(int id)
        {
            Product produ = new Product();
            ViewModel view = new ViewModel();
            var similar = _commerceDbContext.Products.Where(x => x.ProductId == id).Select(x=>x.KidCategoryId).FirstOrDefault();
            var items = _commerceDbContext.Products.Where(a => a.KidCategoryId == similar && a.ProductId != id).Select(p=> new ProductSelectView()
            {
               ProductId = p.ProductId,
               ProductName = p.ProductName,
               ProductImage = p.ProductImage,
               ProductViewModels = new ProductViewModel()
                {
                    productVariations = p.ProductVariations.Where(y => y.P_Id == p.ProductId).Select(z => new ProductVariations()
                    {
                        P_VariationsName = z.P_VariationsName,
                        P_VariationsID = z.P_VariationsID,
                        ProductBatches = z.ProductBatches,

                    }).ToList(),
                    ProductDescriptions = p.ProductDescriptions.Select(x => new ProductDescription()
                    {
                        PdescriptionId = x.PdescriptionId,
                        ProductBrand = x.ProductBrand,
                        ProductDes = x.ProductDes,
                        ProductMaterial = x.ProductMaterial,
                        ProductQuality = x.ProductQuality,
                        ProductQuantity = x.ProductQuantity,
                        ProductType = x.ProductType,
                    }).ToList(),
                    ProductDescriptionimages = p.ProductDescriptionimages.Select(x => new ProductDescriptionimage()
                    {
                        ProductdesImage1 = x.ProductdesImage1,
                        ProductDescriptionImagesId = x.ProductDescriptionImagesId,
                    }).ToList(),
                    ProductSpecifications = p.ProductSpecifications.Select(x => new ProductSpecification()
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
            return Json (items);
        }
        public List<Sizes> newsizes = new List<Sizes>();
        public List<ViewModel> viewModel = new List<ViewModel>();

        [HttpGet]
        public JsonResult ProductDetailsbyId(int Id)
        {
        
           var data = _commerceDbContext.Products.Where(x => x.ProductId == Id)
                .Select(x => new ProductSelectView
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    UploadDate = x.UploadDate,
                    ProductImage = x.ProductImage,
                    ProductPrice = x.ProductPrice,
                    SellerName = x.User.UserName,
                    SellerImage = x.User.UserImage,
                    SellerId = x.User.UserId,
                    ProductComments = x.ProductComments.Select(q => new ProductComments()
                    {
                        CommentDescription = q.CommentDescription,
                        Rating = q.Rating,

                    }).ToList(),

                    ProductViewModels = new ProductViewModel()
                    {
                        productVariations = x.ProductVariations.Select(z => new ProductVariations()
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

                    }
                }).FirstOrDefault();
            return Json(data);
        }
        

        [HttpPost]
        public IActionResult GetAllColorsandSizes(GetColoursandSizes coloursandSizes)
        {


            List<ProductBatch> productBatches = _commerceDbContext.ProductBatch.Where(x => x.P_VariationsId == coloursandSizes.VariationId)
                .Select(x => new ProductBatch()
                {
                    Price=x.Price,
                    Quantity = x.Quantity,
                    RemainingQuantity=x.RemainingQuantity,
                    BatchId =x.BatchId,
                }).ToList();
            return Json(productBatches);

        }
        //IQueryable<ProductBatch> batches = _commerceDbContext.ProductBatch.AsQueryable<ProductBatch>();
        //          if (coloursandSizes.ProductId != 0)
        //          {
        //              batches = batches.Where(x => x.P_VariationsId == coloursandSizes.ProductId);
        //          }

        //if (coloursandSizes.SizeId != null )
        //{
        //    batches = batches.Where(x => coloursandSizes.SizeId.Contains(x.SizeId) && x.P_Id == coloursandSizes.ProductId);
        //}
        //if (coloursandSizes.ColourId != null)
        //{
        //    batches = batches.Where(x => coloursandSizes.ColourId.Contains(x.ColourId) && x.P_Id == coloursandSizes.ProductId);
        //}
        //var newbatch = batches.Select(b => new BatchViewModel()
        //{
        //    ProductId = b.P_VariationsId,
        //    //ColourId = b.Colour.ColourId,
        //    //ColourName = b.Colour.ColourName,
        //    Price =b.Price,
        //    Quantity = b.Quantity,
        //    //SizeName = b.Sizes.SizeName,
        //    //SizeId = b.Sizes.Sizeid,
        //}).ToList();


        //else
        //{
        //    IQueryable<ProductBatch> batches = _commerceDbContext.ProductBatch.AsQueryable<ProductBatch>();

        //    if (coloursandSizes.ProductId != 0)
        //    {
        //        batches = batches.Where(x => x.P_Id == coloursandSizes.ProductId);
        //    }

        //    if (coloursandSizes.ColourId != null)
        //    {
        //        batches = batches.Where(x => coloursandSizes.ColourId.Contains(x.ColourId));
        //    }
        //    var newbatch = batches.Select(b => new BatchViewModel()
        //    {
        //        ProductId = b.P_Id,
        //        ColourName = b.Colour.ColourName,
        //        SizeName = b.Sizes.SizeName,
        //    }).Distinct().ToList();
        //    return Json(newbatch);
        //}

        //var data = _commerceDbContext.Products.Where(x => x.ProductId == Id).ToList();
        //var prddata = _commerceDbContext.ProductDescriptions.Where(x => x.ProductId == Id).ToList();
        //var proddescripimages = _commerceDbContext.ProductDescriptionimages.Where(x => x.ProductId == Id).ToList();
        //var siz211 =_commerceDbContext.ProductSize.Where(x => x.ProductId == Id).Select(x=>x.Sizeid).ToList();
        //var sizenew = _commerceDbContext.Sizes.Where(x => siz211.Contains(x.Sizeid)).ToList();
        //view.Sizesvm = sizenew;
        //var colsids = _commerceDbContext.ProductColours.Where(x => x.ProductId == Id).Select(x=>x.ColourId).ToList();
        //var productcols = _commerceDbContext.Colour.Where(x =>colsids.Contains(x.ColourId)).ToList();
        //view.Coloursvm = productcols;
        //view.ProductVm = data;
        //view.ProductDescriptionVm= prddata;
        //view.productDescriptionimages = proddescripimages;


        public List<Product> produ = new List<Product>();

        public object Session { get; private set; }

        //public IActionResult DeleteTableRow(int id)
        //{
        //    ViewModel view = new ViewModel();
        //    Products pdos = new Products();
        //    ProductDescriptionimage prodesimage = new ProductDescriptionimage();
        //    ProductDescription justdescription = new ProductDescription();
        //    ProductColours productColours = new ProductColours();
        //    ProductSize productSize = new ProductSize();
        //    ProductSpecification productSpecification = new ProductSpecification();
        //    using (var db = new CommerceDbContext())
        //    {
        //        descriptionimages = db.ProductDescriptionimages.Where(x => x.ProductId == id).ToList();
        //        db.ProductDescriptionimages.RemoveRange(descriptionimages);
        //        db.SaveChanges();

        //        descriptions = db.ProductDescriptions.Where(x => x.ProductId == id).ToList();
        //        db.ProductDescriptions.RemoveRange(descriptions);
        //        db.SaveChanges();


        //        productsizes = db.ProductSize.Where(x => x.ProductId == id).ToList();
        //        db.ProductSize.RemoveRange(productsizes);
        //        db.SaveChanges();

        //        productcolours = db.ProductColours.Where(x => x.ProductId == id).ToList();
        //        db.ProductColours.RemoveRange(productcolours);
        //        db.SaveChanges();

        //        specifications = db.ProductSpecifications.Where(x => x.ProductId == id).ToList();
        //        db.ProductSpecifications.RemoveRange(specifications);
        //        db.SaveChanges();

        //        produ = db.Products.Where(x => x.ProductId == id).ToList();
        //        db.Products.RemoveRange(produ);
        //        db.SaveChanges();

        //    }
        //    return View();
        //}

        public IActionResult DeleteTableRow(int id)
        {
            ViewModel view = new ViewModel();
            Products pdos = new Products();
            ProductDescriptionimage prodesimage = new ProductDescriptionimage();
            ProductDescription justdescription = new ProductDescription();
            ProductColours productColours = new ProductColours();
            ProductSize productSize = new ProductSize();
            ProductVariations productVariations = new ProductVariations();
            ProductBatch productBatch = new ProductBatch();
            ProductSpecification productSpecification = new ProductSpecification();
            using (var db = new CommerceDbContext())
            {
                descriptionimages = db.ProductDescriptionimages.Where(x => x.ProductId == id).ToList();
                db.ProductDescriptionimages.RemoveRange(descriptionimages);
                db.SaveChanges();

                descriptions = db.ProductDescriptions.Where(x => x.ProductId == id).ToList();
                db.ProductDescriptions.RemoveRange(descriptions);
                db.SaveChanges();


                productsizes = db.ProductSize.Where(x => x.ProductId == id).ToList();
                db.ProductSize.RemoveRange(productsizes);
                db.SaveChanges();

                productcolours = db.ProductColours.Where(x => x.ProductId == id).ToList();
                db.ProductColours.RemoveRange(productcolours);
                db.SaveChanges();

                specifications = db.ProductSpecifications.Where(x => x.ProductId == id).ToList();
                db.ProductSpecifications.RemoveRange(specifications);
                db.SaveChanges();

                var prdvat = db.ProductVariations.Where(x => x.P_Id == id).Select(x=>x.P_VariationsID).ToList();
                var probatch = db.ProductBatch.Where(y => prdvat.Contains(y.P_VariationsId)).ToList();
                db.ProductBatch.RemoveRange(probatch);
                db.SaveChanges();
                db.ProductVariations.RemoveRange(productVariations);
                db.SaveChanges();

                produ = db.Products.Where(x => x.ProductId == id).ToList();
                db.Products.RemoveRange(produ);
                db.SaveChanges();

            }
            return View();
        }
        [HttpDelete]
        public IActionResult DeleteTableRowAdminView(int id)
        {
            ViewModel view = new ViewModel();
            Products pdos = new Products();
            ProductDescriptionimage prodesimage = new ProductDescriptionimage();
            ProductDescription justdescription = new ProductDescription();
            ProductColours productColours = new ProductColours();
            ProductSize productSize = new ProductSize();
            List<ProductVariations> productVariations = new List<ProductVariations>();
            ProductBatch productBatch = new ProductBatch();
            List<MoreSepcifications> moreSepcifications = new List<MoreSepcifications>();
            ProductSpecification productSpecification = new ProductSpecification();
            using (var db = new CommerceDbContext())
            {
                descriptionimages = db.ProductDescriptionimages.Where(x => x.ProductId == id).ToList();
                db.ProductDescriptionimages.RemoveRange(descriptionimages);
                db.SaveChanges();

                descriptions = db.ProductDescriptions.Where(x => x.ProductId == id).ToList();
                db.ProductDescriptions.RemoveRange(descriptions);
                db.SaveChanges();


                productsizes = db.ProductSize.Where(x => x.ProductId == id).ToList();
                db.ProductSize.RemoveRange(productsizes);
                db.SaveChanges();

                productcolours = db.ProductColours.Where(x => x.ProductId == id).ToList();
                db.ProductColours.RemoveRange(productcolours);
                db.SaveChanges();

                specifications = db.ProductSpecifications.Where(x => x.ProductId == id).ToList();
                db.ProductSpecifications.RemoveRange(specifications);
                db.SaveChanges();

                moreSepcifications = db.MoreSepcifications.Where(x => x.ProductId == id).ToList();
                db.MoreSepcifications.RemoveRange(moreSepcifications);
                db.SaveChanges();

                var prdvat = db.ProductVariations.Where(x => x.P_Id == id).Select(x => x.P_VariationsID).ToList();
                var probatch = db.ProductBatch.Where(y => prdvat.Contains(y.P_VariationsId)).ToList();
                db.ProductBatch.RemoveRange(probatch);
                db.SaveChanges();

                productVariations = db.ProductVariations.Where(y => y.P_Id == id).ToList();
                db.ProductVariations.RemoveRange(productVariations);
                db.SaveChanges();

                produ = db.Products.Where(x => x.ProductId == id).ToList();
                db.Products.RemoveRange(produ);
                db.SaveChanges();

            }
            return View();
        }
        [HttpGet]
        public IActionResult EditTable()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditTableAdminView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditTabledata(SaveData saveData)
        {
            
            ViewModel view = new ViewModel();
            Product pdos = new Product();
            List<ProductDescriptionimage> prodesimage = new List<ProductDescriptionimage>();
            ProductDescription justdescription = new ProductDescription();
            ProductColours productColours = new ProductColours();
            Sizes productSize = new Sizes();
            using (var db = new CommerceDbContext())
            {

                products = db.Products.Where(x => x.ProductId == saveData.ProductId).ToList();
                foreach (var item in products)
                {
                    item.ProductId = saveData.ProductId;
                    item.ProductName = saveData.ProductName;
                    if (saveData.file == null)
                    {
                        item.ProductImage = item.ProductImage;
                    }
                    else
                    {
                        string proimage = Path.GetFileName(saveData.file.FileName);
                        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "\\images", proimage);
                        string currentDirectory = _hostingEnvironment.WebRootPath;
                        EnsureFolder(Path.Combine(currentDirectory, "\\images" ?? "Common"));
                        using FileStream stream = new FileStream(currentDirectory + uploadfilepath, FileMode.Create);
                        {
                            saveData.file.CopyTo(stream);
                            stream.Dispose();
                        }
                        item.ProductImage = uploadfilepath;
                    }
                    db.Products.Update(item);
                    db.SaveChanges();
                }

                    descriptions = db.ProductDescriptions.Where(y => y.ProductId == saveData.ProductId).ToList();
                    foreach (var descrip in descriptions)
                    {
                        //descrip.PdescriptionId = saveData.PdescriptionId;
                        descrip.ProductBrand = saveData.ProductBrand;
                        descrip.ProductMaterial = saveData.ProductMaterial;
                        descrip.ProductQuantity = saveData.ProductQuantity;
                        descrip.ProductQuality = saveData.ProductQuality;
                        descrip.ProductType = saveData.ProductType;
                        descrip.ProductDes = saveData.ProductDes;
                        descrip.ProductId = saveData.ProductId;
                        db.ProductDescriptions.Update(descrip);
                        db.SaveChanges();
                }
                    

                if (saveData.savedimages != null)
                {
                    prodesimage = db.ProductDescriptionimages.Where(x => x.ProductId == saveData.ProductId).ToList();
                    db.ProductDescriptionimages.RemoveRange(prodesimage);
                    db.SaveChanges();
                    foreach (var images in saveData.savedimages)
                    {
                        ProductDescriptionimage productDescriptionimage = new ProductDescriptionimage();

                        string filepath = Path.GetFileName(images.FileName);
                        string imagespaths = Path.Combine(Directory.GetCurrentDirectory(), "\\images", filepath);
                        string currentDirectory = _hostingEnvironment.WebRootPath;
                        EnsureFolder(Path.Combine(currentDirectory, "\\images" ?? "Common"));
                        using FileStream stream = new FileStream(currentDirectory + imagespaths, FileMode.Create);
                        {
                            saveData.file.CopyTo(stream);
                            stream.Dispose();
                        }
                        productDescriptionimage.ProductdesImage1 = imagespaths;
                        productDescriptionimage.ProductId = saveData.ProductId;
                        db.ProductDescriptionimages.AddRange(productDescriptionimage);
                        db.SaveChanges();
                    }
                }

                specifications = db.ProductSpecifications.Where(x => x.ProductId == saveData.ProductId).ToList();
                foreach (var spec in specifications)
                {
                    spec.ProductDisplay = saveData.ProductDisplay;
                    spec.ProductWarrantyInfo = saveData.ProductWarrantyInfo;
                    spec.ProductShippingInfo = saveData.ProductShippingInfo;
                    spec.ProductSpecificationsText = saveData.ProductSpecificationsText;
                    spec.CameraQuality = saveData.CameraQuality;
                    spec.Memory = saveData.Memory;
                    spec.Graphics = saveData.Graphics;
                    spec.ProcessorCapacity = saveData.ProcessorCapacity;
                    db.ProductSpecifications.Update(spec);
                    db.SaveChanges();
                }

                var val123 = 0;
                foreach (var b_Id in saveData.BatchIds)
                {
                    var val1 = val123++;
                    int price = saveData.ProdPrice.ElementAtOrDefault(val1);
                    ProductBatch batch = _commerceDbContext.ProductBatch.Where(x => x.BatchId == b_Id).FirstOrDefault();
                    List<ProductBatch> batchlist = _commerceDbContext.ProductBatch.Where(y => y.BatchId == b_Id).ToList();
                    foreach (var item in batchlist)
                    {
                        item.Quantity = batch.Quantity;
                        item.UploadOn = batch.UploadOn;
                        item.Price = price;
                        item.SaleOn = batch.SaleOn;
                        item.RemainingQuantity = batch.RemainingQuantity;
                        _commerceDbContext.ProductBatch.Update(item);
                        _commerceDbContext.SaveChanges();
                    }
                }
            }
                    return Json(saveData.ProductId);
        }
        [HttpGet]
        public List<Colour> ProdColours()
        {
            var colou = _commerceDbContext.Colour.ToList();
            return colou;
        }
        [HttpGet]
        public List<Sizes> ProdSizes()
        {
            var sizu = _commerceDbContext.Sizes.ToList();
            return sizu;
        }

        
    }
}
