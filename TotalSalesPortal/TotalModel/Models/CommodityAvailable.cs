//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class CommodityAvailable
    {
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal VATPercent { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public Nullable<decimal> QuantityAvailable { get; set; }
        public Nullable<bool> Bookable { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
