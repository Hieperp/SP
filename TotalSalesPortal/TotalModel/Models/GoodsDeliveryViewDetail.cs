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
    
    public partial class GoodsDeliveryViewDetail
    {
        public int GoodsDeliveryDetailID { get; set; }
        public int GoodsDeliveryID { get; set; }
        public int HandlingUnitID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public int Identification { get; set; }
        public string PrintedLabel { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal RealWeight { get; set; }
        public string Remarks { get; set; }
        public string ShippingAddress { get; set; }
    }
}
