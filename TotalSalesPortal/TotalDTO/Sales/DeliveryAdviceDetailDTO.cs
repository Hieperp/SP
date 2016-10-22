﻿using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalDTO.Helpers;

namespace TotalDTO.Sales
{
    public class StockableDetailDTO : DiscountVATAmountDetailDTO, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {                                     
        public Nullable<int> WarehouseID { get; set; }
        public int GetWarehouseID() { return (int)this.WarehouseID; } //Purpose: for IHelperWarehouseID only

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailable { get; set; }

        [GenericCompare(CompareToPropertyName = "QuantityAvailable", OperatorName = GenericCompareOperator.LessThanOrEqual, ErrorMessage = "Số lượng không được lớn hơn số lượng còn lại")]
        public override decimal Quantity { get; set; }        
    }

    public class SaleDetailDTO : StockableDetailDTO
    {
        public int DeliveryAdviceDetailID { get; set; }
        public int DeliveryAdviceID { get; set; }

        public int CustomerID { get; set; }
        public int EmployeeID { get; set; }

        public Nullable<bool> IsBonus { get; set; }
    }

    public class DeliveryAdviceDetailDTO : SaleDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.DeliveryAdviceDetailID; }

        public Nullable<int> SalesOrderDetailID { get; set; }
        public Nullable<int> PromotionID { get; set; }

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityName { get; set; }
    }
}