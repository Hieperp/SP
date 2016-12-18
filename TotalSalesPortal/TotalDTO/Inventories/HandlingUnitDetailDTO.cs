using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class HandlingUnitDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.HandlingUnitDetailID; }

        public int HandlingUnitDetailID { get; set; }
        public int HandlingUnitID { get; set; }

        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int GoodsIssueID { get; set; }
        public int GoodsIssueDetailID { get; set; }


        [Display(Name = "SL ĐH")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }


        [Display(Name = "Đơn giá")]
        [UIHint("DecimalReadonly")] 
        public virtual decimal Weight { get; set; }

        [Display(Name = "Trọng lượng")]
        [UIHint("DecimalReadonly")]
        public decimal TotalWeight { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (Math.Round(this.Quantity * this.Weight, 0) != this.TotalWeight) yield return new ValidationResult("Lỗi trọng lượng", new[] { "TotalWeight" });
        }

    }
}
