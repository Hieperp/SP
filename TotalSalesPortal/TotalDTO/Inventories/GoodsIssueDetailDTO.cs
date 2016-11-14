using System;
using System.Collections.Generic;
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

        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }

        public Nullable<int> GoodsIssueTypeID { get; set; }
        public string GoodsIssueTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/GoodsIssueType")]
        public string GoodsIssueTypeName { get; set; }
        public Nullable<int> GoodsIssueClassID { get; set; }

        [Display(Name = "SL ĐH")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "SL QT")]
        [UIHint("DecimalReadonly")]
        public decimal FreeQuantityRemains { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.Quantity != this.QuantityRemains || this.FreeQuantity != this.FreeQuantityRemains) && this.GoodsIssueTypeID == null) yield return new ValidationResult("Vui lòng chọn lý do không xuất kho [" + this.CommodityName + "]", new[] { "GoodsIssueTypeName" });
        }

    }
}