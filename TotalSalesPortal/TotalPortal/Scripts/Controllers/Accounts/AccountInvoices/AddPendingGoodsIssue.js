function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(accountInvoiceGridDataSource, pendingGoodsIssueGridDataSource) {
    if (accountInvoiceGridDataSource != undefined && pendingGoodsIssueGridDataSource != undefined) {
        var pendingGoodsIssueGridDataItems = pendingGoodsIssueGridDataSource.view();
        var accountInvoiceJSON = accountInvoiceGridDataSource.data().toJSON();
        for (var i = 0; i < pendingGoodsIssueGridDataItems.length; i++) {
            if (pendingGoodsIssueGridDataItems[i].IsSelected === true)
                _setParentInput(accountInvoiceJSON, pendingGoodsIssueGridDataItems[i]);
        }
        accountInvoiceGridDataSource.data(accountInvoiceJSON);

        var dataRowTest = accountInvoiceGridDataSource.add({}); //To calculate total
        accountInvoiceGridDataSource.trigger("change");

        cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource

    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid

    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(accountInvoiceJSON, goodsIssueGridDataItem) {

        //var dataRow = accountInvoiceJSON.add({});

        var dataRow = new Object();

        dataRow.AccountInvoiceDetailID = 0;
        dataRow.AccountInvoiceID = window.parent.$("#AccountInvoiceID").val();
        dataRow.EntryDate = null;
        dataRow.LocationID = null;
        dataRow.Remarks = null;

        dataRow.GoodsIssueDetailID = goodsIssueGridDataItem.GoodsIssueDetailID;

        dataRow.CustomerID = goodsIssueGridDataItem.CustomerID;
        dataRow.CommodityID = goodsIssueGridDataItem.CommodityID;
        dataRow.CommodityName = goodsIssueGridDataItem.CommodityName;
        dataRow.CommodityCode = goodsIssueGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = goodsIssueGridDataItem.CommodityTypeID;



        dataRow.Quantity = goodsIssueGridDataItem.Quantity;
        dataRow.ListedPrice = goodsIssueGridDataItem.ListedPrice;
        dataRow.DiscountPercent = goodsIssueGridDataItem.DiscountPercent;
        dataRow.UnitPrice = goodsIssueGridDataItem.UnitPrice;
        dataRow.VATPercent = goodsIssueGridDataItem.VATPercent;
        dataRow.GrossPrice = goodsIssueGridDataItem.GrossPrice;
        dataRow.Amount = goodsIssueGridDataItem.Amount;
        dataRow.VATAmount = goodsIssueGridDataItem.VATAmount;
        dataRow.GrossAmount = goodsIssueGridDataItem.GrossAmount;

        dataRow.IsBonus = goodsIssueGridDataItem.IsBonus;        

        accountInvoiceJSON.push(dataRow);
    }
}

