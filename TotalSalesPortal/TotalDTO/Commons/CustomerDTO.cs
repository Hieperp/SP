using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface ICustomerBaseDTO
    {
        int CustomerID { get; set; }
        string Name { get; set; }
        string OfficialName { get; set; }
        Nullable<System.DateTime> Birthday { get; set; }
        string VATCode { get; set; }
        string Telephone { get; set; }
        string AddressNo { get; set; }
        int TerritoryID { get; set; }
        string EntireTerritoryEntireName { get; set; }
        int PriceCategoryID { get; set; }
        string PriceCategoryCode { get; set; }
    }

    public class CustomerBaseDTO : BaseDTO, ICustomerBaseDTO
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [Display(Name = "Khách hàng")]
        public string Name { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string OfficialName { get; set; }

        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Display(Name = "Mã số thuế")]
        public string VATCode { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        public string Telephone { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string AddressNo { get; set; }

        [Required]
        [Display(Name = "Khu vực")]
        public int TerritoryID { get; set; }
        [Required]
        [Display(Name = "Khu vực")]
        public string EntireTerritoryEntireName { get; set; }

        [Required]
        [Display(Name = "Bảng giá")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "Bảng giá")]
        public string PriceCategoryCode { get; set; }
    }


    public class CustomerPrimitiveDTO : CustomerBaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Customer; } }

        public int GetID() { return this.CustomerID; }
        public void SetID(int id) { this.CustomerID = id; }

        //public int CustomerID { get; set; }
        //[Required]
        //[Display(Name = "Tên khách")]
        //public string Name { get; set; }
        //[Display(Name = "Tên đầy đủ")]
        //public string OfficialName { get; set; }
        [Display(Name = "Phân khúc khách hàng")]
        [DefaultValue(1)]
        public int CustomerCategoryID { get; set; }
        [Display(Name = "Phân loại khách hàng")]
        [DefaultValue(1)]
        public int CustomerTypeID { get; set; }
        //[Display(Name = "Khu vực")]
        //public int TerritoryID { get; set; }
        //[Display(Name = "Khu vực")]
        //[Required]
        //public string EntireTerritoryEntireName { get; set; }
        //[Display(Name = "Địa chỉ")]
        //[Required]
        //public string AddressNo { get; set; }
        //[Display(Name = "Mã số thuế")]
        //public string VATCode { get; set; }
        //[Display(Name = "Điện thoại")]
        //[Required]
        //public string Telephone { get; set; }
        public string Facsimile { get; set; }
        [Display(Name = "Người liên hệ")]
        public string AttentionName { get; set; }
        [Display(Name = "Chức danh")]
        public string AttentionTitle { get; set; }
        //[Required]
        //[Display(Name = "Ngày sinh")]
        //public Nullable<System.DateTime> Birthday { get; set; }
        [Display(Name = "Hạn mức tín dụng")]
        public Nullable<double> LimitAmount { get; set; }

        [Display(Name = "Khách hàng")]
        public bool IsCustomer { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public bool IsSupplier { get; set; }
        [Display(Name = "Giới tính nữ")]
        public bool IsFemale { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }

    public class CustomerDTO : CustomerPrimitiveDTO
    {
    }
}
