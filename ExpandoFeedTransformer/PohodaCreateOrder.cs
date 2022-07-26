using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer
{
    public class PohodaCreateOrder
    {
        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/data.xsd")]
        [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/data.xsd",
            IsNullable = false)]
        public class dataPack
        {
            private dataPackDataPackItem dataPackItemField;

            private string idField;

            private uint icoField;

            private string applicationField;

            private decimal versionField;

            private string noteField;

            public dataPackDataPackItem dataPackItem
            {
                get => dataPackItemField;
                set => dataPackItemField = value;
            }

            [XmlAttribute]
            public string id
            {
                get => idField;
                set => idField = value;
            }

            [XmlAttribute]
            public uint ico
            {
                get => icoField;
                set => icoField = value;
            }

            [XmlAttribute]
            public string application
            {
                get => applicationField;
                set => applicationField = value;
            }

            [XmlAttribute]
            public decimal version
            {
                get => versionField;
                set => versionField = value;
            }

            [XmlAttribute]
            public string note
            {
                get => noteField;
                set => noteField = value;
            }
            public static string Serialize(dataPack data)
            {
                var x = new XmlSerializer(data.GetType());
                
                TextWriter writer = new Utf8StringWriter();
                x.Serialize(writer, data);
                var s =  writer.ToString() ?? throw new InvalidOperationException();
                writer.Flush();
                writer.Close();
                return s;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/data.xsd")]
        public class dataPackDataPackItem
        {
            private order orderField;

            private string idField;

            private decimal versionField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/order.xsd")]
            public order order
            {
                get => orderField;
                set => orderField = value;
            }

            [XmlAttribute]
            public string id
            {
                get => idField;
                set => idField = value;
            }

            [XmlAttribute]
            public decimal version
            {
                get => versionField;
                set => versionField = value;
            }

        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd",
            IsNullable = false)]
        public class order
        {
            private orderOrderHeader orderHeaderField;

            private orderOrderItem[] orderDetailField;

            private orderOrderSummary orderSummaryField;

            private decimal versionField;

            public orderOrderHeader orderHeader
            {
                get => orderHeaderField;
                set => orderHeaderField = value;
            }

            [XmlArrayItem("orderItem", IsNullable = false)]
            public orderOrderItem[] orderDetail
            {
                get => orderDetailField;
                set => orderDetailField = value;
            }

            public orderOrderSummary orderSummary
            {
                get => orderSummaryField;
                set => orderSummaryField = value;
            }

            [XmlAttribute]
            public decimal version
            {
                get => versionField;
                set => versionField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeader
        {
            private string orderTypeField;

            private string numberOrderField;

            private DateTime dateField;

            private DateTime dateFromField;

            private DateTime dateToField;

            private string textField;

            private orderOrderHeaderNumber numberField;

            private orderOrderHeaderPartnerIdentity partnerIdentityField;

            private orderOrderHeaderPaymentType paymentTypeField;

            private orderOrderHeaderPriceLevel priceLevelField;

            private orderOrderHeaderCarrier carrierField;

            public string orderType
            {
                get => orderTypeField;
                set => orderTypeField = value;
            }

            public string numberOrder
            {
                get => numberOrderField;
                set => numberOrderField = value;
            }

            [XmlElement(DataType = "date")]
            public DateTime date
            {
                get => dateField;
                set => dateField = value;
            }

            [XmlElement(DataType = "date")]
            public DateTime dateFrom
            {
                get => dateFromField;
                set => dateFromField = value;
            }

            [XmlElement(DataType = "date")]
            public DateTime dateTo
            {
                get => dateToField;
                set => dateToField = value;
            }

            public string text
            {
                get => textField;
                set => textField = value;
            }

            public orderOrderHeaderNumber number
            {
                get => numberField;
                set => numberField = value;
            }

            public orderOrderHeaderPartnerIdentity partnerIdentity
            {
                get => partnerIdentityField;
                set => partnerIdentityField = value;
            }

            public orderOrderHeaderPaymentType paymentType
            {
                get => paymentTypeField;
                set => paymentTypeField = value;
            }

            public orderOrderHeaderPriceLevel priceLevel
            {
                get => priceLevelField;
                set => priceLevelField = value;
            }

            public orderOrderHeaderCarrier carrier
            {
                get => carrierField;
                set => carrierField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeaderNumber
        {
            private ulong numberRequestedField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public ulong numberRequested
            {
                get => numberRequestedField;
                set => numberRequestedField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeaderPartnerIdentity
        {
            private address addressField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public address address
            {
                get => addressField;
                set => addressField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd",
            IsNullable = false)]
        public class address
        {
            private string companyField;

            private string divisionField;

            private string nameField;

            private string cityField;

            private string streetField;

            private string zipField;

            private uint icoField;

            private string dicField;

            private string phoneField;

            private string emailField;

            private addressCountry countryField;

            public string company
            {
                get => companyField;
                set => companyField = value;
            }

            public string division
            {
                get => divisionField;
                set => divisionField = value;
            }

            public string name
            {
                get => nameField;
                set => nameField = value;
            }

            public string city
            {
                get => cityField;
                set => cityField = value;
            }

            public string street
            {
                get => streetField;
                set => streetField = value;
            }

            public string zip
            {
                get => zipField;
                set => zipField = value;
            }

            public uint ico
            {
                get => icoField;
                set => icoField = value;
            }

            public string dic
            {
                get => dicField;
                set => dicField = value;
            }

            public string phone
            {
                get => phoneField;
                set => phoneField = value;
            }

            public string email
            {
                get => emailField;
                set => emailField = value;
            }

            public addressCountry country
            {
                get => countryField;
                set => countryField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public class addressCountry
        {
            private string idsField;

            public string ids
            {
                get => idsField;
                set => idsField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeaderPaymentType
        {
            private string idsField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public string ids
            {
                get => idsField;
                set => idsField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeaderPriceLevel
        {
            private string idsField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public string ids
            {
                get => idsField;
                set => idsField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderHeaderCarrier
        {
            private string idsField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public string ids
            {
                get => idsField;
                set => idsField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderItem
        {
            private string textField;

            private byte quantityField;

            private byte deliveredField;

            private string rateVATField;

            private orderOrderItemHomeCurrency homeCurrencyField;

            private orderOrderItemStockItem stockItemField;

            public string text
            {
                get => textField;
                set => textField = value;
            }

            public byte quantity
            {
                get => quantityField;
                set => quantityField = value;
            }

            public byte delivered
            {
                get => deliveredField;
                set => deliveredField = value;
            }

            public string rateVAT
            {
                get => rateVATField;
                set => rateVATField = value;
            }

            public orderOrderItemHomeCurrency homeCurrency
            {
                get => homeCurrencyField;
                set => homeCurrencyField = value;
            }

            public orderOrderItemStockItem stockItem
            {
                get => stockItemField;
                set => stockItemField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderItemHomeCurrency
        {
            private decimal unitPriceField;

            private bool unitPriceFieldSpecified;

            private decimal priceVATField;

            private decimal priceField;

            private bool priceFieldSpecified;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public decimal unitPrice
            {
                get => unitPriceField;
                set => unitPriceField = value;
            }

            [XmlIgnore]
            public bool unitPriceSpecified
            {
                get => unitPriceFieldSpecified;
                set => unitPriceFieldSpecified = value;
            }

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public decimal priceVAT
            {
                get => priceVATField;
                set => priceVATField = value;
            }

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public decimal price
            {
                get => priceField;
                set => priceField = value;
            }

            [XmlIgnore]
            public bool priceSpecified
            {
                get => priceFieldSpecified;
                set => priceFieldSpecified = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderItemStockItem
        {
            private stockItem stockItemField;

            [XmlElement(Namespace =
                "http://www.stormware.cz/schema/version_2/type.xsd")]
            public stockItem stockItem
            {
                get => stockItemField;
                set => stockItemField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd",
            IsNullable = false)]
        public class stockItem
        {
            private string eANField;

            public string EAN
            {
                get => eANField;
                set => eANField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true,
            Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public class orderOrderSummary
        {
            private string roundingDocumentField;

            public string roundingDocument
            {
                get => roundingDocumentField;
                set => roundingDocumentField = value;
            }
        }
    }
}