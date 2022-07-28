﻿using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer;

public class PohodaGetOrdersByDateRequest
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/data.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/data.xsd", IsNullable = false)]
    public class dataPack
    {
        private dataPackDataPackItem dataPackItemField;

        private int idField;

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
        public int id
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
            var s = writer.ToString() ?? throw new InvalidOperationException();
            writer.Flush();
            writer.Close();
            return s;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/data.xsd")]
    public class dataPackDataPackItem
    {
        private listOrderRequest listOrderRequestField;

        private string idField;

        private decimal versionField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
        public listOrderRequest listOrderRequest
        {
            get => listOrderRequestField;
            set => listOrderRequestField = value;
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
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/list.xsd", IsNullable = false)]
    public class listOrderRequest
    {
        private listOrderRequestRequestOrder requestOrderField;

        private decimal versionField;

        private string orderTypeField;

        private decimal orderVersionField;

        public listOrderRequestRequestOrder requestOrder
        {
            get => requestOrderField;
            set => requestOrderField = value;
        }

        [XmlAttribute]
        public decimal version
        {
            get => versionField;
            set => versionField = value;
        }

        [XmlAttribute]
        public string orderType
        {
            get => orderTypeField;
            set => orderTypeField = value;
        }

        [XmlAttribute]
        public decimal orderVersion
        {
            get => orderVersionField;
            set => orderVersionField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/list.xsd")]
    public class listOrderRequestRequestOrder
    {
        private filter filterField;

        [XmlElement(Namespace = "http://www.stormware.cz/schema/version_2/filter.xsd")]
        public filter filter
        {
            get => filterField;
            set => filterField = value;
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.stormware.cz/schema/version_2/filter.xsd")]
    [XmlRoot(Namespace = "http://www.stormware.cz/schema/version_2/filter.xsd", IsNullable = false)]
    public class filter
    {
        private string dateFromField;

        private string dateTillField;

        [XmlElement(DataType = "date")]
        public string dateFrom
        {
            get => dateFromField;
            set => dateFromField = value;
        }

        [XmlElement(DataType = "date")]
        public string dateTill
        {
            get => dateTillField;
            set => dateTillField = value;
        }
    }
}