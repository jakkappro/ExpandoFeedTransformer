using System.Net;
using System.Text;
using ExpandoFeedTransformer.Factories.Pohoda;
using ExpandoFeedTransformer.Services;

namespace ExpandoFeedTransformer;

public static class Program
{
    private const string Path = "\\\\AzetCool-Pohoda\\POHODA_SK_E1_DATA\\Dokumenty\\ACecom\\Obrázky\\";
    public const decimal SellingPriceCo = 1.25m;

    private static async Task Main(string[] args)
    {
        var apiPassword = "2d20b17664867cd0fe1dc38c0195ac36";
        var httpClient = new HttpClient { BaseAddress = new Uri("https://www.zasilkovna.cz/api/rest/") };

        //var orderIds = new List<long>();

        if (args.Length == 0 || !int.TryParse(args[0], out var days)) 
            days = 1;

        var line = "001";
        try
        {
            var sr = new StreamReader("index.txt");
            line = await sr.ReadLineAsync();
            sr.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        var orderNumber = ulong.Parse($"3{DateTime.Now:yyMMdd}{line}");

        Console.WriteLine("Getting expando feed");

        var expandoOrders = await GetExpandoOrders(days);

        Console.WriteLine("Getting prehome feed");

        var prehomeArticles = await GetPrehomeFeed();
        AddMissingItems(prehomeArticles);

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
                                                    <td>PohodaId</td>
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
                                                            <td>link</td>
                                                            <td>dealer</td>
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
                    <td>[[PohodaId]]</td>
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
                                <td><a href=""[[URL]]"">link</a></td>
                                <td>[[DEALER]]</td>";
        #endregion

        var mail = new MailService("noreply@azetcool.com", "jakkappro@gmail.com", "pojtek@gmail.com",
            "W6&2bB9;T?ukTk4");

        mail.CreateTemplate(mailMessage);
        mail.CreateRowTemplate(row);
        mail.CreateItemTemplate(itemTemplate);

        Console.WriteLine("Creating orders");

        var mailOnlyMode = args.Length > 1 && args[1] == "mail";

        if (mailOnlyMode)
        {
            foreach (var order in expandoOrders.order)
            {
                var shopItems = order.items
                    .Select(item => prehomeArticles.FirstOrDefault(e => e.ITEM_ID == item.itemId))
                    .Where(article => article != null).ToList();
                mail.AddNoPohodaRow(order, shopItems);
            }
        }
        else
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

            var ordersFilter = new PohodaGetOrdersByDateRequest.dataPack
            {
                dataPackItem = new PohodaGetOrdersByDateRequest.dataPackDataPackItem
                {
                    listOrderRequest = new PohodaGetOrdersByDateRequest.listOrderRequest
                    {
                        requestOrder = new PohodaGetOrdersByDateRequest.listOrderRequestRequestOrder
                        {
                            filter = new PohodaGetOrdersByDateRequest.filter
                            {
                                dateFrom = (DateTime.Today - TimeSpan.FromDays(days + 1)).ToString("yyyy-MM-dd"),
                                dateTill = DateTime.Now.ToString("yyyy-MM-dd")
                            }
                        },
                        version = 2.0m,
                        orderType = "receivedOrder",
                        orderVersion = 2.0m
                    },
                    id = "li1",
                    version = 2.0m
                },
                id = 1,
                ico = 53870441,
                application = "SWTTest",
                version = 2.0m,
                note = "Export objednavok"
            };

            Console.WriteLine("From: " + (DateTime.Today - TimeSpan.FromDays(days + 1)).ToString("yyyy-MM-dd"));
            Console.WriteLine("To: " + DateTime.Now.ToString("yyyy-MM-dd"));

            var res = await mServer.SendRequest(PohodaGetOrdersByDateRequest.dataPack.Serialize(ordersFilter));

            var existingOrders =
                PohodaGetOrdersByDateResponse.Deserialize(res);

            var existingOrdersList = new List<PohodaGetOrdersByDateResponse.listOrderOrder>();

            if (existingOrders.responsePackItem.listOrder.order is not null)
                existingOrdersList = existingOrders.responsePackItem.listOrder.order.ToList();

            foreach (var order in expandoOrders.order)
            {
                var exists = existingOrdersList.Any(existingOrder =>
                    existingOrder.orderHeader.numberOrder == order.orderId);

                if (exists)
                {
                    Console.WriteLine("Order {0} already exists", order.orderId);
                    continue;
                }

                if (order.orderStatus == "Canceled")
                {
                    // update order to canceled
                    Console.WriteLine($"Skipping order {order.orderId} with status {order.orderStatus}.");
                    continue;
                }

                Console.WriteLine($"Trying to create order {order.orderId}.");
                using var client = new HttpClient();

                var orderDetail = new List<PohodaCreateOrder.orderOrderItem>();

                var shopItems = new List<PrehomeFeed.SHOPSHOPITEM>();

                foreach (var item in order.items)
                {
                    Console.WriteLine($"Trying to get stock EAN: {item.itemId}");
                    var article = prehomeArticles.FirstOrDefault(e => e.ITEM_ID == item.itemId);

                    if (article == null)
                        continue;

                    Console.WriteLine($"Found stock in prehome feed: {article.URL}");
                    shopItems.Add(article);
                    var request = PohodaGetStockRequestFactory.CreateRequest(item);

                    var response =
                        PohodaGetStockResponse.Deserialize(
                            await mServer.SendRequest(PohodaGetStockRequest.dataPack.Serialize(request)));

                    if (response.responsePackItem.listStock.stock is null)
                    {
                        Console.WriteLine("Couldn't find stock, creating new one");
                        var dataPack = await PohodaCreateStockFactory.CreateRequest(article, Path, client);
                        Console.WriteLine("Sending request");
                        Console.WriteLine(PohodaCreateStock.dataPack.Serialize(dataPack) + "\n");
                        await mServer.SendRequest(PohodaCreateStock.dataPack.Serialize(dataPack));
                    }
                    else
                    {
                        Console.WriteLine($"Found stock in pohoda SKU: {article.ITEM_ID}");
                    }

                    // shipping
                    var orderItem = new PohodaCreateOrder.orderOrderItem
                    {
                        text = "Doprava",
                        quantity = 1,
                        delivered = 0,
                        rateVAT = "high",
                        homeCurrency = new PohodaCreateOrder.orderOrderItemHomeCurrency
                        {
                            unitPrice = 5.0m
                        },
                        typeServiceMOSS = new PohodaCreateOrder.orderOrderItemTypeServiceMOSS
                        {
                            ids = "GD"
                        },
                        payVAT = true
                    };

                    // order item
                    var orderItem2 = new PohodaCreateOrder.orderOrderItem
                    {
                        quantity = item.itemQuantity,
                        delivered = 0,
                        rateVAT = "high",
                        stockItem = new PohodaCreateOrder.orderOrderItemStockItem
                        {
                            stockItem = new PohodaCreateOrder.stockItem
                            {
                                EAN = article.EAN
                            }
                        },
                        homeCurrency = new PohodaCreateOrder.orderOrderItemHomeCurrency
                        {
                            unitPrice = item.itemPrice * SellingPriceCo
                        },
                        typeServiceMOSS = new PohodaCreateOrder.orderOrderItemTypeServiceMOSS
                        {
                            ids = "GD"
                        },
                        payVAT = true
                    };

                    orderDetail.Add(orderItem);
                    orderDetail.Add(orderItem2);
                }

                Console.WriteLine($"Creating order {order.orderId}");
                var orderDataPack = new PohodaCreateOrder.dataPack
                {
                    version = 2.0m,
                    ico = 53870441,
                    note = "Export",
                    id = "zas001",
                    application = "StwTest",
                    dataPackItem = new PohodaCreateOrder.dataPackDataPackItem
                    {
                        id = "zas001",
                        version = 2.0m,
                        order = new PohodaCreateOrder.order
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
                                partnerIdentity = new PohodaCreateOrder.orderOrderHeaderPartnerIdentity
                                {
                                    address = new PohodaCreateOrder.address
                                    {
                                        name = order.customer.firstname + " " + order.customer.surname,
                                        city = order.customer.address.city,
                                        street = order.customer.address.address1,
                                        zip = order.customer.address.zip,
                                        country = new PohodaCreateOrder.addressCountry
                                        {
                                            ids = order.customer.address.country
                                        },
                                        company = order.customer.companyName ?? (order.customer.companyName is "-"
                                            ? null
                                            : order.customer.companyName),
                                        dic = "",
                                        division = "",
                                        email = order.customer.email,
                                        mobilPhone = order.customer.phone
                                    }
                                },
                                paymentType = new PohodaCreateOrder.orderOrderHeaderPaymentType
                                {
                                    ids = "Plat.kartou"
                                },
                                priceLevel = new PohodaCreateOrder.orderOrderHeaderPriceLevel
                                {
                                    ids = order.customer.address.country == "DE"
                                        ? "DE"
                                        : "Predajná"
                                },
                                MOSS = new PohodaCreateOrder.orderOrderHeaderMOSS
                                {
                                    ids = order.customer.address.country == "DE"
                                        ? "DE"
                                        : "AT"
                                },
                                evidentiaryResourcesMOSS =
                                    new PohodaCreateOrder.orderOrderHeaderEvidentiaryResourcesMOSS
                                    {
                                        ids = "A"
                                    },
                                number = new PohodaCreateOrder.orderOrderHeaderNumber
                                {
                                    numberRequested = orderNumber.ToString()
                                },
                                isExecuted = false,
                                carrier = new PohodaCreateOrder.orderOrderHeaderCarrier
                                {
                                    ids = order.customer.address.country == "DE"
                                        ? "Doprava DE"
                                        : "Doprava AT"
                                },
                                isDelivered = false
                            },
                            orderDetail = orderDetail.ToArray(),
                            orderSummary = new PohodaCreateOrder.orderOrderSummary
                            {
                                roundingDocument = "none"
                            }
                        }
                    }
                };

                //var fixedAddress = order.customer.address.address1.Replace("ß", "ss");

                // var packetaOrder = new PacketaCreateOrder.createPacket()
                // {
                //     apiPassword = apiPassword,
                //     packetAttributes = new PacketaCreateOrder.createPacketPacketAttributes
                //     {
                //         number = orderNumber.ToString(),
                //         name = order.customer.firstname,
                //         surname = order.customer.surname,
                //         email = order.customer.email,
                //         addressId = (uint)(order.customer.address.country == "DE" ? 13613 : 80),
                //         value = order.totalPrice,
                //         eshop = "AzetCool",
                //         weight = 0.99m,
                //         sender_id = 331585, 
                //         phone = order.customer.phone,
                //         zip = order.customer.address.zip,
                //         street = fixedAddress,
                //         houseNumber = string.IsNullOrWhiteSpace(order.customer.address.address2)
                //             ? ""
                //             : order.customer.address.address2,
                //         city = order.customer.address.city,
                //         security = new PacketaCreateOrder.createPacketPacketAttributesSecurity()
                //         {
                //             allowPublicTracking = 1
                //         }
                //     }
                // };
                //
                //
                //
                // var l = PacketaCreateOrderResponse.Deserialize(await (await httpClient.PostAsync("",
                //         new ByteArrayContent(
                //             Encoding.ASCII.GetBytes(PacketaCreateOrder.createPacket.Serialize(packetaOrder))))).Content
                //     .ReadAsStringAsync());
                //
                // if (l.result is not null)
                // {
                //     Console.WriteLine(l.result);
                // }

                await Task.Delay(1500);

                await mServer.SendRequest(PohodaCreateOrder.dataPack.Serialize(orderDataPack));

                shopItems.ForEach(e => Console.WriteLine($"Adding item {e.ITEM_ID} to mail."));
                mail.AddRow(order, shopItems, orderNumber.ToString());

                orderNumber += 1;
            }
        }
        // foreach (var p in orderIds.Select(orderId => new PacketaGenerateLabel.packetLabelPdf()
        //          {
        //              apiPassword = apiPassword,
        //              format = "A6 on A4",
        //              offset = 0,
        //              packetId = orderId
        //          }))
        // {
        //     var pdf = PacketaGenerateLabelResponse.Deserialize(await (await httpClient.PostAsync("",
        //             new ByteArrayContent(
        //                 Encoding.ASCII.GetBytes(PacketaGenerateLabel.packetLabelPdf.Serialize(p)))))
        //         .Content.ReadAsStringAsync());
        //
        //     await using var stream = GenerateStreamFromString(pdf.result);
        //     var buffer = new byte[stream.Length];
        //     stream.Read(buffer, 0, buffer.Length);
        //     await File.WriteAllBytesAsync($"{p.packetId}.pdf", buffer);
        // }


        Console.WriteLine("Sending mail");
        mail.PopulateTemplate();
        mail.SendMail();
        Console.WriteLine("Finished");
        Console.Read();

        // mServer.StopServer();

        if (mailOnlyMode)
            return;

        try
        {
            var s = orderNumber.ToString().Substring(orderNumber.ToString().Length - 3);
            var write = s.Length >= 3 ? s : s.Length == 2 ? "0" + s : "00" + s;
            var sw = new StreamWriter("index.txt", false, Encoding.ASCII);
            await sw.WriteAsync(write);
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static async Task<List<PrehomeFeed.SHOPSHOPITEM>> GetPrehomeFeed()
    {
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.prehome.sk/feed/amazon.xml")
        };
        using var response = await httpClient.GetAsync("");
        if (response.StatusCode != HttpStatusCode.OK)
            throw new ArgumentException("Couldn't get feed");

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
            throw new ArgumentException("Couldn't get feed");

        return ExpandoFeed.Deserialize(await response.Content.ReadAsStringAsync());
    }

    private static void AddMissingItems(List<PrehomeFeed.SHOPSHOPITEM> items)
    {
        if (!items.Exists(i => i.ITEM_ID == 294489))
            items.Add(
                new PrehomeFeed.SHOPSHOPITEM
                {
                    ITEM_ID = 294489,
                    PRODUCTNAME = "Intex 69629 Skladacie vesla 218cm",
                    EAN = "6941057417837",
                    IMGURL = "",
                    PRICE_VAT = 15.47m * SellingPriceCo,
                    VAT = 20
                });
    }
}