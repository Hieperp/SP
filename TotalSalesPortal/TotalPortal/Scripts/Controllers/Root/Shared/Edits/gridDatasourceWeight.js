define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function () {
        this._updateTotalToModelProperty("TotalWeight", "Weight", "sum", false);

        definedExemplar._super._removeTotalToModelProperty.call(this);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowWeight(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeUnitWeight = function (dataRow) {
        this._updateRowWeight(dataRow);
    }
    
    definedExemplar.prototype._changeWeight = function (dataRow) {        
        this._updateTotalToModelProperty("TotalWeight", "Weight", "sum");
    }

    

    

    definedExemplar.prototype._updateRowWeight = function (dataRow) {
        dataRow.set("Weight", Math.round(dataRow.Quantity * dataRow.UnitWeight, requireConfig.websiteOptions.rndWeight));
    }


    return definedExemplar;
}));