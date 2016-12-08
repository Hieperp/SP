using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Accounts;

using TotalCore.Repositories.Accounts;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Accounts.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class AccountInvoiceApiController : Controller
    {
        private readonly IAccountInvoiceAPIRepository accountInvoiceAPIRepository;

        public AccountInvoiceApiController(IAccountInvoiceAPIRepository accountInvoiceAPIRepository)
        {
            this.accountInvoiceAPIRepository = accountInvoiceAPIRepository;
        }

        public JsonResult GetAccountInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<AccountInvoiceIndex> accountInvoiceIndexes = this.accountInvoiceAPIRepository.GetEntityIndexes<AccountInvoiceIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = accountInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingGoodsIssueDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? accountInvoiceID, int? goodsIssueID, int? customerID, int? commodityTypeID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            var result = this.accountInvoiceAPIRepository.GetPendingGoodsIssueDetails(accountInvoiceID, goodsIssueID, customerID, commodityTypeID, User.Identity.GetUserId(), locationID, fromDate, toDate.AddHours(23).AddMinutes(59).AddSeconds(59), goodsIssueDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


    }

}