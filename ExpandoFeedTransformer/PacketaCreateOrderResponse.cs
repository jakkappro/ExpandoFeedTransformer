using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer
{
    public class PacketaCreateOrderResponse
    {
        public static response Deserialize(string source)
        {
            var serializer = new XmlSerializer(typeof(response));
            using var reader = new StringReader(source);
            return (response)serializer.Deserialize(reader);
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        public class response
        {
            private string statusField;

            private responseResult resultField;

            public string status
            {
                get => statusField;
                set => statusField = value;
            }

            public responseResult result
            {
                get => resultField;
                set => resultField = value;
            }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        public class responseResult
        {
            private ulong idField;

            private string barcodeField;

            private string barcodeTextField;

            public long id
            {
                get => idField;
                set => idField = value;
            }

            public string barcode
            {
                get => barcodeField;
                set => barcodeField = value;
            }

            public string barcodeText
            {
                get => barcodeTextField;
                set => barcodeTextField = value;
            }
        }
    }
}