define(["gridEditorGoodsIssueType"], (function (gridEditorGoodsIssueType) {

    gridEditorGoodsIssueTypeSelect = function (e) {
        var gridEditorGoodsIssueTypeInstance = new gridEditorGoodsIssueType("kendoGridDetails");
        gridEditorGoodsIssueTypeInstance.handleSelect(e);
    }

    gridEditorGoodsIssueTypeChange = function (e) {
        var gridEditorGoodsIssueTypeInstance = new gridEditorGoodsIssueType("kendoGridDetails");
        gridEditorGoodsIssueTypeInstance.handleChange(e);
    }
}));