using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class GoodsIssueService : GenericWithViewDetailService<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO>, IGoodsIssueService
    {
        private readonly IGoodsIssueRepository goodsIssueRepository;

        public GoodsIssueService(IGoodsIssueRepository goodsIssueRepository)
            : base(goodsIssueRepository, "GoodsIssuePostSaveValidate", "GoodsIssueSaveRelative", null, null, null, "GetGoodsIssueViewDetails")
        {
            this.goodsIssueRepository = goodsIssueRepository;
        }

        public override ICollection<GoodsIssueViewDetail> GetViewDetails(int goodsIssueID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetGoodsIssueViewDetails instead.", "Purchase Invoice Service");
        }

        public ICollection<GoodsIssueViewDetail> GetGoodsIssueViewDetails(int goodsIssueID, int deliveryAdviceID, int customerID, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsIssueID", goodsIssueID), new ObjectParameter("DeliveryAdviceID", deliveryAdviceID), new ObjectParameter("CustomerID", customerID), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(GoodsIssueDTO goodsIssueDTO)
        {
            goodsIssueDTO.GoodsIssueViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsIssueDTO);
        }        
    }
}
