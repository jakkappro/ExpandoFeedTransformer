using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpandoFeedTransformer.Factories.Pohoda
{
    public class PohodaGetStockRequestFactory
    {
        public static PohodaGetStockRequest.dataPack CreateRequest(ExpandoFeed.ordersOrderItem item)
        {
            return new PohodaGetStockRequest.dataPack()
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
        }
    }
}
