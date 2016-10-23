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

namespace TotalDTO.Accounts
{
    public class AccountInvoicePrimitiveDTO : DiscountVATAmountDTO<AccountInvoiceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.AccountInvoice; } }

        public int GetID() { return this.AccountInvoiceID; }
        public void SetID(int id) { this.AccountInvoiceID = id; }

        public int AccountInvoiceID { get; set; }

        public virtual int CustomerID { get; set; }

        [Display(Name = "Số hóa đơn")]
        [Required(ErrorMessage = "Vui lòng nhập Số hóa đơn")]
        public string VATInvoiceNo { get; set; }
        [Display(Name = "Số seri")]
        [Required(ErrorMessage = "Vui lòng nhập Số seri")]
        public string VATInvoiceSeries { get; set; }
        [Display(Name = "Ngày hóa đơn")]
        [Required(ErrorMessage = "Vui lòng Ngày hóa đơn")]
        public Nullable<System.DateTime> VATInvoiceDate { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; });
        }
    }

    public class AccountInvoiceDTO : AccountInvoicePrimitiveDTO, IBaseDetailEntity<AccountInvoiceDetailDTO>
    {
        public AccountInvoiceDTO()
        {
            this.AccountInvoiceViewDetails = new List<AccountInvoiceDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }


        public List<AccountInvoiceDetailDTO> AccountInvoiceViewDetails { get; set; }
        public List<AccountInvoiceDetailDTO> ViewDetails { get { return this.AccountInvoiceViewDetails; } set { this.AccountInvoiceViewDetails = value; } }

        public ICollection<AccountInvoiceDetailDTO> GetDetails() { return this.AccountInvoiceViewDetails; }

        protected override IEnumerable<AccountInvoiceDetailDTO> DtoDetails() { return this.AccountInvoiceViewDetails; }
    }

}
