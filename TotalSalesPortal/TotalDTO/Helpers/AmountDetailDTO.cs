using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IAmountDetailDTO : IQuantityDetailDTO
    {
        int CalculatingTypeID { get; set; }

        decimal UnitPrice { get; set; }
        decimal Amount { get; set; }
    }

    public abstract class AmountDetailDTO : QuantityDetailDTO, IAmountDetailDTO
    {
        public int CalculatingTypeID { get; set; }

        [Display(Name = "Đơn giá")]
        [UIHint("DecimalReadonly")] //[UIHint("Decimal")]
        public virtual decimal UnitPrice { get; set; }

        [Display(Name = "Thành tiền")]
        [UIHint("DecimalReadonly")]
        public decimal Amount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (Math.Round(this.Quantity * this.UnitPrice, 0) != this.Amount) yield return new ValidationResult("Lỗi thành tiền", new[] { "Amount" });
        }
    }
}
