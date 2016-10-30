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

namespace TotalDTO.Sales
{
    public class DeliveryAdvicePrimitiveDTO : DiscountVATAmountDTO<DeliveryAdviceDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.DeliveryAdvice; } }

        public int GetID() { return this.DeliveryAdviceID; }
        public void SetID(int id) { this.DeliveryAdviceID = id; }

        public int DeliveryAdviceID { get; set; }

        public virtual int CustomerID { get; set; }
        
        [Required]
        [Display(Name = "Bảng giá")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "Bảng giá")]
        public string PriceCategoryName { get; set; }

        [Display(Name = "Phương thức TT")]
        public int PaymentTermID { get; set; }

        public Nullable<int> SalesOrderID { get; set; }
        [Display(Name = "Đơn đặt hàng")]
        public string SalesOrderReference { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> SalesOrderEntryDate { get; set; }

        public virtual Nullable<int> PromotionID { get; set; }
        [Display(Name = "Chứng từ khuyến mãi")]
        [Required(ErrorMessage = "Nhập chứng từ khuyến mãi")]
        public string PromotionVouchers { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public virtual int EmployeeID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.PromotionID = this.PromotionID; e.EmployeeID = this.EmployeeID; });
        }
    }


    public class DeliveryAdviceDTO : DeliveryAdvicePrimitiveDTO, IBaseDetailEntity<DeliveryAdviceDetailDTO>
    {
        public DeliveryAdviceDTO()
        {
            this.DeliveryAdviceViewDetails = new List<DeliveryAdviceDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> PromotionID { get { return (this.Promotion != null ? this.Promotion.PromotionID : null); } }
        [UIHint("Commons/Promotion")]
        public PromotionDTO Promotion { get; set; }

        public override int EmployeeID { get { return (this.Employee != null ? this.Employee.EmployeeID : 0); } }
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Employee { get; set; }

        public List<DeliveryAdviceDetailDTO> DeliveryAdviceViewDetails { get; set; }
        public List<DeliveryAdviceDetailDTO> ViewDetails { get { return this.DeliveryAdviceViewDetails; } set { this.DeliveryAdviceViewDetails = value; } }

        public ICollection<DeliveryAdviceDetailDTO> GetDetails() { return this.DeliveryAdviceViewDetails; }

        protected override IEnumerable<DeliveryAdviceDetailDTO> DtoDetails() { return this.DeliveryAdviceViewDetails; }
    }

}
