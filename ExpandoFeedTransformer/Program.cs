using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace ExpandoFeedTransformer
{
    internal class Program
    {
        // TODO: set right path this one is temporary
        private const string path = "\"\\\\AzetCool-Pohoda\\POHODA_SK_E1_DATA\\Dokumenty\\ACecom\\Obrázky\"";

        private static async Task Main(string[] args)
        {
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

                using var client = new HttpClient();

                var orderDetail = new List<PohodaCreateOrder.orderOrderItem>();

                foreach (var item in order.items)
                {
                    var i = items.Find(e => e.ITEM_ID == item.itemId);

                    //bug chyba item quantity
                    data += $"<td>{item.itemId}</td><td>{i.EAN}</td><td><a href={i.URL}>link</a></td>";

                    var request = new PohodaGetStockRequest.dataPack()
                    {
                        version = 2.0m,
                        ico = 53870441,
                        note = "Export",
                        id = "zas001",
                        application = "StwTest",
                        dataPackItem = new PohodaGetStockRequest.dataPackDataPackItem()
                        {
                            id = "zas001",
                            version = 2.0m,
                            listStockRequest = new PohodaGetStockRequest.listStockRequest()
                            {
                                version = 2.0m,
                                stockVersion = 2.0m,
                                requestStock = new PohodaGetStockRequest.listStockRequestRequestStock()
                                {
                                    filter = new PohodaGetStockRequest.filter()
                                    {
                                        code = item.itemId.ToString()
                                    }
                                }
                            }
                        }
                    };

                    var response =
                        PohodaGetStockResponse.Deserialize(
                            await mServer.SendRequest(PohodaGetStockRequest.dataPack.Serialize(request)));

                    if (response.responsePackItem.listStock.stock is null)
                    {
                        Console.WriteLine("Couldn't find stock, creating new one");
                        var dataPack = new PohodaCreateStock.dataPack()
                        {
                            version = 2.0m,
                            ico = 53870441,
                            note = "Imported from xml",
                            id = "zas001",
                            application = "StwTest",
                        };

                        var pathToPicture = i.IMGURL.Split('/').Last();
                        Console.WriteLine($"Downloading picture{pathToPicture}");
                        try
                        {
                            var fileBytes = await client.GetByteArrayAsync(new Uri(i.IMGURL));
                            if (!File.Exists(path + pathToPicture))
                            {
                                await using var fs = File.Create(path + pathToPicture);
                                await fs.WriteAsync(fileBytes);
                                fs.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Failed to download image for {i.PRODUCTNAME}");
                        }

                        Console.WriteLine("Creating request");
                        var dataPackItem = new PohodaCreateStock.dataPackDataPackItem()
                        {
                            version = 2.0m,
                            id = "ZAS001",
                            stock = new PohodaCreateStock.stock()
                            {
                                version = 2.0m,
                                stockHeader = new PohodaCreateStock.stockStockHeader
                                {
                                    stockType = "card",
                                    code = i.ITEM_ID,
                                    EAN = i.EAN,
                                    PLU = 0,
                                    isSales = false,
                                    isInternet = true,
                                    isBatch = true,
                                    purchasingRateVAT = "high",
                                    sellingRateVAT = "high",
                                    name = i.PRODUCTNAME,
                                    nameComplement = "ISO 9001",
                                    unit = "ks",
                                    storage = new PohodaCreateStock.stockStockHeaderStorage()
                                    {
                                        ids = "Amazon"
                                    },
                                    typePrice = new PohodaCreateStock.stockStockHeaderTypePrice()
                                    {
                                        ids = "SK"
                                    },
                                    purchasingPrice = 0,
                                    sellingPrice = i.PRICE * 1.2m,
                                    limitMin = 0,
                                    limitMax = 1000,
                                    mass = i.WEIGHT,
                                    supplier = new PohodaCreateStock.stockStockHeaderSupplier()
                                    {
                                        id = 1
                                    },
                                    producer = i.MANUFACTURER,
                                    description = i.DESCRIPTION,
                                    pictures = new PohodaCreateStock.stockStockHeaderPictures()
                                    {
                                        picture = new PohodaCreateStock.stockStockHeaderPicturesPicture()
                                        {
                                            @default = true,
                                            description = "obrazok produktu",
                                            filepath = pathToPicture
                                        }
                                    },
                                    note = "Importovane z xml",
                                    relatedLinks = new PohodaCreateStock.stockStockHeaderRelatedLinks()
                                    {
                                        relatedLink = new PohodaCreateStock.stockStockHeaderRelatedLinksRelatedLink
                                        {
                                            addressURL = i.URL,
                                            description = "odkaz na produkt",
                                            order = 1
                                        }
                                    },
                                }
                            }
                        };

                        dataPack.dataPackItem = new[]
                        {
                            dataPackItem
                        };
                        Console.WriteLine("Sending request");
                        await mServer.SendRequest(PohodaCreateStock.dataPack.Serialize(dataPack));
                    }

                    var orderItem = new PohodaCreateOrder.orderOrderItem()
                    {
                        text = "nevim",
                        quantity = item.itemQuantity,
                        delivered = 0,
                        rateVAT = "high",
                        homeCurrency = new PohodaCreateOrder.orderOrderItemHomeCurrency()
                        {
                            unitPrice = item.itemPrice
                        }
                    };

                    var orderItem2 = new PohodaCreateOrder.orderOrderItem()
                    {
                        quantity = 1,
                        delivered = 0,
                        rateVAT = "high",
                        stockItem = new PohodaCreateOrder.orderOrderItemStockItem()
                        {
                            stockItem = new PohodaCreateOrder.stockItem()
                            {
                                EAN = i.EAN
                            }
                        }
                    };

                    orderDetail.Add(orderItem);
                    orderDetail.Add(orderItem2);
                }

                if (order.orderStatus == "Canceled")
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
                            orderHeader = new PohodaCreateOrder.orderOrderHeader()
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
                                        }
                                    }
                                },
                                paymentType = new PohodaCreateOrder.orderOrderHeaderPaymentType()
                                {
                                    ids = "Plat.kartou"
                                },
                                priceLevel = new PohodaCreateOrder.orderOrderHeaderPriceLevel()
                                {
                                    ids = "normal"
                                }
                            },
                            // not good solution what about more items at once can i even do it TODO: figure it out :D
                            orderDetail = orderDetail.ToArray(),
                            orderSummary = new PohodaCreateOrder.orderOrderSummary()
                            {
                                roundingDocument = "math2one"
                            }
                        }
                    }
                };

                await mServer.SendRequest(PohodaCreateOrder.dataPack.Serialize(orderDataPack));

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

                message = message.Replace("[[items]]", data);
            }

            mailMessage = mailMessage.Replace("[[DATA]]", message);

            Console.WriteLine("Sending mail");
            SendEmail(mailMessage);
            Console.WriteLine("Finished");
            Console.Read();
        }


        private static void SendEmail(string htmlString)
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient();
                message.From = new MailAddress("noreply@azetcool.com");
                message.To.Add(new MailAddress("jakkappro@gmail.com"));
                message.CC.Add(new MailAddress("pojtek@gmail.com"));
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