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
        public IEnumerable<PendingGoodsIssue> GetPendingGoodsIssues(int goodsIssueID, string aspUserID, int locationID, DateTime fromDate, DateTime toDate, int accountInvoiceID, string goodsIssueDetailIDs)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssue> pendingGoodsIssues = base.TotalSalesPortalEntities.GetPendingGoodsIssues(goodsIssueID, aspUserID, locationID, fromDate, toDate, accountInvoiceID, goodsIssueDetailIDs).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }
    }

}
