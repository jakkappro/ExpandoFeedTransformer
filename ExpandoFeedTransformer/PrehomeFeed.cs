using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer
{
    public class PrehomeFeed
    {
        public static SHOP Deserialize(string source)
        {
            var serializer = new XmlSerializer(typeof(SHOP));
            using var reader = new StringReader(source);
            return (SHOP)serializer.Deserialize(reader);
        }

        public static string ParseShopToXML(SHOP shop)
        {
            var serializer = new XmlSerializer(typeof(SHOP));
            using var writer = new StringWriter();
            serializer.Serialize(writer, shop);
            return writer.ToString();
        }

        [SerializableAttribute()]
        [DesignerCategory("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        [XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class SHOP
        {

            private SHOPSHOPITEM[] sHOPITEMField;

            [XmlElementAttribute("SHOPITEM")]
            public SHOPSHOPITEM[] SHOPITEM
            {
                get => this.sHOPITEMField;
                set => this.sHOPITEMField = value;
            }
        }

        [SerializableAttribute()]
        [DesignerCategoryAttribute("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class SHOPSHOPITEM
        {

            private uint iTEM_IDField;

            private string pRODUCTNAMEField;

            private string pRODUCTField;

            private string dESCRIPTIONField;

            private string uRLField;

            private string iMGURLField;

            private decimal pRICEField;

            private decimal pRICE_VATField;

            private byte vATField;

            private string mANUFACTURERField;

            private string cATEGORYTEXTField;

            private string eANField;

            private uint pRODUCTNOField;

            private byte dELIVERY_DATEField;

            private SHOPSHOPITEMDELIVERY dELIVERYField;

            private decimal wEIGHTField;

            private string wEIGHT_UNITSField;

            private byte sTOCKField;

            public uint ITEM_ID
            {
                get => this.iTEM_IDField;
                set => this.iTEM_IDField = value;
            }

            public string PRODUCTNAME
            {
                get => this.pRODUCTNAMEField;
                set => this.pRODUCTNAMEField = value;
            }

            public string PRODUCT
            {
                get => this.pRODUCTField;
                set => this.pRODUCTField = value;
            }

            public string DESCRIPTION
            {
                get => this.dESCRIPTIONField;
                set => this.dESCRIPTIONField = value;
            }

            public string URL
            {
                get => this.uRLField;
                set => this.uRLField = value;
            }

            public string IMGURL
            {
                get => this.iMGURLField;
                set => this.iMGURLField = value;
            }

            public decimal PRICE
            {
                get => this.pRICEField;
                set => this.pRICEField = value;
            }

            public decimal PRICE_VAT
            {
                get => this.pRICE_VATField;
                set => this.pRICE_VATField = value;
            }

            public byte VAT
            {
                get => this.vATField;
                set => this.vATField = value;
            }

            public string MANUFACTURER
            {
                get => this.mANUFACTURERField;
                set => this.mANUFACTURERField = value;
            }

            public string CATEGORYTEXT
            {
                get => this.cATEGORYTEXTField;
                set => this.cATEGORYTEXTField = value;
            }

            public string EAN
            {
                get => this.eANField;
                set => this.eANField = value;
            }

            public uint PRODUCTNO
            {
                get => this.pRODUCTNOField;
                set => this.pRODUCTNOField = value;
            }

            public byte DELIVERY_DATE
            {
                get => this.dELIVERY_DATEField;
                set => this.dELIVERY_DATEField = value;
            }

            public SHOPSHOPITEMDELIVERY DELIVERY
            {
                get => this.dELIVERYField;
                set => this.dELIVERYField = value;
            }

            public decimal WEIGHT
            {
                get => this.wEIGHTField;
                set => this.wEIGHTField = value;
            }

            public string WEIGHT_UNITS
            {
                get => this.wEIGHT_UNITSField;
                set => this.wEIGHT_UNITSField = value;
            }

            public byte STOCK
            {
                get => this.sTOCKField;
                set => this.sTOCKField = value;
            }
        }

        [SerializableAttribute()]
        [DesignerCategoryAttribute("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class SHOPSHOPITEMDELIVERY
        {

            private string dELIVERY_IDField;

            private decimal dELIVERY_PRICEField;

            private decimal dELIVERY_PRICE_CODField;

            public string DELIVERY_ID
            {
                get => this.dELIVERY_IDField;
                set => this.dELIVERY_IDField = value;
            }

            public decimal DELIVERY_PRICE
            {
                get => this.dELIVERY_PRICEField;
                set => this.dELIVERY_PRICEField = value;
            }

            public decimal DELIVERY_PRICE_COD
            {
                get => this.dELIVERY_PRICE_CODField;
                set => this.dELIVERY_PRICE_CODField = value;
            }
        }


    }
}