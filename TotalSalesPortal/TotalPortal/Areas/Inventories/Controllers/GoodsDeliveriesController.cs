using System.Web.Mvc;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class GoodsDeliveriesController : GenericViewDetailController<GoodsDelivery, GoodsDeliveryDetail, GoodsDeliveryViewDetail, GoodsDeliveryDTO, GoodsDeliveryPrimitiveDTO, GoodsDeliveryDetailDTO, GoodsDeliveryViewModel>
    {
        public GoodsDeliveriesController(IGoodsDeliveryService goodsDeliveryService, IGoodsDeliveryViewModelSelectListBuilder goodsDeliveryViewModelSelectListBuilder)
            : base(goodsDeliveryService, goodsDeliveryViewModelSelectListBuilder, true)
        {
        }

        public virtual ActionResult GetPendingHandlingUnits()
        {
            return View();
        }
    }
}