using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;

namespace TotalDTO.Accounts
{
    public class AccountInvoiceDetailDTO : DiscountVATAmountDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.AccountInvoiceDetailID; }

        public int AccountInvoiceDetailID { get; set; }
        public int AccountInvoiceID { get; set; }

        public int CustomerID { get; set; }
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id hóa đơn bán hàng")]
        public int GoodsIssueDetailID { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal Quantity { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal DiscountPercent { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal UnitPrice { get; set; }
        [UIHint("DecimalReadonly")]
        public override decimal GrossPrice { get; set; }


        public Nullable<bool> IsBonus { get; set; }        
    }
}
