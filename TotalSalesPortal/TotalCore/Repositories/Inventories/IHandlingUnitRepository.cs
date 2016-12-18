using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IHandlingUnitRepository : IGenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>
    {
    }

    public interface IHandlingUnitAPIRepository : IGenericAPIRepository
    {
        IEnumerable<HUPendingGoodsIssueCustomer> GetCustomers(int? locationID, int? handlingUnitID);
        IEnumerable<HUPendingGoodsIssue> GetGoodsIssues(int? locationID, int? handlingUnitID);

        IEnumerable<HUPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string aspUserID, int? locationID, string goodsIssueDetailIDs, bool isReadonly);
    }
}
