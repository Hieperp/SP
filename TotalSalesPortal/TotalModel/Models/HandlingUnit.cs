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
    
    public partial class HandlingUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HandlingUnit()
        {
            this.HandlingUnitDetails = new HashSet<HandlingUnitDetail>();
            this.GoodsDeliveryDetails = new HashSet<GoodsDeliveryDetail>();
        }
    
        public int HandlingUnitID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public Nullable<int> GoodsIssueID { get; set; }
        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }
        public int ApproverID { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalWeight { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public int Identification { get; set; }
        public decimal RealWeight { get; set; }
        public int PackingMaterialID { get; set; }
        public string Dimension { get; set; }
        public Nullable<int> GoodsDeliveryID { get; set; }
        public string ShippingAddress { get; set; }
    
        public virtual GoodsIssue GoodsIssue { get; set; }
        public virtual Location Location { get; set; }
        public virtual PackingMaterial PackingMaterial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HandlingUnitDetail> HandlingUnitDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsDeliveryDetail> GoodsDeliveryDetails { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
    }
}
