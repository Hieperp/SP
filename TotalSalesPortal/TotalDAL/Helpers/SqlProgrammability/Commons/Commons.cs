using System.Text;
using TotalModel.Models;
using TotalBase.Enums;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Commons
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Commons(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.SplitToIntList();
            this.EntireTerritories();
            this.EntireCommodityCategories();
            this.EntireCustomerCategories();
        }

        private void SplitToIntList()
        {
            string queryString = " (@strString varchar(max)) " + "\r\n";
            queryString = queryString + " RETURNS @Result TABLE(Id int) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @x XML " + "\r\n";
            queryString = queryString + "       SELECT @x = CAST('<A>'+ REPLACE(@strString,',','</A><A>')+ '</A>' AS XML) " + "\r\n";

            queryString = queryString + "       INSERT INTO @Result " + "\r\n";
            queryString = queryString + "       SELECT t.value('.', 'int') AS inVal " + "\r\n";
            queryString = queryString + "       FROM @x.nodes('/A') AS x(t) " + "\r\n";
            queryString = queryString + "       RETURN " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateUserDefinedFunction("SplitToIntList", queryString);
        }


        private void EntireTerritories()
        {

            //--------A1.BIGIN
            string queryString = "   SELECT      " + "\r\n";
            queryString = queryString + "               TerritoryID, Name, Name AS EntireName, " + "\r\n";

            queryString = queryString + "               TerritoryID AS TerritoryID1, Name AS Name1, " + "\r\n";
            queryString = queryString + "               0 AS TerritoryID2, '' AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS TerritoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        Territories WHERE AncestorID IS NULL " + "\r\n";

            //--------A1.END
            //--------A2.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               Territories2.TerritoryID, Territories2.Name, Territories2.Name + ', ' + Territories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               Territories1.TerritoryID AS TerritoryID1, Territories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               Territories2.TerritoryID AS TerritoryID2, Territories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS TerritoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     TerritoryID, Name, AncestorID FROM Territories WHERE AncestorID IS NULL) AS Territories1 " + "\r\n";
            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     TerritoryID, Name, AncestorID FROM Territories WHERE AncestorID IN (SELECT TerritoryID FROM Territories WHERE AncestorID IS NULL)) AS Territories2 " + "\r\n";
            queryString = queryString + "               ON Territories1.TerritoryID = Territories2.AncestorID " + "\r\n";
            //--------A2.END

            //--------A3.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               Territories3.TerritoryID, Territories3.Name, Territories3.Name + ', ' + Territories2.Name + ', ' + Territories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               Territories1.TerritoryID AS TerritoryID1, Territories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               Territories2.TerritoryID AS TerritoryID2, Territories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               Territories3.TerritoryID AS TerritoryID3, Territories3.Name AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     TerritoryID, Name, AncestorID FROM Territories WHERE AncestorID IS NULL) AS Territories1 " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     TerritoryID, Name, AncestorID FROM Territories WHERE AncestorID IN (SELECT TerritoryID FROM Territories WHERE AncestorID IS NULL)) AS Territories2 " + "\r\n";
            queryString = queryString + "               ON Territories1.TerritoryID = Territories2.AncestorID " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     TerritoryID, Name, AncestorID FROM Territories WHERE AncestorID IN (SELECT TerritoryID FROM Territories WHERE AncestorID IN (SELECT TerritoryID FROM Territories WHERE AncestorID IS NULL))) AS Territories3 " + "\r\n";
            queryString = queryString + "               ON Territories2.TerritoryID = Territories3.AncestorID " + "\r\n";
            //--------A3.END

            this.totalSalesPortalEntities.CreateView("VWTerritories", queryString);
        }


        private void EntireCommodityCategories()
        {

            //--------A1.BIGIN
            string queryString = "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategoryID, Name, Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CommodityCategoryID AS CommodityCategoryID1, Name AS Name1, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID2, '' AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        CommodityCategories WHERE AncestorID IS NULL " + "\r\n";

            //--------A1.END
            //--------A2.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID, CommodityCategories2.Name, CommodityCategories2.Name + ', ' + CommodityCategories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CommodityCategories1.CommodityCategoryID AS CommodityCategoryID1, CommodityCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID AS CommodityCategoryID2, CommodityCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CommodityCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, AncestorID FROM CommodityCategories WHERE AncestorID IS NULL) AS CommodityCategories1 " + "\r\n";
            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL)) AS CommodityCategories2 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories1.CommodityCategoryID = CommodityCategories2.AncestorID " + "\r\n";
            //--------A2.END

            //--------A3.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CommodityCategories3.CommodityCategoryID, CommodityCategories3.Name, CommodityCategories3.Name + ', ' + CommodityCategories2.Name + ', ' + CommodityCategories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CommodityCategories1.CommodityCategoryID AS CommodityCategoryID1, CommodityCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CommodityCategories2.CommodityCategoryID AS CommodityCategoryID2, CommodityCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               CommodityCategories3.CommodityCategoryID AS CommodityCategoryID3, CommodityCategories3.Name AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, AncestorID FROM CommodityCategories WHERE AncestorID IS NULL) AS CommodityCategories1 " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL)) AS CommodityCategories2 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories1.CommodityCategoryID = CommodityCategories2.AncestorID " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CommodityCategoryID, Name, AncestorID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IN (SELECT CommodityCategoryID FROM CommodityCategories WHERE AncestorID IS NULL))) AS CommodityCategories3 " + "\r\n";
            queryString = queryString + "               ON CommodityCategories2.CommodityCategoryID = CommodityCategories3.AncestorID " + "\r\n";
            //--------A3.END

            this.totalSalesPortalEntities.CreateView("VWCommodityCategories", queryString);
        }


        private void EntireCustomerCategories()
        {

            //--------A1.BIGIN
            string queryString = "   SELECT      " + "\r\n";
            queryString = queryString + "               CustomerCategoryID, Name, Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CustomerCategoryID AS CustomerCategoryID1, Name AS Name1, " + "\r\n";
            queryString = queryString + "               0 AS CustomerCategoryID2, '' AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CustomerCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        CustomerCategories WHERE AncestorID IS NULL " + "\r\n";

            //--------A1.END
            //--------A2.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CustomerCategories2.CustomerCategoryID, CustomerCategories2.Name, CustomerCategories2.Name + ', ' + CustomerCategories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CustomerCategories1.CustomerCategoryID AS CustomerCategoryID1, CustomerCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CustomerCategories2.CustomerCategoryID AS CustomerCategoryID2, CustomerCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               0 AS CustomerCategoryID3, '' AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CustomerCategoryID, Name, AncestorID FROM CustomerCategories WHERE AncestorID IS NULL) AS CustomerCategories1 " + "\r\n";
            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CustomerCategoryID, Name, AncestorID FROM CustomerCategories WHERE AncestorID IN (SELECT CustomerCategoryID FROM CustomerCategories WHERE AncestorID IS NULL)) AS CustomerCategories2 " + "\r\n";
            queryString = queryString + "               ON CustomerCategories1.CustomerCategoryID = CustomerCategories2.AncestorID " + "\r\n";
            //--------A2.END

            //--------A3.BEGIN
            queryString = queryString + "   UNION ALL   " + "\r\n";

            queryString = queryString + "   SELECT      " + "\r\n";
            queryString = queryString + "               CustomerCategories3.CustomerCategoryID, CustomerCategories3.Name, CustomerCategories3.Name + ', ' + CustomerCategories2.Name + ', ' + CustomerCategories1.Name AS EntireName, " + "\r\n";

            queryString = queryString + "               CustomerCategories1.CustomerCategoryID AS CustomerCategoryID1, CustomerCategories1.Name AS Name1, " + "\r\n";
            queryString = queryString + "               CustomerCategories2.CustomerCategoryID AS CustomerCategoryID2, CustomerCategories2.Name AS Name2, " + "\r\n";
            queryString = queryString + "               CustomerCategories3.CustomerCategoryID AS CustomerCategoryID3, CustomerCategories3.Name AS Name3 " + "\r\n";

            queryString = queryString + "   FROM        " + "\r\n";

            queryString = queryString + "               (SELECT     CustomerCategoryID, Name, AncestorID FROM CustomerCategories WHERE AncestorID IS NULL) AS CustomerCategories1 " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CustomerCategoryID, Name, AncestorID FROM CustomerCategories WHERE AncestorID IN (SELECT CustomerCategoryID FROM CustomerCategories WHERE AncestorID IS NULL)) AS CustomerCategories2 " + "\r\n";
            queryString = queryString + "               ON CustomerCategories1.CustomerCategoryID = CustomerCategories2.AncestorID " + "\r\n";

            queryString = queryString + "               INNER JOIN " + "\r\n";
            queryString = queryString + "               (SELECT     CustomerCategoryID, Name, AncestorID FROM CustomerCategories WHERE AncestorID IN (SELECT CustomerCategoryID FROM CustomerCategories WHERE AncestorID IN (SELECT CustomerCategoryID FROM CustomerCategories WHERE AncestorID IS NULL))) AS CustomerCategories3 " + "\r\n";
            queryString = queryString + "               ON CustomerCategories2.CustomerCategoryID = CustomerCategories3.AncestorID " + "\r\n";
            //--------A3.END

            this.totalSalesPortalEntities.CreateView("VWCustomerCategories", queryString);
        }



        //        private void VehicleJournal()
        //        {


        //    string queryString = " @dDateFrom DateTime, @dDateTo DateTime " + "\r\n";
        //    queryString = queryString + " WITH ENCRYPTION " + "\r\n";
        //    queryString = queryString + " AS " + "\r\n";
        //    queryString = queryString + "    BEGIN " + "\r\n";

        //    queryString = queryString + "       SELECT      ListItemCommodity.CommodityID, ListItemCommodity.Description, ListItemCommodity.DescriptionOfficial, ListItemCommodity.UnitSales, ListItemCommodity.LeadTime, VWListItemCategory.SellLife, " + "\r\n";
        //    queryString = queryString + "                   WHJournalMaster.WHInputID, WHJournalMaster.WHInputDate, WHJournalMaster.SerialID, WHJournalMaster.Packing, WHJournalMaster.BatchNo, WHJournalMaster.SerialNo, WHJournalMaster.OriginalName, WHJournalMaster.ExpiryDate, " + "\r\n";
        //    queryString = queryString + "                   ISNULL(ListWarehouseName.WHLocationID, 0) AS WHLocationID, ISNULL(ListWarehouseName.WHCategoryID, 0) AS WHCategoryID, ISNULL(ListWarehouseName.WarehouseID, 0) AS WarehouseID, ISNULL(ListWarehouseName.Description, '') AS WarehouseName, " + "\r\n";
        //    queryString = queryString + "                   VWListSupplierName.SupplierTypeID, VWListSupplierName.SupplierID, VWListSupplierName.DescriptionOfficial AS SupplierName, WHJournalMaster.GoodsHolderFirstName, WHJournalMaster.GoodsHolderLastName, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerID, VWListCustomerName.Description AS CustomerCode, VWListCustomerName.DescriptionOfficial AS CustomerName, ListWHTransferType.WHTransferTypeID, ListWHTransferType.DescriptionOfficial AS WHTransferTypeName, " + "\r\n";

        //    queryString = queryString + "                   WHJournalMaster.QuantityBegin, WHJournalMaster.QuantityInputINV, WHJournalMaster.QuantityInputRTN, WHJournalMaster.QuantityInputTRF, WHJournalMaster.QuantityInputADJ, WHJournalMaster.QuantityInputBLD, WHJournalMaster.QuantityInputUBL, WHJournalMaster.QuantityInputINV + WHJournalMaster.QuantityInputRTN + WHJournalMaster.QuantityInputTRF + WHJournalMaster.QuantityInputADJ + WHJournalMaster.QuantityInputBLD + WHJournalMaster.QuantityInputUBL AS QuantityInput, " + "\r\n";
        //    queryString = queryString + "                   WHJournalMaster.QuantityOutputINV, WHJournalMaster.QuantityOutputTRF, WHJournalMaster.QuantityOutputADJ, WHJournalMaster.QuantityOutputBLD, WHJournalMaster.QuantityOutputUBL, WHJournalMaster.QuantityOutputINV + WHJournalMaster.QuantityOutputTRF + WHJournalMaster.QuantityOutputADJ + WHJournalMaster.QuantityOutputBLD + WHJournalMaster.QuantityOutputUBL AS QuantityOutput, " + "\r\n";
        //    queryString = queryString + "                   WHJournalMaster.QuantityBegin + WHJournalMaster.QuantityInputINV + WHJournalMaster.QuantityInputRTN + WHJournalMaster.QuantityInputTRF + WHJournalMaster.QuantityInputADJ + WHJournalMaster.QuantityInputBLD + WHJournalMaster.QuantityInputUBL - WHJournalMaster.QuantityOutputINV - WHJournalMaster.QuantityOutputTRF - WHJournalMaster.QuantityOutputADJ - WHJournalMaster.QuantityOutputBLD - WHJournalMaster.QuantityOutputUBL AS QuantityEnd, " + "\r\n";
        //    queryString = queryString + "                   WHJournalMaster.QuantityOnShip, WHJournalMaster.QuantityOnInput, " + "\r\n";
        //    queryString = queryString + "                   WHJournalMaster.UPriceCURInventory, WHJournalMaster.UPriceNMDInventory, WHJournalMaster.MovementMIN, WHJournalMaster.MovementMAX, WHJournalMaster.MovementAVG, " + "\r\n";

        //    queryString = queryString + "                   VWListItemCategory.ItemCategoryID, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description1 AS ItemCategoryName1, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description2 AS ItemCategoryName2, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description3 AS ItemCategoryName3, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description4 AS ItemCategoryName4, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description5 AS ItemCategoryName5, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description6 AS ItemCategoryName6, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description7 AS ItemCategoryName7, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description8 AS ItemCategoryName8, " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory.Description9 AS ItemCategoryName9, " + "\r\n";

        //    queryString = queryString + "                   VWListCustomerName.DistrictID, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.ProvinceID," + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryID, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName1, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName2, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName3, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName4, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName5, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName6, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName7, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName8, " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName.CustomerCategoryName9 " + "\r\n";


        //    queryString = queryString + "       FROM       (" + "\r\n";

        //                    //--BEGIN-INPUT-OUTPUT-END.END

        //                    queryString = queryString + "   SELECT  WHInputMaster.WHInputID, WHInputMaster.WHInputDate, WHInputDetail.SerialID, WHInputDetail.CommodityID, WHInputDetail.SupplierID, WHInputDetail.Packing, WHInputDetail.BatchNo, WHInputDetail.SerialNo, WHInputDetail.OriginalName, WHInputDetail.ExpiryDate, WHInputDetail.WarehouseID, WHInputMaster.WHTransferTypeID, WHInputMaster.CustomerID, GoodsHolderMaster.FirstName AS GoodsHolderFirstName, GoodsHolderMaster.LastName AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           WHUnionMaster.QuantityBegin, WHUnionMaster.QuantityInputINV, WHUnionMaster.QuantityInputRTN, WHUnionMaster.QuantityInputTRF, WHUnionMaster.QuantityInputADJ, WHUnionMaster.QuantityInputBLD, WHUnionMaster.QuantityInputUBL, WHUnionMaster.QuantityOutputINV, WHUnionMaster.QuantityOutputTRF, WHUnionMaster.QuantityOutputADJ, WHUnionMaster.QuantityOutputBLD, WHUnionMaster.QuantityOutputUBL, 0 AS QuantityOnShip, 0 AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "          (WHInputDetail.AmountCostCUR + WHInputDetail.AmountClearanceCUR)/ WHInputDetail.Quantity AS UPriceCURInventory, (WHInputDetail.AmountCostUSD + WHInputDetail.AmountClearanceUSD)/ WHInputDetail.Quantity AS UPriceNMDInventory, WHUnionMaster.MovementMIN, WHUnionMaster.MovementMAX, WHUnionMaster.MovementAVG " + "\r\n";

        //// NOTE 24.APR.2007: VIEC TINH GIA TON KHO (WHInputDetail.AmountCostCUR + WHInputDetail.AmountClearanceCUR)/ WHInputDetail.Quantity AS UPriceCURInventory, (WHInputDetail.AmountCostUSD + WHInputDetail.AmountClearanceUSD)/ WHInputDetail.Quantity AS UPriceNMDInventory
        //// SU DUNG CONG THUC TREN CHI TAM THOI MA THOI, CO THE DAN DEN SAI SO (SU DUNG TAM THOI DE IN BAO CAO KHO CO SO LIEU)
        //// SAU NAY NEN SUA LAI, SU DUNG PHEP +/ - MA THOI
        //// XEM SPWHAmountCostofsalesGet DE TINH LUONG REMAIN NHE

        //                    queryString = queryString + "   FROM   (" + "\r\n";

        //                            queryString = queryString + "   SELECT  WHUnionDetail.WHInputID, WHUnionDetail.SerialID, " + "\r\n";
        //                            queryString = queryString + "           SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInputINV) AS QuantityInputINV, SUM(QuantityInputRTN) AS QuantityInputRTN, SUM(QuantityInputTRF) AS QuantityInputTRF, SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(QuantityInputBLD) AS QuantityInputBLD, SUM(QuantityInputUBL) AS QuantityInputUBL, SUM(QuantityOutputINV) AS QuantityOutputINV, SUM(QuantityOutputTRF) AS QuantityOutputTRF, SUM(QuantityOutputADJ) AS QuantityOutputADJ, SUM(QuantityOutputBLD) AS QuantityOutputBLD, SUM(QuantityOutputUBL) AS QuantityOutputUBL, " + "\r\n";
        //                            queryString = queryString + "           MIN(MovementDate) AS MovementMIN, MAX(MovementDate) AS MovementMAX, SUM((QuantityOutputINV + QuantityOutputTRF + QuantityOutputADJ + QuantityOutputBLD + QuantityOutputUBL) * MovementDate) / SUM(QuantityOutputINV + QuantityOutputTRF + QuantityOutputADJ + QuantityOutputBLD + QuantityOutputUBL) AS MovementAVG " + "\r\n";
        //                            queryString = queryString + "   FROM    (" + "\r\n";
        //                                    //BEGINING
        //                                            //WHINPUT
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, ROUND(WHInputDetail.Quantity - WHInputDetail.QuantityOutput, " & p0QTY & ") AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, NULL AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHInputMaster.WHInputDate < @dDateFrom AND WHInputDetail.Quantity > WHInputDetail.QuantityOutput " + "\r\n";

        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                            //UNDO (CAC CAU SQL CHO INVOICE, WHTRANSFER, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
        //                                            //UNDO WHOUTPUT (INVOICE)
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, WHOutputDetail.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, NULL AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHOutputDetail ON WHInputDetail.WHInputID = WHOutputDetail.WHInputID AND WHInputDetail.SerialID = WHOutputDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHOutputMaster ON WHOutputDetail.WHOutputID = WHOutputMaster.WHOutputID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHInputMaster.WHInputDate < @dDateFrom AND WHOutputMaster.WHOutputDate >= @dDateFrom " + "\r\n";

        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                            //UNDO WHTRANSFER
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, WHTransferDetail.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, NULL AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHTransferDetail ON WHInputDetail.WHInputID = WHTransferDetail.WHInputID AND WHInputDetail.SerialID = WHTransferDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHTransferMaster ON WHTransferDetail.WHTransferID = WHTransferMaster.WHTransferID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHInputMaster.WHInputDate < @dDateFrom AND WHTransferMaster.WHTransferDate >= @dDateFrom " + "\r\n";

        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                            //UNDO WHADJUST
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, -WHAdjustDetail.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, NULL AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHAdjustDetail ON WHInputDetail.WHInputID = WHAdjustDetail.WHInputID AND WHInputDetail.SerialID = WHAdjustDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHAdjustMaster ON WHAdjustDetail.WHAdjustID = WHAdjustMaster.WHAdjustID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHInputMaster.WHInputDate < @dDateFrom AND WHAdjustMaster.WHAdjustDate >= @dDateFrom AND WHAdjustDetail.Quantity < 0 " + "\r\n"; //ONLY AND WHAdjustDetail.Quantity < 0: OUPUT FROM WHINPUT
        //                                            //UNDO WHASSEMBLY: BUILD + UNBUILD: IMPLEMENT CODE LATER

        //                                    //INTPUT
        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, 0 AS QuantityBegin, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EInvoice & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputINV, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EReturn & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputRTN, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EWHTransfer & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputTRF, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EWHAdjust & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputADJ, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EWHAssemblyMaster & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputBLD, " + "\r\n";
        //                                    queryString = queryString + "               CASE WHEN WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EWHAssemblyDetail & " THEN WHInputDetail.Quantity ELSE 0 END AS QuantityInputUBL, " + "\r\n";
        //                                    queryString = queryString + "               0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, NULL AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHInputMaster.WHInputDate >= @dDateFrom AND WHInputMaster.WHInputDate <= @dDateTo " + "\r\n";

        //                                    //OUTPUT (CAC CAU SQL CHO INVOICE, WHTRANSFER, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
        //                                    queryString = queryString + "   UNION ALL "
        //                                            //WHOUTPUT (INVOICE) + "\r\n";
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, WHOutputDetail.Quantity AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, DATEDIFF(DAY, WHInputMaster.WHInputDate, WHOutputMaster.WHOutputDate) AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHOutputDetail ON WHInputDetail.WHInputID = WHOutputDetail.WHInputID AND WHInputDetail.SerialID = WHOutputDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHOutputMaster ON WHOutputDetail.WHOutputID = WHOutputMaster.WHOutputID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHOutputMaster.WHOutputDate >= @dDateFrom AND WHOutputMaster.WHOutputDate <= @dDateTo " + "\r\n";

        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                            //WHTRANSFER
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, WHTransferDetail.Quantity AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, DATEDIFF(DAY, WHInputMaster.WHInputDate, WHTransferMaster.WHTransferDate) AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHTransferDetail ON WHInputDetail.WHInputID = WHTransferDetail.WHInputID AND WHInputDetail.SerialID = WHTransferDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHTransferMaster ON WHTransferDetail.WHTransferID = WHTransferMaster.WHTransferID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHTransferMaster.WHTransferDate >= @dDateFrom AND WHTransferMaster.WHTransferDate <= @dDateTo " + "\r\n";

        //                                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                                        //WHADJUST
        //                                    queryString = queryString + "   SELECT      WHInputDetail.WHInputID, WHInputDetail.SerialID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, -WHAdjustDetail.Quantity AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, DATEDIFF(DAY, WHInputMaster.WHInputDate, WHAdjustMaster.WHAdjustDate) AS MovementDate " + "\r\n";
        //                                    queryString = queryString + "   FROM        WHInputMaster INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHAdjustDetail ON WHInputDetail.WHInputID = WHAdjustDetail.WHInputID AND WHInputDetail.SerialID = WHAdjustDetail.WHInputSerialID INNER JOIN " + "\r\n";
        //                                    queryString = queryString + "               WHAdjustMaster ON WHAdjustDetail.WHAdjustID = WHAdjustMaster.WHAdjustID " + "\r\n";
        //                                    queryString = queryString + "   WHERE       WHAdjustMaster.WHAdjustDate >= @dDateFrom AND WHAdjustMaster.WHAdjustDate <= @dDateTo AND WHAdjustDetail.Quantity < 0 "  + "\r\n"; //ONLY AND WHAdjustDetail.Quantity < 0: OUPUT FROM WHINPUT
        //                                        //WHASSEMBLY: IMPLEMENT CODE LATER

        //                                queryString = queryString + "       ) AS WHUnionDetail " + "\r\n";
        //                            queryString = queryString + "   GROUP BY WHUnionDetail.WHInputID, WHUnionDetail.SerialID " + "\r\n";
        //                            queryString = queryString + "   ) AS WHUnionMaster INNER JOIN " + "\r\n";

        //                    queryString = queryString + "           WHInputMaster ON WHUnionMaster.WHInputID = WHInputMaster.WHInputID INNER JOIN " + "\r\n";
        //                    queryString = queryString + "           WHInputDetail ON WHUnionMaster.WHInputID = WHInputDetail.WHInputID AND WHUnionMaster.SerialID = WHInputDetail.SerialID LEFT JOIN " + "\r\n";
        //                    queryString = queryString + "           HRmgr.dbo.EmployeeMaster GoodsHolderMaster ON WHInputMaster.GoodsHolderID = GoodsHolderMaster.EmployeeID " + "\r\n";

        //                    //--BEGIN-INPUT-OUTPUT-END.END

        //                    queryString = queryString + "   UNION ALL "
        //                    //--ON SHIP.BEGIN
        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, PContractDetail.CommodityID, PContractMaster.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, (PContractDetail.Quantity - PContractDetail.QuantityShipment) AS QuantityOnShip, 0 AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    PContractMaster INNER JOIN PContractDetail ON PContractMaster.PContractID = PContractDetail.PContractID " + "\r\n";
        //                    queryString = queryString + "   WHERE   PContractMaster.PContractDate <= @dDateTo AND PContractDetail.Quantity > PContractDetail.QuantityShipment " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";

        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, ShipmentDetail.CommodityID, PContractMaster.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, ShipmentDetail.Quantity AS QuantityOnShip, 0 AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    PContractMaster INNER JOIN ShipmentMaster ON PContractMaster.PContractID = ShipmentMaster.PContractID INNER JOIN " + "\r\n";
        //                    queryString = queryString + "           ShipmentDetail ON ShipmentMaster.ShipmentID = ShipmentDetail.ShipmentID" + "\r\n";
        //                    queryString = queryString + "   WHERE   PContractMaster.PContractDate <= @dDateTo AND ShipmentMaster.ShipmentDate > @dDateTo  " + "\r\n";
        //                    //--ON SHIP.END

        //                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                    //--ON INPUT.BEGIN (CAC CAU SQL DUNG CHO EWHInputVoucherTypeID.EInvoice, EWHInputVoucherTypeID.EReturn, EWHInputVoucherTypeID.EWHTransfer, EWHInputVoucherTypeID.EWHAdjust, EWHInputVoucherTypeID.EWHAssemblyMaster, EWHInputVoucherTypeID.EWHAssemblyDetail LA HOAN TOAN GIONG NHAU)
        //                            //EWHInputVoucherTypeID.EInvoice
        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, ShipmentDetail.CommodityID, ShipmentDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, (ShipmentDetail.Quantity - ShipmentDetail.QuantityInput) AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    ShipmentMaster INNER JOIN ShipmentDetail ON ShipmentMaster.ShipmentID = ShipmentDetail.ShipmentID " + "\r\n";
        //                    queryString = queryString + "   WHERE   ShipmentMaster.ShipmentDate <= @dDateTo AND ShipmentDetail.Quantity > ShipmentDetail.QuantityInput " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";

        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, WHInputDetail.CommodityID, WHInputDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, WHInputDetail.Quantity AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    ShipmentMaster INNER JOIN WHInputMaster ON ShipmentMaster.ShipmentID = WHInputMaster.WHInputVoucherID AND WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EInvoice & " INNER JOIN " + "\r\n";
        //                    queryString = queryString + "           WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID" + "\r\n";
        //                    queryString = queryString + "   WHERE   ShipmentMaster.ShipmentDate <= @dDateTo AND WHInputMaster.WHInputDate > @dDateTo " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                            //EWHInputVoucherTypeID.EReturn
        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, ReturnDetail.CommodityID, ReturnDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, (ReturnDetail.Quantity - ReturnDetail.QuantityInput) AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    ReturnMaster INNER JOIN ReturnDetail ON ReturnMaster.ReturnID = ReturnDetail.ReturnID " + "\r\n";
        //                    queryString = queryString + "   WHERE   ReturnMaster.ReturnDate <= @dDateTo AND ReturnDetail.Quantity > ReturnDetail.QuantityInput " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";

        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, WHInputDetail.CommodityID, WHInputDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, WHInputDetail.Quantity AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    ReturnMaster INNER JOIN WHInputMaster ON ReturnMaster.ReturnID = WHInputMaster.WHInputVoucherID AND WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EReturn & " INNER JOIN " + "\r\n";
        //                    queryString = queryString + "           WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID"
        //                    queryString = queryString + "   WHERE   ReturnMaster.ReturnDate <= @dDateTo AND WHInputMaster.WHInputDate > @dDateTo " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";
        //                            //EWHInputVoucherTypeID.EWHTransfer
        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, WHTransferDetail.CommodityID, WHTransferDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, (WHTransferDetail.Quantity - WHTransferDetail.QuantityInput) AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    WHTransferMaster INNER JOIN WHTransferDetail ON WHTransferMaster.WHTransferID = WHTransferDetail.WHTransferID " + "\r\n";
        //                    queryString = queryString + "   WHERE   WHTransferMaster.WHTransferDate <= @dDateTo AND WHTransferDetail.Quantity > WHTransferDetail.QuantityInput " + "\r\n";

        //                    queryString = queryString + "   UNION ALL " + "\r\n";

        //                    queryString = queryString + "   SELECT  0 AS WHInputID, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS WHInputDate, 0 AS SerialID, WHInputDetail.CommodityID, WHInputDetail.SupplierID, '' AS Packing, '' AS BatchNo, '' AS SerialNo, '' AS OriginalName, CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) AS ExpiryDate, 0 AS WarehouseID, 0 AS WHTransferTypeID, 0 AS CustomerID, '' AS GoodsHolderFirstName, '' AS GoodsHolderLastName, " + "\r\n";
        //                    queryString = queryString + "           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityInputBLD, 0 AS QuantityInputUBL, 0 AS QuantityOutputINV, 0 AS QuantityOutputTRF, 0 AS QuantityOutputADJ, 0 AS QuantityOutputBLD, 0 AS QuantityOutputUBL, 0 AS QuantityOnShip, WHInputDetail.Quantity AS QuantityOnInput, " + "\r\n";
        //                    queryString = queryString + "           0 AS UPriceCURInventory, 0 AS UPriceNMDInventory, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
        //                    queryString = queryString + "   FROM    WHTransferMaster INNER JOIN WHInputMaster ON WHTransferMaster.WHTransferID = WHInputMaster.WHInputVoucherID AND WHInputMaster.WHInputVoucherTypeID = " & EWHInputVoucherTypeID.EWHTransfer & " INNER JOIN " + "\r\n";
        //                    queryString = queryString + "           WHInputDetail ON WHInputMaster.WHInputID = WHInputDetail.WHInputID" + "\r\n";
        //                    queryString = queryString + "   WHERE   WHTransferMaster.WHTransferDate <= @dDateTo AND WHInputMaster.WHInputDate > @dDateTo " + "\r\n";

        //                            //EWHInputVoucherTypeID.EWHAdjust: KHONG XET T/H EWHInputVoucherTypeID.EWHAdjust, BOI VI KHI SAVE EWHInputVoucherTypeID.EWHAdjust: CHUONG TRINH TU DONG NHAP KHO, DO DO LUON LUON SE KHONG CON QuantityOnInput
        //                            //IMPLEMENT CODE LATER FOR: EWHInputVoucherTypeID.EWHAssemblyMaster, EWHInputVoucherTypeID.EWHAssemblyDetail
        //                    //--ON INPUT.END

        //                    queryString = queryString + "   ) AS WHJournalMaster INNER JOIN " + "\r\n";

        //    queryString = queryString + "                   VWListSupplierName ON WHJournalMaster.SupplierID = VWListSupplierName.SupplierID INNER JOIN " + "\r\n";
        //    queryString = queryString + "                   ListItemCommodity ON WHJournalMaster.CommodityID = ListItemCommodity.CommodityID INNER JOIN " + "\r\n";
        //    queryString = queryString + "                   VWListItemCategory ON ListItemCommodity.ItemCategoryID = VWListItemCategory.ItemCategoryID LEFT JOIN " + "\r\n";

        //    queryString = queryString + "                   ListWarehouseName ON WHJournalMaster.WarehouseID = ListWarehouseName.WarehouseID LEFT JOIN " + "\r\n";
        //    queryString = queryString + "                   ListWHTransferType ON WHJournalMaster.WHTransferTypeID = ListWHTransferType.WHTransferTypeID LEFT JOIN " + "\r\n";
        //    queryString = queryString + "                   VWListCustomerName ON WHJournalMaster.CustomerID = VWListCustomerName.CustomerID " + "\r\n";

        //    queryString = queryString + "    END " + "\r\n";


        //        }





















        private void GetPendingGoodsIssues()
        {
            string queryString; string queryEdit; string queryNew;

            queryNew = "                SELECT          GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.EntryDate AS GoodsIssueDetailEntryDate, GoodsIssueDetails.Reference AS GoodsIssueDetailReference, 0 AS AccountInvoiceDetailID, 0 AS AccountInvoiceID, " + "\r\n";
            queryNew = queryNew + "                     GoodsIssueDetails.CustomerID, Customers.Name AS CustomerName, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) AS QuantityRemains, " + "\r\n";
            queryNew = queryNew + "                     0.0 AS AccountInvoiceAmount, GoodsIssueDetails.Quantity, GoodsIssueDetails.Remarks " + "\r\n";

            queryNew = queryNew + "     FROM            GoodsIssueDetails INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID AND ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) > 0 " + "\r\n";
            queryNew = queryNew + "     WHERE           (@GoodsIssueID = 0 OR GoodsIssueDetails.GoodsIssueDetailID = @GoodsIssueID) AND (@CustomerID = 0 OR GoodsIssueDetails.CustomerID = @CustomerID) " + "\r\n";


            queryEdit = "               SELECT          GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.EntryDate AS GoodsIssueDetailEntryDate, GoodsIssueDetails.Reference AS GoodsIssueDetailReference, AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssueDetails.CustomerID, Customers.Name AS CustomerName, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.AccountInvoiceAmount, 0) AS QuantityRemains, " + "\r\n";
            queryEdit = queryEdit + "                   AccountInvoiceDetails.AccountInvoiceAmount, GoodsIssueDetails.Quantity, AccountInvoiceDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            GoodsIssueDetails INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND GoodsIssueDetails.GoodsIssueDetailID = AccountInvoiceDetails.GoodsIssueDetailID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Customers ON AccountInvoiceDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryEdit = queryEdit + "   WHERE           (@GoodsIssueID = 0 OR GoodsIssueDetails.GoodsIssueDetailID = @GoodsIssueID) AND (@CustomerID = 0 OR GoodsIssueDetails.CustomerID = @CustomerID) " + "\r\n";


            queryString = " @AccountInvoiceID Int, @GoodsIssueID Int, @CustomerID Int, @CommodityTypeID int, @AspUserID nvarchar(128), @LocationID Int, @FromDate DateTime, @ToDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + " IF (@AccountInvoiceID <= 0) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryNew + "\r\n";
            queryString = queryString + "           ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";
            queryString = queryString + " ELSE " + "\r\n";

            queryString = queryString + "       IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryNew + " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM AccountInvoiceDetails WHERE AccountInvoiceID = @AccountInvoiceID) " + "\r\n";
            queryString = queryString + "               UNION ALL " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssues", queryString);

        }























        private void GetPendingGoodsIssues1()
        {
            string queryString;

            queryString = " @GoodsIssueID Int, @AspUserID nvarchar(128), @LocationID Int, @CommodityTypeID int, @FromDate DateTime, @ToDate DateTime, @AccountInvoiceID Int, @GoodsIssueDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n"; //IF @GoodsIssueID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssues", queryString);
        }

        private string GetPendingGoodsIssuesBuildSQL(bool isGoodsIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@AccountInvoiceID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLA(isGoodsIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLA(isGoodsIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLA(bool isGoodsIssueID, bool isAccountInvoiceID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLB(isGoodsIssueID, isAccountInvoiceID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLB(isGoodsIssueID, isAccountInvoiceID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        /// <summary>
        /// KTRA: SAVE UPDATE -- IsFinished KTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinished
        /// </summary>
        /// <param name="isGoodsIssueID"></param>
        /// <param name="isAccountInvoiceID"></param>
        /// <param name="isGoodsIssueDetailIDs"></param>
        /// <returns></returns>
        private string GetPendingGoodsIssuesBuildSQLB(bool isGoodsIssueID, bool isAccountInvoiceID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AddressNo, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.Amount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        GoodsIssueDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.GoodsIssueID " + (isGoodsIssueID ? " = @GoodsIssueID " : " IN (SELECT GoodsIssues.GoodsIssueID FROM                  GoodsIssues              WHERE GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate AND GoodsIssues.LocationID = @LocationID AND GoodsIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2))      AND (GoodsIssueDetails.AccountInvoiceID IS NULL " + (isAccountInvoiceID ? " OR GoodsIssueDetails.AccountInvoiceID = @AccountInvoiceID" : "") + ")" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "")) + " AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 AND (@CommodityTypeID = 0 OR Commodities.CommodityTypeID = @CommodityTypeID) INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }


    }



}
