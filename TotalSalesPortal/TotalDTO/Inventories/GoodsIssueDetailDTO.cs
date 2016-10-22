using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Sales;

namespace TotalDTO.Inventories
{
    public class GoodsIssueDetailDTO : SaleDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsIssueDetailID; }

        public int GoodsIssueDetailID { get; set; }
        public int GoodsIssueID { get; set; }

        [Display(Name = "SL ĐH")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }
    }
}    