using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsIssueRepository : GenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>, IGoodsIssueRepository
    {
        public GoodsIssueRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GoodsIssueEditable")
        {
        }

        public ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID, int? goodsIssueID, string deliveryAdviceReference)
        {
            return this.TotalSalesPortalEntities.GetPendingDeliveryAdvices(locationID, goodsIssueID, deliveryAdviceReference).ToList();
        }

        public ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID, int? goodsIssueID, string customerName)
        {
            return this.TotalSalesPortalEntities.GetPendingDeliveryAdviceCustomers(locationID, goodsIssueID, customerName).ToList();
        }
    }


    public class GoodsIssueAPIRepository : GenericAPIRepository, IGoodsIssueAPIRepository
    {
        public GoodsIssueAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetGoodsIssueIndexes")
        {
        }
    }
}
