using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsIssueRepository : IGenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>
    {        
    }

    public interface IGoodsIssueAPIRepository : IGenericAPIRepository
    {
        ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID, int? goodsIssueID, string deliveryAdviceReference);
        ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID, int? goodsIssueID, string customerName);
    }
}
