using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IListedAmountDiscountVATAmountDetailDTO : IDiscountVATAmountDetailDTO
    {
        decimal ListedGrossPrice { get; set; }

        decimal ListedAmount { get; set; }
        decimal ListedVATAmount { get; set; }
        decimal ListedGrossAmount { get; set; }
    }

    public abstract class ListedAmountDiscountVATAmountDetailDTO : DiscountVATAmountDetailDTO, IListedAmountDiscountVATAmountDetailDTO
    {
        [Display(Name = "Giá sau thuế")]
        [UIHint("DecimalReadonly")] //[UIHint("Decimal")]
        public virtual decimal ListedGrossPrice { get; set; }

        [Display(Name = "Thành tiền")]
        [UIHint("DecimalReadonly")]
        public decimal ListedAmount { get; set; }

        [Display(Name = "Thuế VAT")]
        [UIHint("DecimalReadonly")]
        public decimal ListedVATAmount { get; set; }

        [Display(Name = "Tổng cộng")]
        [UIHint("DecimalReadonly")]
        public decimal ListedGrossAmount { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if ((this.ListedPrice != 0 && this.ListedGrossPrice == 0) || (this.ListedPrice == 0 && this.ListedGrossPrice != 0)) yield return new ValidationResult("Lỗi giá gốc sau thuế", new[] { "ListedGrossPrice" });
            
            if (Math.Round(this.Quantity * this.ListedPrice, 0) != this.ListedAmount) yield return new ValidationResult("Lỗi thành tiền giá gốc", new[] { "ListedAmount" });
            if (Math.Round(this.Quantity * this.ListedGrossPrice, 0) != this.ListedGrossAmount) yield return new ValidationResult("Lỗi thành tiền giá gốc sau thuế", new[] { "ListedGrossAmount" });
            if ((this.ListedAmount == 0 && this.ListedVATAmount != 0) || (this.ListedAmount != 0 && this.VATPercent != 0 && this.ListedVATAmount == 0) || (this.ListedAmount != 0 && this.VATPercent == 0 && this.ListedVATAmount != 0)) yield return new ValidationResult("Lỗi tiền thuế giá gốc", new[] { "ListedVATAmount" });
            if (Math.Round(this.ListedAmount + this.ListedVATAmount, 0) != this.ListedGrossAmount) yield return new ValidationResult("Lỗi thành tiền giá gốc sau thuế", new[] { "ListedGrossAmount" });

        }
    }
}