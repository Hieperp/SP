using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IHandlingUnitRepository : IGenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>
    {
    }

    public interface IHandlingUnitAPIRepository : IGenericAPIRepository
    {
        IEnumerable<HUPendingGoodsIssueCustomer> GetCustomers(int? locationID);
        IEnumerable<HUPendingGoodsIssue> GetGoodsIssues(int? locationID);

        IEnumerable<HUPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string shippingAddress, string goodsIssueDetailIDs, bool isReadonly);
    }
}
