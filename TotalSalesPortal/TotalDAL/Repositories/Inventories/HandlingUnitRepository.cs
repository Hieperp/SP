using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class HandlingUnitRepository : GenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>, IHandlingUnitRepository
    {
        public HandlingUnitRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities) { }
    }


    public class HandlingUnitAPIRepository : GenericAPIRepository, IHandlingUnitAPIRepository
    {
        public HandlingUnitAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetHandlingUnitIndexes")
        {
        }

        public IEnumerable<HUPendingGoodsIssueCustomer> GetCustomers(int? locationID, int? handlingUnitID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HUPendingGoodsIssueCustomer> pendingGoodsIssueCustomers = base.TotalSalesPortalEntities.GetHUPendingGoodsIssueCustomers(locationID, handlingUnitID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueCustomers;
        }

        public IEnumerable<HUPendingGoodsIssue> GetGoodsIssues(int? locationID, int? handlingUnitID)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HUPendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetHUPendingGoodsIssues(locationID, handlingUnitID).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }


        public IEnumerable<HUPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string aspUserID, int? locationID, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HUPendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetHUPendingGoodsIssueDetails(handlingUnitID, goodsIssueID, customerID, receiverID, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
