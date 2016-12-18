﻿using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class HandlingUnitPrimitiveDTO : QuantityDTO<HandlingUnitDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.HandlingUnit; } }

        public int GetID() { return this.HandlingUnitID; }
        public void SetID(int id) { this.HandlingUnitID = id; }

        public int HandlingUnitID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int ReceiverID { get; set; }
        public virtual Nullable<int> GoodsIssueID { get; set; }


        [Display(Name = "Số thứ tự thùng, bao")]
        public string Identification { get; set; }
        [Display(Name = "Loại thùng, bao")]
        public int PackingMaterialID { get; set; }
        [Display(Name = "Quy cách, kích thước thùng, bao")]
        public string Dimension { get; set; }

        [Display(Name = "Tổng trọng lượng")]
        public decimal TotalWeight { get; set; }
        protected virtual decimal GetTotalWeight() { return this.DtoDetails().Select(o => o.Weight).Sum(); }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalWeight != this.GetTotalWeight()) yield return new ValidationResult("Lỗi tổng trọng lượng", new[] { "TotalWeight" });
        }
        

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; });
        }
    }

    public class HandlingUnitDTO : HandlingUnitPrimitiveDTO, IBaseDetailEntity<HandlingUnitDetailDTO>
    {
        public HandlingUnitDTO()
        {
            this.HandlingUnitViewDetails = new List<HandlingUnitDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override Nullable<int> GoodsIssueID { get { return (this.GoodsIssue != null ? (Nullable<int>)this.GoodsIssue.GoodsIssueID : null); } }
        [UIHint("Commons/GoodsIssueBox")]
        public GoodsIssueBoxDTO GoodsIssue { get; set; }

        public List<HandlingUnitDetailDTO> HandlingUnitViewDetails { get; set; }
        public List<HandlingUnitDetailDTO> ViewDetails { get { return this.HandlingUnitViewDetails; } set { this.HandlingUnitViewDetails = value; } }

        public ICollection<HandlingUnitDetailDTO> GetDetails() { return this.HandlingUnitViewDetails; }

        protected override IEnumerable<HandlingUnitDetailDTO> DtoDetails() { return this.HandlingUnitViewDetails; }
    }

}