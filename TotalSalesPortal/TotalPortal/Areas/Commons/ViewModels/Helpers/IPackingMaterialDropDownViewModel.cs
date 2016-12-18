using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IPackingMaterialDropDownViewModel
    {
        [Display(Name = "Phương thức TT")]
        int PackingMaterialID { get; set; }
        IEnumerable<SelectListItem> PackingMaterialSelectList { get; set; }
    }
}

