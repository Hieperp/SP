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
            : base(handlingUnitService, handlingUnitViewModelSelectListBuilder, true)
        {
        }

        protected override HandlingUnitViewModel InitViewModelByCopy(HandlingUnitViewModel simpleViewModel)
        {
            return new HandlingUnitViewModel() { Customer = simpleViewModel.Customer, Receiver = simpleViewModel.Receiver, ShippingAddress = simpleViewModel.ShippingAddress, GoodsIssue = simpleViewModel.GoodsIssue, PackagingStaff = simpleViewModel.PackagingStaff, ConsignmentNo = simpleViewModel.ConsignmentNo };
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