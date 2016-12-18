using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class HandlingUnitsController : GenericViewDetailController<HandlingUnit, HandlingUnitDetail, HandlingUnitViewDetail, HandlingUnitDTO, HandlingUnitPrimitiveDTO, HandlingUnitDetailDTO, HandlingUnitViewModel>
    {
        public HandlingUnitsController(IHandlingUnitService handlingUnitService, IHandlingUnitViewModelSelectListBuilder handlingUnitViewModelSelectListBuilder)
            : base(handlingUnitService, handlingUnitViewModelSelectListBuilder, true, true)
        {
        }

        public ActionResult PrintDetail(int? id)
        {
            return View(InitPrintViewModel(id));
        }

        public virtual ActionResult GetPendingGoodsIssueDetails()
        {
            return View();
        }
    }
}