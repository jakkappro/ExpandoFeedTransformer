using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace ExpandoFeedTransformer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Getting expando feed");

            var orders = await GetExpandoOrders(1);

            Console.WriteLine("Getting prehome feed");

            var items = await GetPrehomeFeed();

            var mailMessage =
                @"<!DOCTYPE html>
                    <html lang=""en"">
                      <head>
                        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                        <meta name=""viewport"" content=""width=device-width"" />
                        <title></title>
                        <style></style>
                      </head>

                      <body>
                        <table cellpadding=""0"" cellspacing=""5"" width=""100%"" align=""center"" border=""0"">
                          <tr>
                            <td>
                              <table cellpadding=""0"" cellspacing=""5"" align=""left"" border=""1"">
                                <table
                                  cellpadding=""0""
                                  cellspacing=""5""
                                  align=""left""
                                  border=""1""
                                >[[DATA]]</table>
                              </table>
                            </td>
                          </tr>
                        </table>
                      </body>
                    </html>";

            var message = @"<tr><td>OrderId</td>
                <td>purchaseDate</td>
                <td>latestShipDate</td>
                <td>totalPrice</td>
                <td>companyName</td>
                <td>firstName</td>
                <td>surname</td>
                <td>address</td>
                <td>city</td>
                <td>zip</td>
                <td>country</td>
                <td>
                <table cellpadding=""0"" cellspacing=""5"" align=""left"" border=""1"">
                <tr>
                <td>itemId</td>
                <td>ean</td>
                <td>link></td>
                </tr>
                </table>
                </td>
                </tr>";

            Console.WriteLine("Creating orders");

            foreach (var order in orders.order)
            {
                if (order.orderStatus == "Canceled")
                {
                    continue;
                }

                message += @"<tr>
                    <td>[[OrderId]]</td>
                    <td>[[purchaseDate]]</td>
                    <td>[[latestShipDate]]</td>
                    <td>[[totalPrice]]</td>
                    <td>[[companyName]]</td>
                    <td>[[firstName]]</td>
                    <td>[[surname]]</td>
                    <td>[[address]]</td>
                    <td>[[city]]</td>
                    <td>[[zip]]</td>
                    <td>[[country]]</td>
                    <td>
                    <table cellpadding=""0"" cellspacing=""5"" align=""left"" border=""1"">
                    <tr>
                    [[items]]
                    </tr>
                    </table>
                    </td> 
                    </tr>";

                var data = "";

                message = message.Replace("[[OrderId]]", order.orderId);
                message = message.Replace("[[purchaseDate]]", order.purchaseDate.Split(" ")[0]);
                message = message.Replace("[[latestShipDate]]", order.latestShipDate.Split(" ")[0]);
                message = message.Replace("[[totalPrice]]", order.totalPrice.ToString(CultureInfo.InvariantCulture));
                message = message.Replace("[[companyName]]", order.customer.companyName);
                message = message.Replace("[[firstName]]", order.customer.firstname);
                message = message.Replace("[[surname]]", order.customer.surname);
                message = message.Replace("[[address]]", order.customer.address.address1);
                message = message.Replace("[[city]]", order.customer.address.city);
                message = message.Replace("[[zip]]", order.customer.address.zip);
                message = message.Replace("[[country]]", order.customer.address.country);

                foreach (var item in order.items)
                {
                    var i = items.Find(e => e.ITEM_ID == item.itemId);

                    data += $"<td>{item.itemId}</td><td>{i.EAN}</td><td><a href={i.URL}>link</a></td>";

                    i = null;
                }

                message = message.Replace("[[items]]", data);
            }

            mailMessage = mailMessage.Replace("[[DATA]]", message);

            Console.WriteLine("Sending mail");
            sendEmail(mailMessage);
            Console.WriteLine("Finished");
            Console.Read();
        }

        static void sendEmail(string htmlString)
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient();
                message.From = new MailAddress("noreply@azetcool.com");
                message.To.Add(new MailAddress("info@prehome.sk"));
                message.Subject = "Expando - Amazon objednavky za den [Today-1]";
                message.IsBodyHtml = true;
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "mail.hostmaster.sk";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("noreply@azetcool.com", "W6&2bB9;T?ukTk4");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Error while sending mail :(");
            }
        }

        private static async Task<List<PrehomeFeed.SHOPSHOPITEM>> GetPrehomeFeed()
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://www.prehome.sk/feed/amazon.xml")
            };
            using var response = await httpClient.GetAsync("");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException("Couldn't get feed");
            }

            return PrehomeFeed.Deserialize(await response.Content.ReadAsStringAsync()).SHOPITEM.ToList();
        }

        private static async Task<ExpandoFeed.orders> GetExpandoOrders(int numberOfDays)
        {
            using var httpClient = new HttpClient
            {
                BaseAddress =
                    new Uri(
                        $"https://app.expan.do/api/v2/orderfeed?access_token=11w1QgSM7YR4tHyr4BR0BV&days={numberOfDays}")
            };
            using var response = await httpClient.GetAsync("");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException("Couldn't get feed");
            }

            return ExpandoFeed.Deserialize(await response.Content.ReadAsStringAsync());
        }
    }
}