using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer;

public class PohodaGetOrdersByDateResponse
{
    public static responsePack Deserialize(string source)
    {
        var serializer = new XmlSerializer(typeof(responsePack));
        using var reader = new StringReader(source);
        return (responsePack)serializer.Deserialize(reader);
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/response.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/response.xsd", IsNullable = false)]
    public class responsePack
    {
        private responsePackResponsePackItem responsePackItemField;

        private decimal versionField;

        private byte idField;

        private string stateField;

        private string programVersionField;

        private uint icoField;

        private string keyField;

        private string noteField;

        public responsePackResponsePackItem responsePackItem
        {
            get => responsePackItemField;
            set => responsePackItemField = value;
        }

        [XmlAttribute]
        public decimal version
        {
            get => versionField;
            set => versionField = value;
        }

        [XmlAttribute]
        public byte id
        {
            get => idField;
            set => idField = value;
        }

        [XmlAttribute]
        public string state
        {
            get => stateField;
            set => stateField = value;
        }

        [XmlAttribute]
        public string programVersion
        {
            get => programVersionField;
            set => programVersionField = value;
        }

        [XmlAttribute]
        public uint ico
        {
            get => icoField;
            set => icoField = value;
        }

        [XmlAttribute]
        public string key
        {
            get => keyField;
            set => keyField = value;
        }

        [XmlAttribute]
        public string note
        {
            get => noteField;
            set => noteField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/response.xsd")]
    public class responsePackResponsePackItem
    {
        private listOrder listOrderField;

        private decimal versionField;

        private string idField;

        private string stateField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
        public listOrder listOrder
        {
            get => listOrderField;
            set => listOrderField = value;
        }

        [XmlAttribute]
        public decimal version
        {
            get => versionField;
            set => versionField = value;
        }

        [XmlAttribute]
        public string id
        {
            get => idField;
            set => idField = value;
        }

        [XmlAttribute]
        public string state
        {
            get => stateField;
            set => stateField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/list.xsd", IsNullable = false)]
    public class listOrder
    {
        private listOrderOrder[] orderField;

        private decimal versionField;

        private DateTime dateTimeStampField;

        private DateTime dateValidFromField;

        private string stateField;

        [XmlElement("order")]
        public listOrderOrder[] order
        {
            get => orderField;
            set => orderField = value;
        }

        [XmlAttribute]
        public decimal version
        {
            get => versionField;
            set => versionField = value;
        }

        [XmlAttribute]
        public DateTime dateTimeStamp
        {
            get => dateTimeStampField;
            set => dateTimeStampField = value;
        }

        [XmlAttribute(DataType = "date")]
        public DateTime dateValidFrom
        {
            get => dateValidFromField;
            set => dateValidFromField = value;
        }

        [XmlAttribute]
        public string state
        {
            get => stateField;
            set => stateField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
    public class listOrderOrder
    {
        private orderHeader orderHeaderField;

        private orderDetailOrderItem[] orderDetailField;

        private orderSummary orderSummaryField;

        private linkedDocuments linkedDocumentsField;

        private decimal versionField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public orderHeader orderHeader
        {
            get => orderHeaderField;
            set => orderHeaderField = value;
        }

        [XmlArray(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        [XmlArrayItem("orderItem", IsNullable = false)]
        public orderDetailOrderItem[] orderDetail
        {
            get => orderDetailField;
            set => orderDetailField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public orderSummary orderSummary
        {
            get => orderSummaryField;
            set => orderSummaryField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
        public linkedDocuments linkedDocuments
        {
            get => linkedDocumentsField;
            set => linkedDocumentsField = value;
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
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd", IsNullable = false)]
    public class orderHeader
    {
        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        [XmlElement("MOSS", typeof(orderHeaderMOSS))]
        [XmlElement("carrier", typeof(orderHeaderCarrier))]
        [XmlElement("date", typeof(DateTime), DataType = "date")]
        [XmlElement("dateFrom", typeof(DateTime), DataType = "date")]
        [XmlElement("dateTo", typeof(DateTime), DataType = "date")]
        [XmlElement("evidentiaryResourcesMOSS", typeof(orderHeaderEvidentiaryResourcesMOSS))]
        [XmlElement("histRate", typeof(bool))]
        [XmlElement("id", typeof(byte))]
        [XmlElement("isDelivered", typeof(bool))]
        [XmlElement("isExecuted", typeof(bool))]
        [XmlElement("isReserved", typeof(bool))]
        [XmlElement("markRecord", typeof(bool))]
        [XmlElement("myIdentity", typeof(orderHeaderMyIdentity))]
        [XmlElement("note", typeof(string))]
        [XmlElement("number", typeof(orderHeaderNumber))]
        [XmlElement("numberOrder", typeof(string))]
        [XmlElement("orderType", typeof(string))]
        [XmlElement("parameters", typeof(orderHeaderParameters))]
        [XmlElement("partnerIdentity", typeof(orderHeaderPartnerIdentity))]
        [XmlElement("paymentType", typeof(orderHeaderPaymentType))]
        [XmlElement("permanentDocument", typeof(bool))]
        [XmlElement("text", typeof(string))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items
        {
            get => itemsField;
            set => itemsField = value;
        }

        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType[] ItemsElementName
        {
            get => itemsElementNameField;
            set => itemsElementNameField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderMOSS
    {
        private string idsField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderCarrier
    {
        private byte idField;

        private string idsField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte id
        {
            get => idField;
            set => idField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderEvidentiaryResourcesMOSS
    {
        private string idsField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderMyIdentity
    {
        private address addressField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public address address
        {
            get => addressField;
            set => addressField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class address
    {
        private string companyField;

        private string nameField;

        private string cityField;

        private string streetField;

        private string numberField;

        private string zipField;

        private uint icoField;

        private bool icoFieldSpecified;

        private uint dicField;

        private bool dicFieldSpecified;

        private string icDphField;

        private addressCountry countryField;

        private string mobilPhoneField;

        private long phoneField;

        private bool phoneFieldSpecified;

        private string emailField;

        public string company
        {
            get => companyField;
            set => companyField = value;
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

        public string number
        {
            get => numberField;
            set => numberField = value;
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

        [XmlIgnore]
        public bool icoSpecified
        {
            get => icoFieldSpecified;
            set => icoFieldSpecified = value;
        }

        public uint dic
        {
            get => dicField;
            set => dicField = value;
        }

        [XmlIgnore]
        public bool dicSpecified
        {
            get => dicFieldSpecified;
            set => dicFieldSpecified = value;
        }

        public string icDph
        {
            get => icDphField;
            set => icDphField = value;
        }

        public addressCountry country
        {
            get => countryField;
            set => countryField = value;
        }

        public string mobilPhone
        {
            get => mobilPhoneField;
            set => mobilPhoneField = value;
        }

        public long phone
        {
            get => phoneField;
            set => phoneField = value;
        }

        [XmlIgnore]
        public bool phoneSpecified
        {
            get => phoneFieldSpecified;
            set => phoneFieldSpecified = value;
        }

        public string email
        {
            get => emailField;
            set => emailField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    public class addressCountry
    {
        private byte idField;

        private string idsField;

        public byte id
        {
            get => idField;
            set => idField = value;
        }

        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderNumber
    {
        private uint numberRequestedField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public uint numberRequested
        {
            get => numberRequestedField;
            set => numberRequestedField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderParameters
    {
        private parameter[] parameterField;

        [XmlElement("parameter", Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public parameter[] parameter
        {
            get => parameterField;
            set => parameterField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class parameter
    {
        private string nameField;

        private string textValueField;

        private DateTime datetimeValueField;

        private bool datetimeValueFieldSpecified;

        private bool booleanValueField;

        private bool booleanValueFieldSpecified;

        public string name
        {
            get => nameField;
            set => nameField = value;
        }

        public string textValue
        {
            get => textValueField;
            set => textValueField = value;
        }

        [XmlElement(DataType = "date")]
        public DateTime datetimeValue
        {
            get => datetimeValueField;
            set => datetimeValueField = value;
        }

        [XmlIgnore]
        public bool datetimeValueSpecified
        {
            get => datetimeValueFieldSpecified;
            set => datetimeValueFieldSpecified = value;
        }

        public bool booleanValue
        {
            get => booleanValueField;
            set => booleanValueField = value;
        }

        [XmlIgnore]
        public bool booleanValueSpecified
        {
            get => booleanValueFieldSpecified;
            set => booleanValueFieldSpecified = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderPartnerIdentity
    {
        private address addressField;

        private shipToAddress shipToAddressField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public address address
        {
            get => addressField;
            set => addressField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public shipToAddress shipToAddress
        {
            get => shipToAddressField;
            set => shipToAddressField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class shipToAddress
    {
        private string companyField;

        private string nameField;

        private string cityField;

        private string streetField;

        private string zipField;

        private shipToAddressCountry countryField;

        private uint phoneField;

        private string emailField;

        public string company
        {
            get => companyField;
            set => companyField = value;
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

        public shipToAddressCountry country
        {
            get => countryField;
            set => countryField = value;
        }

        public uint phone
        {
            get => phoneField;
            set => phoneField = value;
        }

        public string email
        {
            get => emailField;
            set => emailField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    public class shipToAddressCountry
    {
        private byte idField;

        private string idsField;

        public byte id
        {
            get => idField;
            set => idField = value;
        }

        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderHeaderPaymentType
    {
        private byte idField;

        private string idsField;

        private string paymentTypeField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte id
        {
            get => idField;
            set => idField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string ids
        {
            get => idsField;
            set => idsField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string paymentType
        {
            get => paymentTypeField;
            set => paymentTypeField = value;
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        MOSS,

        carrier,

        date,

        dateFrom,

        dateTo,

        evidentiaryResourcesMOSS,

        histRate,

        id,

        isDelivered,

        isExecuted,

        isReserved,

        markRecord,

        myIdentity,

        note,

        number,

        numberOrder,

        orderType,

        parameters,

        partnerIdentity,

        paymentType,

        permanentDocument,

        text,
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderDetailOrderItem
    {
        private ushort idField;

        private string textField;

        private decimal quantityField;

        private decimal deliveredField;

        private string unitField;

        private decimal coefficientField;

        private bool payVATField;

        private string rateVATField;

        private decimal percentVATField;

        private bool percentVATFieldSpecified;

        private decimal discountPercentageField;

        private orderDetailOrderItemHomeCurrency homeCurrencyField;

        private string noteField;

        private orderDetailOrderItemTypeServiceMOSS typeServiceMOSSField;

        private string codeField;

        private orderDetailOrderItemStockItem stockItemField;

        private bool pDPField;

        public ushort id
        {
            get => idField;
            set => idField = value;
        }

        public string text
        {
            get => textField;
            set => textField = value;
        }

        public decimal quantity
        {
            get => quantityField;
            set => quantityField = value;
        }

        public decimal delivered
        {
            get => deliveredField;
            set => deliveredField = value;
        }

        public string unit
        {
            get => unitField;
            set => unitField = value;
        }

        public decimal coefficient
        {
            get => coefficientField;
            set => coefficientField = value;
        }

        public bool payVAT
        {
            get => payVATField;
            set => payVATField = value;
        }

        public string rateVAT
        {
            get => rateVATField;
            set => rateVATField = value;
        }

        public decimal percentVAT
        {
            get => percentVATField;
            set => percentVATField = value;
        }

        [XmlIgnore]
        public bool percentVATSpecified
        {
            get => percentVATFieldSpecified;
            set => percentVATFieldSpecified = value;
        }

        public decimal discountPercentage
        {
            get => discountPercentageField;
            set => discountPercentageField = value;
        }

        public orderDetailOrderItemHomeCurrency homeCurrency
        {
            get => homeCurrencyField;
            set => homeCurrencyField = value;
        }

        public string note
        {
            get => noteField;
            set => noteField = value;
        }

        public orderDetailOrderItemTypeServiceMOSS typeServiceMOSS
        {
            get => typeServiceMOSSField;
            set => typeServiceMOSSField = value;
        }

        public string code
        {
            get => codeField;
            set => codeField = value;
        }

        public orderDetailOrderItemStockItem stockItem
        {
            get => stockItemField;
            set => stockItemField = value;
        }

        public bool PDP
        {
            get => pDPField;
            set => pDPField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderDetailOrderItemHomeCurrency
    {
        private decimal unitPriceField;

        private decimal priceField;

        private decimal priceVATField;

        private decimal priceSumField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal unitPrice
        {
            get => unitPriceField;
            set => unitPriceField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal price
        {
            get => priceField;
            set => priceField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal priceVAT
        {
            get => priceVATField;
            set => priceVATField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal priceSum
        {
            get => priceSumField;
            set => priceSumField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderDetailOrderItemTypeServiceMOSS
    {
        private string idsField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderDetailOrderItemStockItem
    {
        private store storeField;

        private stockItem stockItemField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public store store
        {
            get => storeField;
            set => storeField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public stockItem stockItem
        {
            get => stockItemField;
            set => stockItemField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class store
    {
        private byte idField;

        private string idsField;

        public byte id
        {
            get => idField;
            set => idField = value;
        }

        public string ids
        {
            get => idsField;
            set => idsField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class stockItem
    {
        private ushort idField;

        private string idsField;

        private ulong eANField;

        private bool eANFieldSpecified;

        public ushort id
        {
            get => idField;
            set => idField = value;
        }

        public string ids
        {
            get => idsField;
            set => idsField = value;
        }

        public ulong EAN
        {
            get => eANField;
            set => eANField = value;
        }

        [XmlIgnore]
        public bool EANSpecified
        {
            get => eANFieldSpecified;
            set => eANFieldSpecified = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd", IsNullable = false)]
    public class orderSummary
    {
        private string roundingDocumentField;

        private string roundingVATField;

        private bool calculateVATField;

        private bool calculateVATFieldSpecified;

        private orderSummaryHomeCurrency homeCurrencyField;

        public string roundingDocument
        {
            get => roundingDocumentField;
            set => roundingDocumentField = value;
        }

        public string roundingVAT
        {
            get => roundingVATField;
            set => roundingVATField = value;
        }

        public bool calculateVAT
        {
            get => calculateVATField;
            set => calculateVATField = value;
        }

        [XmlIgnore]
        public bool calculateVATSpecified
        {
            get => calculateVATFieldSpecified;
            set => calculateVATFieldSpecified = value;
        }

        public orderSummaryHomeCurrency homeCurrency
        {
            get => homeCurrencyField;
            set => homeCurrencyField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    public class orderSummaryHomeCurrency
    {
        private byte priceNoneField;

        private byte priceLowField;

        private byte priceLowVATField;

        private byte priceLowSumField;

        private decimal priceHighField;

        private decimal priceHighVATField;

        private decimal priceHighSumField;

        private byte price3Field;

        private byte price3VATField;

        private byte price3SumField;

        private round roundField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte priceNone
        {
            get => priceNoneField;
            set => priceNoneField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte priceLow
        {
            get => priceLowField;
            set => priceLowField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte priceLowVAT
        {
            get => priceLowVATField;
            set => priceLowVATField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte priceLowSum
        {
            get => priceLowSumField;
            set => priceLowSumField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal priceHigh
        {
            get => priceHighField;
            set => priceHighField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal priceHighVAT
        {
            get => priceHighVATField;
            set => priceHighVATField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public decimal priceHighSum
        {
            get => priceHighSumField;
            set => priceHighSumField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte price3
        {
            get => price3Field;
            set => price3Field = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte price3VAT
        {
            get => price3VATField;
            set => price3VATField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public byte price3Sum
        {
            get => price3SumField;
            set => price3SumField = value;
        }

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public round round
        {
            get => roundField;
            set => roundField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class round
    {
        private decimal priceRoundField;

        public decimal priceRound
        {
            get => priceRoundField;
            set => priceRoundField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd", IsNullable = false)]
    public class linkedDocuments
    {
        private link linkField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
        public link link
        {
            get => linkField;
            set => linkField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/type.xsd", IsNullable = false)]
    public class link
    {
        private string sourceAgendaField;

        private linkSourceDocument sourceDocumentField;

        public string sourceAgenda
        {
            get => sourceAgendaField;
            set => sourceAgendaField = value;
        }

        public linkSourceDocument sourceDocument
        {
            get => sourceDocumentField;
            set => sourceDocumentField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/type.xsd")]
    public class linkSourceDocument
    {
        private byte idField;

        private uint numberField;

        public byte id
        {
            get => idField;
            set => idField = value;
        }

        public uint number
        {
            get => numberField;
            set => numberField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/order.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/order.xsd", IsNullable = false)]
    public class orderDetail
    {
        private orderDetailOrderItem[] orderItemField;

        [XmlElement("orderItem")]
        public orderDetailOrderItem[] orderItem
        {
            get => orderItemField;
            set => orderItemField = value;
        }
    }
}