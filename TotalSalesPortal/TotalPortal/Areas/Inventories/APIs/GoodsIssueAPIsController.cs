using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;


using Microsoft.AspNet.Identity;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsIssueAPIsController : Controller
    {
        private readonly IGoodsIssueAPIRepository goodsIssueAPIRepository;

        public GoodsIssueAPIsController(IGoodsIssueAPIRepository goodsIssueAPIRepository)
        {
            this.goodsIssueAPIRepository = goodsIssueAPIRepository;
        }

        public JsonResult GetGoodsIssueIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<GoodsIssueIndex> goodsIssueIndexes = this.goodsIssueAPIRepository.GetEntityIndexes<GoodsIssueIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = goodsIssueIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDeliveryAdvices([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? goodsIssueID, string searchText)
        {
            ICollection<PendingDeliveryAdvice> GoodsIssueGetDeliveryAdvices = this.goodsIssueAPIRepository.GetDeliveryAdvices(locationID, goodsIssueID, searchText);
            return Json(GoodsIssueGetDeliveryAdvices.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? goodsIssueID, string searchText)
        {
            ICollection<PendingDeliveryAdviceCustomer> pendingDeliveryAdviceCustomers = this.goodsIssueAPIRepository.GetCustomers(locationID, goodsIssueID, searchText);
            return Json(pendingDeliveryAdviceCustomers.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }


}