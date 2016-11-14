define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The GoodsIssueType here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("GoodsIssueTypeID", dataItem.GoodsIssueTypeID);
            currentDataSourceRow.set("GoodsIssueTypeCode", dataItem.Code);
            currentDataSourceRow.set("GoodsIssueTypeName", dataItem.Name);
            currentDataSourceRow.set("GoodsIssueClassID", dataItem.GoodsIssueClassID);
        }
    };

    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("GoodsIssueTypeName", window.GoodsIssueTypeNameBeforeChange);
    };

    return definedExemplar;
}));