using TotalDTO.Commons;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ISupplierAutoCompleteViewModel
    {
        int CustomerID { get; set; }
        CustomerBaseDTO Customer { get; set; }
    }
}
