using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Accounts;



namespace TotalDAL.Repositories.Accounts
{
    public class AccountInvoiceRepository : GenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>, IAccountInvoiceRepository
    {
        public AccountInvoiceRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities) { }
    }


    public class AccountInvoiceAPIRepository : GenericAPIRepository, IAccountInvoiceAPIRepository
    {
        public AccountInvoiceAPIRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities, "GetAccountInvoiceIndexes")
        {
        }
        public IEnumerable<PendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSalesPortalEntities.GetPendingGoodsIssueDetails(accountInvoiceID, goodsIssueID, customerID, commodityTypeID, aspUserID, locationID, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
