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
    using System.Collections.Generic;
    
    public partial class DeliveryAdviceDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryAdviceDetail()
        {
            this.GoodsIssueDetails = new HashSet<GoodsIssueDetail>();
        }
    
        public int DeliveryAdviceDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int DeliveryAdviceID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> SalesOrderDetailID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> PromotionID { get; set; }
        public int EmployeeID { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityIssue { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public Nullable<bool> IsBonus { get; set; }
        public string Remarks { get; set; }
        public decimal ControlFreeQuantity { get; set; }
        public decimal FreeQuantity { get; set; }
        public decimal FreeQuantityIssue { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public int ReceiverID { get; set; }
        public bool Approved { get; set; }
        public bool InActive { get; set; }
        public bool InActiveIssue { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual VoidType VoidType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssueDetail> GoodsIssueDetails { get; set; }
        public virtual DeliveryAdvice DeliveryAdvice { get; set; }
    }
}
