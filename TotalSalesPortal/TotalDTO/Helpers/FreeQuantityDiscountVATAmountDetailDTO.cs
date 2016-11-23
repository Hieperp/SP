﻿using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Helpers
{
    public interface IFreeQuantityDiscountVATAmountDetailDTO : IDiscountVATAmountDetailDTO
    {
        decimal ControlFreeQuantity { get; set; }
        decimal FreeQuantity { get; set; }
    }

    public abstract class FreeQuantityDiscountVATAmountDetailDTO : DiscountVATAmountDetailDTO, IFreeQuantityDiscountVATAmountDetailDTO
    {
        [Display(Name = "SL yêu cầu để được hưởng Quà tặng")]
        [UIHint("DecimalReadonly")]
        public decimal ControlFreeQuantity { get; set; }

        [Display(Name = "QT")]        
        public virtual decimal FreeQuantity { get; set; }
    }
}
