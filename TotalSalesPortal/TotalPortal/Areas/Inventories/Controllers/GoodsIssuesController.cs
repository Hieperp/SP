using System.Collections.Generic;

using TotalModel.Models;

using TotalDTO.Inventories;
using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;


namespace TotalPortal.Areas.Inventories.Controllers
{
    public class GoodsIssuesController : GenericViewDetailController<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO, GoodsIssueViewModel>
    {
        private readonly IGoodsIssueService goodsIssueService;

        public GoodsIssuesController(IGoodsIssueService goodsIssueService, IGoodsIssueViewModelSelectListBuilder goodsIssueViewModelSelectListBuilder)
            : base(goodsIssueService, goodsIssueViewModelSelectListBuilder, true)
        {
            this.goodsIssueService = goodsIssueService;
        }


        protected override ICollection<GoodsIssueViewDetail> GetEntityViewDetails(GoodsIssueViewModel goodsIssueViewModel)
        {
            ICollection<GoodsIssueViewDetail> goodsIssueViewDetails = this.goodsIssueService.GetGoodsIssueViewDetails(goodsIssueViewModel.GoodsIssueID, goodsIssueViewModel.DeliveryAdviceID == null ? 0 : (int)goodsIssueViewModel.DeliveryAdviceID, goodsIssueViewModel.CustomerID, goodsIssueViewModel.ReceiverID, false);

            return goodsIssueViewDetails;
        }

        //protected override GoodsIssueViewModel TailorViewModel(GoodsIssueViewModel simpleViewModel)
        //{
        //    return base.TailorViewModel(simpleViewModel);

        //    if (simpleViewModel.Employee.EmployeeID == null && simpleViewModel.Employee.Name == null)
        //    {

        //    }
        //}
    }
}
