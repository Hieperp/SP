using System;
using System.Linq;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;


namespace TotalCore.Repositories.Accounts
{
    public interface IAccountInvoiceRepository : IGenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>
    {
    }

    public interface IAccountInvoiceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingGoodsIssue> GetPendingGoodsIssues(int goodsIssueID, string aspUserID, int locationID, int commodityTypeID, DateTime fromDate, DateTime toDate, int accountInvoiceID, string goodsIssueDetailIDs);
    }
}
