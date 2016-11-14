using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;

namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsIssueTypeAPIsController : Controller
    {
        private readonly IGoodsIssueTypeRepository goodsIssueTypeRepository;

        public GoodsIssueTypeAPIsController(IGoodsIssueTypeRepository goodsIssueTypeRepository)
        {
            this.goodsIssueTypeRepository = goodsIssueTypeRepository;
        }


        public JsonResult SearchGoodsIssueTypes(string searchText)
        {
            var result = goodsIssueTypeRepository.SearchGoodsIssueTypes(searchText).Select(s => new { s.GoodsIssueTypeID, s.Code, s.Name, s.GoodsIssueClassID });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}