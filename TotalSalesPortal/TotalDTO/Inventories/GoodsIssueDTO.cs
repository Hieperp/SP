using System;
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
    public class GoodsIssuePrimitiveDTO : FreeQuantityDiscountVATAmountDTO<GoodsIssueDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsIssue; } }

        public int GetID() { return this.GoodsIssueID; }
        public void SetID(int id) { this.GoodsIssueID = id; }

        public int GoodsIssueID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int ReceiverID { get; set; }

        public Nullable<int> DeliveryAdviceID { get; set; }
        [Display(Name = "Đơn đặt hàng")]
        public string DeliveryAdviceReference { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> DeliveryAdviceEntryDate { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public virtual int EmployeeID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ReceiverID = this.ReceiverID; e.EmployeeID = this.EmployeeID; });
        }
    }


    public class GoodsIssueDTO : GoodsIssuePrimitiveDTO, IBaseDetailEntity<GoodsIssueDetailDTO>
    {
        public GoodsIssueDTO()
        {
            this.GoodsIssueViewDetails = new List<GoodsIssueDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public override int EmployeeID { get { return (this.Employee != null ? this.Employee.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Employee { get; set; }

        public List<GoodsIssueDetailDTO> GoodsIssueViewDetails { get; set; }
        public List<GoodsIssueDetailDTO> ViewDetails { get { return this.GoodsIssueViewDetails; } set { this.GoodsIssueViewDetails = value; } }

        public ICollection<GoodsIssueDetailDTO> GetDetails() { return this.GoodsIssueViewDetails; }

        protected override IEnumerable<GoodsIssueDetailDTO> DtoDetails() { return this.GoodsIssueViewDetails; }
    }


}



