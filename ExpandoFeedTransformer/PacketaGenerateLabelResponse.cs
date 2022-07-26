using System.ComponentModel;
using System.Xml.Serialization;

namespace ExpandoFeedTransformer
{
    public class PacketaGenerateLabelResponse
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

            private string resultField;

            public string status
            {
                get => statusField;
                set => statusField = value;
            }

            public string result
            {
                get => resultField;
                set => resultField = value;
            }
        }
    }
}