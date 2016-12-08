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
        IEnumerable<PendingGoodsIssue> GetPendingGoodsIssues(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly);
    }
}
