using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsIssueRepository : IGenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>
    {
        ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID, int? goodsIssueID, string deliveryAdviceReference);
        ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID, int? goodsIssueID, string customerName);
    }

    public interface IGoodsIssueAPIRepository : IGenericAPIRepository
    {
    }
}
