define(["superBase", "gridDatasourceDiscount"], (function (superBase, gridDatasourceDiscount) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceDiscount);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalFreeQuantity", "FreeQuantity", "sum", false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }










    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowFreeQuantity(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }


    definedExemplar.prototype._changeFreeQuantity = function (dataRow) {
        this._updateTotalToModelProperty("TotalFreeQuantity", "FreeQuantity", "sum");
    }





    definedExemplar.prototype._updateRowFreeQuantity = function (dataRow) {
        dataRow.set("FreeQuantity", (dataRow.ControlFreeQuantity === 0 ? 0 : Math.floor(dataRow.Quantity / dataRow.ControlFreeQuantity)));
    }


    return definedExemplar;
}));