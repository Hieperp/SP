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


        [Display(Name = "TL chuẩn")]
        [UIHint("DecimalReadonly")] 
        public virtual decimal UnitWeight { get; set; }

        [Display(Name = "Trọng lượng")]
        [UIHint("DecimalReadonly")]
        public decimal Weight { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (Math.Round(this.Quantity * this.UnitWeight, 0) != this.Weight) yield return new ValidationResult("Lỗi trọng lượng", new[] { "TotalWeight" });
        }

    }
}
