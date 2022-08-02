namespace ExpandoFeedTransformer.Factories.Pohoda;

public class PohodaCreateStockFactory
{
    public static async Task<PohodaCreateStock.dataPack> CreateRequest(PrehomeFeed.SHOPSHOPITEM i, string path,
        HttpClient client)
    {
        var dataPack = new PohodaCreateStock.dataPack
        {
            version = 2.0m,
            ico = 53870441,
            note = "Imported from xml",
            id = "zas001",
            application = "StwTest"
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
            Console.WriteLine(
                $"Failed to download image for {i.PRODUCTNAME}, path {path + pathToPicture}, message {e.Message}");
        }

        Console.WriteLine("Creating request");
        var dataPackItem = new PohodaCreateStock.dataPackDataPackItem
        {
            version = 2.0m,
            id = "ZAS001",
            stock = new PohodaCreateStock.stock
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
                    unit = "ks",
                    storage = new PohodaCreateStock.stockStockHeaderStorage
                    {
                        ids = "Amazon"
                    },
                    typePrice = new PohodaCreateStock.stockStockHeaderTypePrice
                    {
                        ids = "SK"
                    },
                    purchasingPrice = 0,
                    sellingPrice = i.PRICE * Program.SellingPriceCo,
                    limitMin = 0,
                    limitMax = 1000,
                    mass = i.WEIGHT,
                    supplier = new PohodaCreateStock.stockStockHeaderSupplier
                    {
                        id = 1
                    },
                    producer = i.MANUFACTURER,
                    description = i.DESCRIPTION,
                    pictures = new PohodaCreateStock.stockStockHeaderPictures
                    {
                        picture = new PohodaCreateStock.stockStockHeaderPicturesPicture
                        {
                            @default = true,
                            description = "obrazok produktu",
                            filepath = pathToPicture
                        }
                    },
                    note = "Importovane z xml",
                    relatedLinks = new PohodaCreateStock.stockStockHeaderRelatedLinks
                    {
                        relatedLink = new PohodaCreateStock.stockStockHeaderRelatedLinksRelatedLink
                        {
                            addressURL = i.URL,
                            description = "odkaz na produkt",
                            order = 1
                        }
                    }
                }
            }
        };

        dataPack.dataPackItem = new[]
        {
            dataPackItem
        };

        return dataPack;
    }
}