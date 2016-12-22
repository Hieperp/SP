using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsDeliveryDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsDeliveryDetailID; }

        public int GoodsDeliveryDetailID { get; set; }
        public int GoodsDeliveryID { get; set; }

        public int HandlingUnitID { get; set; }

        public override int CommodityID { get { return 1; } }
        public override string CommodityName { get { return "#"; } }
        public override int CommodityTypeID { get { return 1; } }

        
        public int CustomerID { get; set; }
        [Display(Name = "Mã khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerCode { get; set; }
        [Display(Name = "Tên khách hàng")]
        [UIHint("StringReadonly")]
        public string CustomerName { get; set; }

        public int ReceiverID { get; set; }
        [Display(Name = "Mã đơn vị nhận")]
        [UIHint("StringReadonly")]
        public string ReceiverCode { get; set; }
        [Display(Name = "Tên đơn vị nhận")]
        [UIHint("StringReadonly")]
        public string ReceiverName { get; set; }
        [Display(Name = "Địa chỉ")]
        [UIHint("StringReadonly")]
        public string ReceiverBillingAddress { get; set; }
        [Display(Name = "Khu vực")]
        [UIHint("StringReadonly")]
        public string EntireTerritoryEntireName { get; set; }

        public int Identification { get; set; }
        public string PrintedLabel { get; set; }

        [Display(Name = "Trọng lượng")]
        [UIHint("DecimalReadonly")]
        public decimal Weight { get; set; }
        [Display(Name = "TL thực tế")]
        [UIHint("DecimalReadonly")]
        public decimal RealWeight { get; set; }
    }
}
