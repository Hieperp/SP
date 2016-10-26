using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalModel.Models;
using TotalDTO.Accounts;
using TotalCore.Repositories.Accounts;


using Microsoft.AspNet.Identity;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Accounts.APIs
{    
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ReceiptAPIsController : Controller
    {
        private readonly IReceiptRepository receiptRepository;
        private readonly IReceiptAPIRepository receiptAPIRepository;

        public ReceiptAPIsController(IReceiptRepository receiptRepository, IReceiptAPIRepository receiptAPIRepository)
        {
            this.receiptRepository = receiptRepository;
            this.receiptAPIRepository = receiptAPIRepository;
        }

        public JsonResult GetReceiptIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<ReceiptIndex> receiptIndexes = this.receiptAPIRepository.GetEntityIndexes<ReceiptIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = receiptIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoodsIssueReceivables([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? receiptID, string goodsIssueReference)
        {
            ICollection<GoodsIssueReceivable> ReceiptGetGoodsIssueReceivables = this.receiptRepository.GetGoodsIssueReceivables(locationID, receiptID, goodsIssueReference);
            return Json(ReceiptGetGoodsIssueReceivables.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerReceivables([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? receiptID, string customerName)
        {
            ICollection<CustomerReceivable> pendingGoodsIssueCustomers = this.receiptRepository.GetCustomerReceivables(locationID, receiptID, customerName);
            return Json(pendingGoodsIssueCustomers.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }


}