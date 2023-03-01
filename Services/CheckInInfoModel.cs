using DarazApp.Entities;

namespace DarazApp.Services
{
    public class CheckInInfoModel
    {
        public int PdescriptionId { get; set; }

        public string? ProductType { get; set; }

        public string? ProductQuality { get; set; }

        public string? ProductColor { get; set; }

        public string? ProductQuantity { get; set; }

        public string? ProductDes { get; set; }

        public int P { get; set; }

        public string? ProductImage { get; set; }
        public string? ProductName { get; set; }
        public string? ProductBrand { get; set; }

        public string? ProductMaterial { get; set; }
        public User User { get; set; }
       
        public ProductColours ProductColour { get; set; }
        public ProductSize ProductSize { get; set; }
        public Colour Colour { get; set; }
        public Sizes Sizes { get; set; }
        public ProductDescription ProductDescription { get; set; }
        public ProductDescriptionimage ProductDescriptionimage { get; set; }
        public ProductSpecification ProductSpecification { get; set; }  
        public QuestionReply QuestionReply { get; set; }
        public QuestionsApp QuestionsApp { get; set; }
        public AccountDetail AccountDetail { get; set; }
        public UserAccount UserAccount { get; set; }
        public CardData CardData { get; set; }
        //public List<ProductDescriptionimage> ProductDescriptionimages { get; set; } = new List<ProductDescriptionimage>();

        //public List<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();

        //public List<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();

        //public List<SaleInvoice> SaleInvoices { get; set; } = new List<SaleInvoice>();
        //public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        //public List<Sizes> Size { get; set; } = new List<Sizes>();
        //public List<Colour> Colours { get; set; } = new List<Colour>();
        //public List<ProductColours> ProductColours { get; set; } = new List<ProductColours>();
        //public List<CardData> CardDatas { get; set; } = new List<CardData>();

    }
}
