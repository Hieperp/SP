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
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.AccountInvoices = new HashSet<AccountInvoice>();
            this.Receipts = new HashSet<Receipt>();
            this.DeliveryAdvices = new HashSet<DeliveryAdvice>();
            this.GoodsIssues = new HashSet<GoodsIssue>();
            this.DeliveryAdvices1 = new HashSet<DeliveryAdvice>();
            this.DeliveryAdvices2 = new HashSet<DeliveryAdvice>();
            this.GoodsIssues1 = new HashSet<GoodsIssue>();
        }
    
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public int PriceCategoryID { get; set; }
        public int CustomerCategoryID { get; set; }
        public int CustomerTypeID { get; set; }
        public int TerritoryID { get; set; }
        public string AddressNo { get; set; }
        public string VATCode { get; set; }
        public string Telephone { get; set; }
        public string Facsimile { get; set; }
        public string AttentionName { get; set; }
        public string AttentionTitle { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<double> LimitAmount { get; set; }
        public string Remarks { get; set; }
        public bool InActive { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsSupplier { get; set; }
        public bool IsFemale { get; set; }
        public string Code { get; set; }
        public int EmployeeID { get; set; }
    
        public virtual EntireTerritory EntireTerritory { get; set; }
        public virtual PriceCategory PriceCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountInvoice> AccountInvoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssue> GoodsIssues { get; set; }
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdvice> DeliveryAdvices2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsIssue> GoodsIssues1 { get; set; }
    }
}
