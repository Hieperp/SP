using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IOrderInvoiceViewModel : ISupplierAutoCompleteViewModel
    {
        Nullable<int> DeliveryAdviceID { get; set; }
        string DeliveryAdviceReference { get; set; }
        Nullable<System.DateTime> DeliveryAdviceEntryDate { get; set; }
    }
}