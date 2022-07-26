using System.Net;
using System.Text;
using ExpandoFeedTransformer.Factories.Pohoda;
using ExpandoFeedTransformer.Services;

namespace ExpandoFeedTransformer
{
    internal class Program
    {
        private const string path = "\\\\AzetCool-Pohoda\\POHODA_SK_E1_DATA\\Dokumenty\\ACecom\\Obrázky\\";
        private static uint num = 3;

        private static async Task Main(string[] args)
        {
            var line = "000";
            try
            {
                var sr = new StreamReader("index.txt");
                line = await sr.ReadLineAsync();
                sr.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            var v = uint.Parse($"3{DateTime.Now:yyMMdd}{line}");
            num = v;
            Console.WriteLine("Starting pohoda mServer");
            var mServer = new PohodaMServer("test", "\"C:\\Program Files (x86)\\STORMWARE\\POHODA SK E1\"",
                "http://127.0.0.1:5336", "admin", "acecom", 1000, 3);

            mServer.StartServer();

            if (!await mServer.IsConnectionAvailable())
            {
                Console.WriteLine("Couldn't connect to mServer");
                return;
            }


            Console.WriteLine("Getting expando feed");

            var orders = await GetExpandoOrders(1);

            Console.WriteLine("Getting prehome feed");

            var items = await GetPrehomeFeed();

            #region Templates

            const string mailMessage =
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
                                            <table cellpadding=""0"" cellspacing=""5"" align="" left"" border=""1"">
                                                <tr>
                                                    <td>OrderId</td>
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
                                                </tr>
                                                [[data]]
                                            </table>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                </html>";

            const string row = @"<tr>
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

            const string itemTemplate = @"<td>[[ID]]</td>
                                <td>[[EAN]]</td>
                                <td><a href=""[[URL]]"">link</a></td>";

            #endregion

            var mail = new MailService("noreply@azetcool.com", "jakkappro@gmail.com", "pojtek@gmail.com",
                "W6&2bB9;T?ukTk4");

            mail.CreateTemplate(mailMessage);
            mail.CreateRowTemplate(row);
            mail.CreateItemTemplate(itemTemplate);

            Console.WriteLine("Creating orders");

            foreach (var order in orders.order)
            {
                var failed = false;
                Console.WriteLine($"Creating order {order.orderId}.");
                using var client = new HttpClient();

                var orderDetail = new List<PohodaCreateOrder.orderOrderItem>();

                var shopItems = new List<PrehomeFeed.SHOPSHOPITEM>();

                foreach (var item in order.items)
                {
                    Console.WriteLine($"Trying to get stock EAN: {item.itemId}");
                    var i = items.Find(e => e.ITEM_ID == item.itemId);
                    if (i is null)
                    {
                        Console.WriteLine($"Couldn't find stock, EAN {item.itemId}, skipping stock \n\n\n");
                        failed = true;
                        break;
                    }
                    Console.WriteLine($"Found stock {i.URL}");
                    shopItems.Add(i);
                    var request = PohodaGetStockRequestFactory.CreateRequest(item);

                    var response =
                        PohodaGetStockResponse.Deserialize(
                            await mServer.SendRequest(PohodaGetStockRequest.dataPack.Serialize(request)));

                    if (response.responsePackItem.listStock.stock is null)
                    {
                        Console.WriteLine("Couldn't find stock, creating new one");
                        var dataPack = await PohodaCreateStockFactory.CreateRequest(i, path, client);
                        Console.WriteLine("Sending request");
                        await mServer.SendRequest(PohodaCreateStock.dataPack.Serialize(dataPack));
                    }

                    // shipping
                    var orderItem = new PohodaCreateOrder.orderOrderItem()
                    {
                        text = "Doprava",
                        quantity = 1,
                        delivered = 0,
                        rateVAT = "high",
                        homeCurrency = new PohodaCreateOrder.orderOrderItemHomeCurrency()
                        {
                            unitPrice = 5.0m,
                            unitPriceSpecified = true
                        }
                    };

                    // order item
                    var orderItem2 = new PohodaCreateOrder.orderOrderItem()
                    {
                        quantity = item.itemQuantity,
                        delivered = 0,
                        rateVAT = "high",
                        stockItem = new PohodaCreateOrder.orderOrderItemStockItem()
                        {
                            stockItem = new PohodaCreateOrder.stockItem()
                            {
                                EAN = i.EAN
                            }
                        },
                        homeCurrency = new PohodaCreateOrder.orderOrderItemHomeCurrency()
                        {
                            unitPrice = i.PRICE_VAT * 1.2m,
                            unitPriceSpecified = true
                        }
                    };

                    orderDetail.Add(orderItem);
                    orderDetail.Add(orderItem2);
                }

                if (order.orderStatus == "Canceled" || failed)
                {
                    // update order to canceled
                    continue;
                }

                var orderDataPack = new PohodaCreateOrder.dataPack()
                {
                    version = 2.0m,
                    ico = 53870441,
                    note = "Export",
                    id = "zas001",
                    application = "StwTest",
                    dataPackItem = new PohodaCreateOrder.dataPackDataPackItem()
                    {
                        id = "zas001",
                        version = 2.0m,
                        order = new PohodaCreateOrder.order()
                        {
                            version = 2.0m,
                            orderHeader = new PohodaCreateOrder.orderOrderHeader
                            {
                                orderType = "receivedOrder",
                                numberOrder = order.orderId,
                                date = Convert.ToDateTime(order.purchaseDate.Split(" ")[0]),
                                dateFrom = Convert.ToDateTime(order.purchaseDate.Split(" ")[0]),
                                dateTo = Convert.ToDateTime(order.purchaseDate.Split(" ")[0]),
                                text = $"Amazon objednavka c. {order.orderId}",
                                partnerIdentity = new PohodaCreateOrder.orderOrderHeaderPartnerIdentity()
                                {
                                    address = new PohodaCreateOrder.address()
                                    {
                                        name = order.customer.firstname + " " + order.customer.surname,
                                        city = order.customer.address.city,
                                        street = order.customer.address.address1,
                                        zip = order.customer.address.zip,
                                        country = new PohodaCreateOrder.addressCountry()
                                        {
                                            ids = order.customer.address.country
                                        },
                                        company = order.customer.companyName ?? "",
                                        dic = "",
                                        division = "",
                                        email = order.customer.email,
                                        phone = order.customer.phone
                                    }
                                },
                                paymentType = new PohodaCreateOrder.orderOrderHeaderPaymentType()
                                {
                                    ids = "Plat.kartou"
                                },
                                priceLevel = new PohodaCreateOrder.orderOrderHeaderPriceLevel()
                                {
                                    ids = order.customer.address.country == "DE"
                                        ? "DE"
                                        : "Predajná"
                                },
                                number = new PohodaCreateOrder.orderOrderHeaderNumber()
                                {
                                    numberRequested = num
                                },
                                carrier = new PohodaCreateOrder.orderOrderHeaderCarrier()
                                {
                                    ids = order.customer.address.country == "DE"
                                        ? "Doprava DE"
                                        : "Doprava AT"
                                },

                            },
                            // not good solution what about more items at once can i even do it TODO: figure it out :D
                            orderDetail = orderDetail.ToArray(),
                            orderSummary = new PohodaCreateOrder.orderOrderSummary()
                            {
                                roundingDocument = "math2half" // Toto treba zmenit na ine zaokruhlovanie
                            }
                        }
                    }
                };

                num += 1;

                await Task.Delay(1500);

                await mServer.SendRequest(PohodaCreateOrder.dataPack.Serialize(orderDataPack));

                mail.AddRow(order, shopItems);
            }

            Console.WriteLine("Sending mail");
            mail.PopulateTemplate();
            mail.SendMail();
            Console.WriteLine("Finished");
            Console.Read();
            // mServer.StopServer();

            try
            {
                var s = num.ToString();
                var write = s.Length >= 3 ? s : s.Length == 2 ? "0" + s : "00" + s;
                var sw = new StreamWriter("index.txt", false, Encoding.ASCII);
                await sw.WriteAsync(write);
                sw.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
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