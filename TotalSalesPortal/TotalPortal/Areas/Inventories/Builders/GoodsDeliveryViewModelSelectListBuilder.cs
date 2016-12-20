using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Inventories;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsDeliveryViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsDeliveryViewModel>
    {
    }

    public class GoodsDeliveryViewModelSelectListBuilder : IGoodsDeliveryViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public GoodsDeliveryViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(GoodsDeliveryViewModel goodsDeliveryViewModel)
        {
            goodsDeliveryViewModel.AspNetUserSelectList = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), goodsDeliveryViewModel.UserID);
        }

    }

}