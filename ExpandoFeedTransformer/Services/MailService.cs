using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace ExpandoFeedTransformer.Services
{
    public class MailService
    {
        private readonly string _from;
        private readonly string _to;
        private readonly string _cc;
        private readonly string _password;
        private string _template;
        private string _rowTemplate;
        private string _itemTemplate;
        private string _rows;

        public MailService(string from, string to, string cc, string password)
        {
            _from = from;
            _to = to;
            _cc = cc;
            _password = password;
            _template = "";
            _rowTemplate = "";
            _itemTemplate = "";
            _rows = "";
        }

        public void CreateTemplate(string template)
        {
            _template = template;
        }

        public void CreateRowTemplate(string template)
        {
            _rowTemplate = template;
        }

        public void CreateItemTemplate(string template)
        {
            _itemTemplate = template;
        }

        public void AddRow(ExpandoFeed.ordersOrder order, List<PrehomeFeed.SHOPSHOPITEM> items)
        {
            _rows = _rowTemplate;
            _rows = _rows.Replace("[[OrderId]]", order.orderId);
            _rows = _rows.Replace("[[purchaseDate]]", order.purchaseDate.Split(" ")[0]);
            _rows = _rows.Replace("[[latestShipDate]]", order.latestShipDate.Split(" ")[0]);
            _rows = _rows.Replace("[[totalPrice]]", order.totalPrice.ToString(CultureInfo.InvariantCulture));
            _rows = _rows.Replace("[[companyName]]", order.customer.companyName);
            _rows = _rows.Replace("[[firstName]]", order.customer.firstname);
            _rows = _rows.Replace("[[surname]]", order.customer.surname);
            _rows = _rows.Replace("[[address]]", order.customer.address.address1);
            _rows = _rows.Replace("[[city]]", order.customer.address.city);
            _rows = _rows.Replace("[[zip]]", order.customer.address.zip);
            _rows = _rows.Replace("[[country]]", order.customer.address.country);

            var data = "";

            foreach (var item in items)
            {
                var temp = _itemTemplate;
                temp = temp.Replace("[[ID]]", item.ITEM_ID.ToString());
                temp = temp.Replace("[[EAN]]", item.EAN);
                temp = temp.Replace("[[URL]]", item.URL);
                data += temp;
            }

            _rows = _rows.Replace("[[items]]", data);
        } 

        public void PopulateTemplate()
        {
            _template = _template.Replace("[[data]]", _rows);
        }

        public void SendMail()
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient();
                message.From = new MailAddress(_from);
                message.To.Add(new MailAddress(_to));
                message.CC.Add(new MailAddress(_cc));
                message.Subject = "Expando - Amazon objednavky za den [Today-1]";
                message.IsBodyHtml = true;
                message.Body = _template;
                smtp.Port = 587;
                smtp.Host = "mail.hostmaster.sk";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_from, _password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                _rows = "";
            }
            catch (Exception)
            {
                Console.WriteLine("Error while sending mail :(");
            }
        }
    }
}
