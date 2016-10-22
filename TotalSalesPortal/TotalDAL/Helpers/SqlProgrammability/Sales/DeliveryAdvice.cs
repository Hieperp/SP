﻿using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Sales
{
    public class DeliveryAdvice
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public DeliveryAdvice(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetDeliveryAdviceIndexes();

            this.GetPromotionByCustomers();

            //this.GetCommoditiesInWarehouses("GetVehicleAvailables", true, false, false, false);
            this.GetCommoditiesInWarehouses("GetCommodityAvailables", false, true, false, false, false); //GetPartAvailables
            //this.GetCommoditiesInWarehouses("GetCommoditiesInWarehouses", false, true, true, false);
            //this.GetCommoditiesInWarehouses("GetCommoditiesAvailables", true, true, false, false);

            //this.GetCommoditiesInWarehouses("GetCommoditiesInWarehousesIncludeOutOfStock", false, true, true, true);

            this.GetDeliveryAdviceViewDetails();
            this.DeliveryAdviceSaveRelative();
            this.DeliveryAdvicePostSaveValidate();

            this.DeliveryAdviceEditable();
            this.DeliveryAdviceInitReference();

        }


        private void GetDeliveryAdviceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdvices.DeliveryAdviceID, CAST(DeliveryAdvices.EntryDate AS DATE) AS EntryDate, DeliveryAdvices.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, SalesOrders.EntryDate AS SalesOrderDate, SalesOrders.Reference AS SalesOrderReference, DeliveryAdvices.TotalGrossAmount, DeliveryAdvices.Approved " + "\r\n";
            queryString = queryString + "       FROM        DeliveryAdvices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON DeliveryAdvices.EntryDate >= @FromDate AND DeliveryAdvices.EntryDate <= @ToDate AND DeliveryAdvices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.DeliveryAdvice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = DeliveryAdvices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON DeliveryAdvices.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   SalesOrders ON DeliveryAdvices.SalesOrderID = SalesOrders.SalesOrderID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetDeliveryAdviceIndexes", queryString);
        }


        private void GetPromotionByCustomers()
        {
            string queryString;

            queryString = " @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CustomerID IS NULL) ";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               SELECT * FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate " + "\r\n"; 
            queryString = queryString + "           END ";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN ";

            queryString = queryString + "               DECLARE @CustomerCategoryID int " + "\r\n";
            queryString = queryString + "               SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

            queryString = queryString + "               SELECT * FROM Promotions WHERE PromotionID IN " + "\r\n"; 
            queryString = queryString + "                  (SELECT Promotions.PromotionID FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND PromotionCustomers.CustomerID = @CustomerID " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategoryies ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.PromotionID = PromotionCustomerCategoryies.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategoryies.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
            queryString = queryString + "                  )" + "\r\n";
            queryString = queryString + "           END ";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionByCustomers", queryString);
        }

        /// <summary>
        /// Get QuantityAvailable (Remaining) Commodities BY EVERY (WarehouseID, CommodityID)
        /// </summary>
        private void GetCommoditiesInWarehouses(string storedProcedureName, bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData, bool includeCommoditiesOutOfStock, bool isUsingWarehouseBalancePrice)
        {
            //HIEN TAI, SU DUNG CHUNG CAC CAU SQL DE TAO RA NHIEU PHIEN BAN StoredProcedure, MUC DICH: NHAM DE QUAN LY CODE TUONG TU NHAU
            //VI VAY, CODE NAY TUAN THEO QUY UOC SAU: (CHI LA QUY UOC THOI, KHONG PHAI DIEU KIEN RANG BUOC GI CA)
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = FALSE and getSavedData = FALSE: GetVehicleAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetPartAvailables
            //withCommoditiesInGoodsReceipts = FALSE and withCommoditiesInWarehouses = TRUE  and getSavedData = TRUE: GetCommoditiesInWarehouses
            //withCommoditiesInGoodsReceipts = TRUE  and withCommoditiesInWarehouses = TRUE  and getSavedData = FALSE: GetCommoditiesAvailables
            //THEO QUY UOC NAY THI: getSavedData = TRUE ONLY WHEN: GetCommoditiesInWarehouses: DUOC SU DUNG TRONG GoodsIssue, StockTransfer, InventoryAdjustment
            //HAI TRUONG HOP: GetVehicleAvailables VA GetPartAvailables: DUOC SU DUNG TRONG VehicleTransferOrder VA PartTransferOrder (TAT NHIEN, SE CON DUOC SU DUNG TRONG NHUNG TRUONG HOP KHAC KHI KHONG YEU CAU LAY getSavedData, TUY NHIEN, HIEN TAI CHUA CO SU DUNG NOI NAO KHAC)
            //TRUONG HOP CUOI CUNG: GetCommoditiesAvailables: CUNG GIONG NHU GetVehicleAvailables VA GetPartAvailables, TUY NHIEN, NO BAO GOM CA Vehicle VA PartANDConsumable (get both Vehicles and Parts/ Consumables on the same view) (HIEN TAI, GetCommoditiesAvailables: CHUA DUOC SU DUNG O CHO NAO CA, CHI DE DANH SAU NAY CAN THIET THI SU DUNG THOI)


            //BO SUNG NGAY 08-JUN-2016: includeCommoditiesOutOfStock = TRUE: CHI DUY NHAT AP DUNG CHO GetCommoditiesInWarehousesIncludeOutOfStock. CAC T/H KHAC CHUA XEM XET, DO CHUA CO NHU CAU SU DUNG. NEU CO NHU CAU -> THI CO THE XEM XET LAI SQL QUERY


            string queryString = " @LocationID int, @CustomerID int, @PriceCategoryID int, @PromotionID int, @EntryDate DateTime, @SearchText nvarchar(60) " + (getSavedData ? ", @GoodsIssueID int, @StockTransferID int, @InventoryAdjustmentID int " : "") + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       DECLARE @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, Name nvarchar(200) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, DiscountPercent decimal(18, 2) NOT NULL, CommodityTypeID int NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @CommoditiesAvailable TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL, QuantityAvailable decimal(18, 2) NOT NULL, Bookable bit NULL)" + "\r\n";
            queryString = queryString + "       DECLARE @HasCommoditiesAvailable int SET @HasCommoditiesAvailable = 0" + "\r\n";

            queryString = queryString + "       INSERT INTO @Commodities SELECT CommodityID, Code, Name, 0 AS GrossPrice, 0 AS DiscountPercent, CommodityTypeID, CommodityCategoryID FROM Commodities WHERE CommodityTypeID IN (" + (withCommoditiesInGoodsReceipts ? "" + (int)GlobalEnums.CommodityTypeID.Vehicles : "") + (withCommoditiesInGoodsReceipts && withCommoditiesInWarehouses ? ", " : "") + (withCommoditiesInWarehouses ? (int)GlobalEnums.CommodityTypeID.Parts + ", " + (int)GlobalEnums.CommodityTypeID.Consumables : "") + ") AND (Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";


            queryString = queryString + "       IF (@@ROWCOUNT > 0) " + "\r\n";
            {
                queryString = queryString + "       BEGIN ";
                if (!isUsingWarehouseBalancePrice) //GET 
                {
                    queryString = queryString + "   UPDATE  Commodities SET Commodities.GrossPrice = CommodityPrices.GrossPrice FROM @Commodities Commodities INNER JOIN CommodityPrices ON Commodities.CommodityID = CommodityPrices.CommodityID AND CommodityPrices.PriceCategoryID = @PriceCategoryID "; //UPDATE GrossPrice BY @PriceCategoryID

                    queryString = queryString + "   IF (NOT @PromotionID IS NULL) ";
                    queryString = queryString + "       BEGIN ";

                    queryString = queryString + "           DECLARE @DiscountPercent decimal(18, 2), @ApplyToAllCommodities bit ";
                    queryString = queryString + "           SELECT @DiscountPercent = DiscountPercent, @ApplyToAllCommodities = ApplyToAllCommodities FROM Promotions WHERE PromotionID = @PromotionID AND InActive = 0 AND GetDate() >= StartDate AND GetDate() <= EndDate ";

                    queryString = queryString + "           IF (NOT @DiscountPercent IS NULL) ";
                    queryString = queryString + "               BEGIN ";
                    queryString = queryString + "                   IF (@ApplyToAllCommodities = 1) ";
                    queryString = queryString + "                       UPDATE @Commodities SET DiscountPercent = @DiscountPercent ";
                    queryString = queryString + "                   ELSE ";
                    queryString = queryString + "                       UPDATE @Commodities SET DiscountPercent = @DiscountPercent WHERE CommodityID IN (";
                    queryString = queryString + "                           SELECT CommodityID FROM PromotionCommodities WHERE PromotionID = @PromotionID AND CommodityID IN (SELECT CommodityID FROM @Commodities)";
                    queryString = queryString + "                           UNION ALL ";
                    queryString = queryString + "                           SELECT  CommodityID FROM Commodities WHERE Commodities.CommodityID IN (SELECT CommodityID FROM @Commodities) AND CommodityCategoryID IN ";
                    queryString = queryString + "                                  (SELECT  AncestorCommodityCategories.CommodityCategoryID " ;
                    queryString = queryString + "                                   FROM    PromotionCommodityCategoryies ";
                    queryString = queryString + "                                           INNER JOIN AncestorCommodityCategories ON PromotionCommodityCategoryies.PromotionID = @PromotionID AND PromotionCommodityCategoryies.CommodityCategoryID = AncestorCommodityCategories.AncestorID ";
                    queryString = queryString + "                                  )";
                    queryString = queryString + "                       )";

                    queryString = queryString + "               END ";

                    queryString = queryString + "       END ";

                    queryString = queryString + "   ELSE "; //(@PromotionID IS NULL)
                    queryString = queryString + "       BEGIN ";

                    queryString = queryString + "           DECLARE @CustomerCategoryID int, @PromotionIDList varchar(3999) " + "\r\n";

                    queryString = queryString + "           SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

                    queryString = queryString + "           SELECT  @PromotionIDList = STUFF((SELECT ',' + CAST(PromotionID AS varchar) " + "\r\n"; //CONVERT TO @PromotionIDList OF @CustomerID, USING @PromotionIDList AS FILTER FOR PromotionCommodities OR PromotionCommodityCategoryies AT NEXT STEP 
                    queryString = queryString + "           FROM    (" + "\r\n";
                    queryString = queryString + "                    SELECT DISTINCT PromotionID FROM " + "\r\n"; //GET DISTINCT PromotionID BY: @CustomerID UNION @CustomerCategoryID
                    queryString = queryString + "                          (SELECT Promotions.PromotionID FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
                    queryString = queryString + "                           UNION ALL " + "\r\n";
                    queryString = queryString + "                           SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND PromotionCustomers.CustomerID = @CustomerID " + "\r\n";
                    queryString = queryString + "                           UNION ALL " + "\r\n";
                    queryString = queryString + "                           SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategoryies ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.PromotionID = PromotionCustomerCategoryies.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategoryies.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
                    queryString = queryString + "                          )UNIONPromotions " + "\r\n";
                    queryString = queryString + "                   ) DISTINCTUNIONPromotions " + "\r\n";
                    queryString = queryString + "           FOR XML PATH('')) ,1,1,'') " + "\r\n";

                    queryString = queryString + "           UPDATE  Commodities SET Commodities.DiscountPercent = OVERPARTITIONPromotions.DiscountPercent "; //UPDATE @Commodities.DiscountPercent BY MAX DiscountPercent
                    queryString = queryString + "           FROM    @Commodities Commodities INNER JOIN ";

                    queryString = queryString + "                   (SELECT  CommodityID, DiscountPercent, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY DiscountPercent DESC) AS RowNo ";
                    queryString = queryString + "                   FROM    ( ";
                    queryString = queryString + "                           SELECT  Commodities.CommodityID, Promotions.DiscountPercent ";
                    queryString = queryString + "                           FROM    Promotions ";
                    queryString = queryString + "                                   CROSS JOIN @Commodities Commodities WHERE Promotions.ApplyToAllCommodities = 1 AND Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) ";
                    queryString = queryString + "                           UNION ALL ";
                    queryString = queryString + "                           SELECT  PromotionCommodities.CommodityID, Promotions.DiscountPercent ";
                    queryString = queryString + "                           FROM    Promotions ";
                    queryString = queryString + "                                   INNER JOIN  PromotionCommodities ON Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) AND PromotionCommodities.CommodityID IN (SELECT CommodityID FROM @Commodities) AND Promotions.PromotionID = PromotionCommodities.PromotionID ";
                    queryString = queryString + "                           UNION ALL ";
                    queryString = queryString + "                           SELECT  Commodities.CommodityID, Promotions.DiscountPercent ";
                    queryString = queryString + "                           FROM    Promotions ";
                    queryString = queryString + "                                   INNER JOIN  PromotionCommodityCategoryies ON Promotions.PromotionID IN (SELECT Id FROM dbo.SplitToIntList (@PromotionIDList)) AND Promotions.PromotionID = PromotionCommodityCategoryies.PromotionID ";
                    queryString = queryString + "                                   INNER JOIN  AncestorCommodityCategories ON PromotionCommodityCategoryies.CommodityCategoryID = AncestorCommodityCategories.AncestorID ";
                    queryString = queryString + "                                   INNER JOIN  @Commodities Commodities ON AncestorCommodityCategories.CommodityCategoryID = Commodities.CommodityCategoryID ";
                    queryString = queryString + "                           ) UNIONPromotions ";
                    queryString = queryString + "                   ) OVERPARTITIONPromotions ON Commodities.CommodityID = OVERPARTITIONPromotions.CommodityID AND OVERPARTITIONPromotions.RowNo = 1 ";
                    
                    queryString = queryString + "       END ";

                }

                queryString = queryString + "       " + this.GetCommoditiesInWarehousesBuildSQL(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, getSavedData, includeCommoditiesOutOfStock, isUsingWarehouseBalancePrice) + "\r\n";
                queryString = queryString + "       END ";
            }
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure(storedProcedureName, queryString);
        }

        private string GetCommoditiesInWarehousesGETAvailable(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData)
        {
            string queryString = "";

            if (withCommoditiesInGoodsReceipts)
            {//GET QuantityAvailable IN GoodsReceiptDetails FOR GlobalEnums.CommodityTypeID.Vehicles
                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, ROUND(Quantity - QuantityIssue, 0) AS QuantityAvailable " + "\r\n";
                queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "               WHERE           CommodityTypeID IN (" + (int)GlobalEnums.CommodityTypeID.Vehicles + ") AND ROUND(Quantity - QuantityIssue, 0) > 0 AND WarehouseID IN (SELECT WarehouseID FROM Warehouses WHERE LocationID = @LocationID) AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (withCommoditiesInWarehouses)
            {
                //GET QuantityEndREC IN WarehouseJournal
                SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSalesPortalEntities);

                queryString = queryString + "               DECLARE @WarehouseIDList varchar(555)        DECLARE @CommodityIDList varchar(3999) " + "\r\n";
                queryString = queryString + "               SELECT  @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar) FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";
                queryString = queryString + "               SELECT  @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar) FROM @Commodities FOR XML PATH('')) ,1,1,'') " + "\r\n";


                queryString = queryString + "               " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

                queryString = queryString + "               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "               SELECT          WarehouseID, CommodityID, QuantityBegin AS QuantityAvailable " + "\r\n"; //QuantityEndREC
                queryString = queryString + "               FROM            @WarehouseJournalTable " + "\r\n";

                queryString = queryString + "               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
            }

            if (getSavedData)
            {//GET SavedData
                queryString = queryString + "               IF (@GoodsIssueID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            GoodsIssueDetails " + "\r\n";
                queryString = queryString + "                               WHERE           GoodsIssueID = @GoodsIssueID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "               IF (@StockTransferID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            StockTransferDetails " + "\r\n";
                queryString = queryString + "                               WHERE           StockTransferID = @StockTransferID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "             IF (@InventoryAdjustmentID > 0) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                               INSERT INTO     @CommoditiesAvailable (WarehouseID, CommodityID, QuantityAvailable) " + "\r\n";
                queryString = queryString + "                               SELECT          WarehouseID, CommodityID, -Quantity AS QuantityAvailable " + "\r\n";
                queryString = queryString + "                               FROM            InventoryAdjustmentDetails " + "\r\n";
                queryString = queryString + "                               WHERE           InventoryAdjustmentID = @InventoryAdjustmentID AND LocationID = @LocationID AND CommodityID IN (SELECT CommodityID FROM @Commodities) " + "\r\n";

                queryString = queryString + "                               SET             @HasCommoditiesAvailable = @HasCommoditiesAvailable + @@ROWCOUNT " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";
            }

            queryString = queryString + "               UPDATE @CommoditiesAvailable SET Bookable = 1 WHERE WarehouseID IN (SELECT WarehouseID FROM CustomerWarehouses WHERE CustomerID = @CustomerID AND InActive = 0)" + "\r\n";


            return queryString;
        }

        private string GetCommoditiesInWarehousesBuildSQL(bool withCommoditiesInGoodsReceipts, bool withCommoditiesInWarehouses, bool getSavedData, bool includeCommoditiesOutOfStock, bool isUsingWarehouseBalancePrice)
        {
            string queryString = "                  BEGIN " + "\r\n";

            queryString = queryString + "               " + this.GetCommoditiesInWarehousesGETAvailable(withCommoditiesInGoodsReceipts, withCommoditiesInWarehouses, getSavedData) + "\r\n";


            if (isUsingWarehouseBalancePrice)
            {
                queryString = queryString + "       DECLARE @CurrentWarehouseBalancePrice TABLE (CommodityID int NOT NULL, UnitPrice decimal(18, 2) NOT NULL)" + "\r\n";
                queryString = queryString + "       INSERT INTO @CurrentWarehouseBalancePrice SELECT CommodityID, UnitPrice FROM (SELECT EntryDate, CommodityID, UnitPrice, ROW_NUMBER() OVER (PARTITION BY CommodityID ORDER BY EntryDate DESC) AS RowNo FROM WarehouseBalancePrice WHERE CommodityID IN (SELECT CommodityID FROM @Commodities) AND EntryDate <= dbo.EOMONTHTIME(@EntryDate, 9999)) WarehouseBalancePriceWithRowNo WHERE RowNo = 1" + "\r\n";
            }


            if (!includeCommoditiesOutOfStock)
                queryString = queryString + "       IF (@HasCommoditiesAvailable > 0) " + "\r\n";

            queryString = queryString + "                   SELECT          " + (!includeCommoditiesOutOfStock ? "" : "TOP 50") + "Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + (!isUsingWarehouseBalancePrice ? "Commodities.GrossPrice" : "ROUND(CAST(ISNULL(CurrentWarehouseBalancePrice.UnitPrice, 0) AS decimal(18, 2)) * (1 + CommodityCategories.VATPercent/100), " + (int)GlobalEnums.rndAmount + ") AS GrossPrice") + ", Commodities.DiscountPercent, CommodityCategories.VATPercent, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.WarehouseID" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.WarehouseID) AS WarehouseID") + ", " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "Warehouses.Code" + (!includeCommoditiesOutOfStock ? "" : ", DEFAULTWarehouses.Code)") + " AS WarehouseCode, " + (!includeCommoditiesOutOfStock ? "" : "ISNULL(") + "CommoditiesAvailable.QuantityAvailable" + (!includeCommoditiesOutOfStock ? "" : ", CAST(0 AS decimal(18, 2)) ) AS QuantityAvailable") + ", " + (!includeCommoditiesOutOfStock ? "CommoditiesAvailable.Bookable" : "CAST(1 AS bit) AS Bookable") + " \r\n";
            queryString = queryString + "                   FROM            @Commodities Commodities INNER JOIN " + "\r\n";
            queryString = queryString + "                                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                  (SELECT WarehouseID, CommodityID, SUM(QuantityAvailable) AS QuantityAvailable, Bookable FROM @CommoditiesAvailable GROUP BY WarehouseID, CommodityID, Bookable) CommoditiesAvailable ON Commodities.CommodityID = CommoditiesAvailable.CommodityID " + (!includeCommoditiesOutOfStock ? "INNER JOIN" : "LEFT JOIN") + "\r\n";
            queryString = queryString + "                                   Warehouses ON CommoditiesAvailable.WarehouseID = Warehouses.WarehouseID " + "\r\n";


            if (isUsingWarehouseBalancePrice)
                queryString = queryString + "                               LEFT JOIN @CurrentWarehouseBalancePrice CurrentWarehouseBalancePrice ON Commodities.CommodityID = CurrentWarehouseBalancePrice.CommodityID " + "\r\n";

            if (includeCommoditiesOutOfStock)
                queryString = queryString + "                               LEFT JOIN (SELECT TOP 1 WarehouseID, Code FROM Warehouses WHERE LocationID = @LocationID) DEFAULTWarehouses ON DEFAULTWarehouses.WarehouseID <> 0 " + "\r\n";

            if (!includeCommoditiesOutOfStock)
            {
                queryString = queryString + "       ELSE " + "\r\n";
                queryString = queryString + "               " + this.GetCommoditiesInWarehousesRETURNNothing() + "\r\n";
            }

            queryString = queryString + "           END " + "\r\n";

            return queryString;
        }




        private string GetCommoditiesInWarehousesRETURNNothing()
        {
            string queryString = "                          BEGIN " + "\r\n";

            queryString = queryString + "                       SELECT      CommodityID, Code AS CommodityCode, Name AS CommodityName, Commodities.CommodityTypeID, Commodities.GrossPrice, 0.0 AS DiscountPercent, 0.0 AS VATPercent, 0 AS WarehouseID, '' AS WarehouseCode, CAST(0 AS decimal(18, 2)) AS QuantityAvailable " + "\r\n";
            queryString = queryString + "                       FROM        @Commodities Commodities " + "\r\n";
            queryString = queryString + "                       WHERE       CommodityID IS NULL " + "\r\n"; //ALWAYS RETURN NOTHING

            queryString = queryString + "                   END " + "\r\n";

            return queryString;
        }




        #region X


        private void GetDeliveryAdviceViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSalesPortalEntities);

            queryString = " @DeliveryAdviceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID FROM DeliveryAdvices WHERE DeliveryAdviceID = @DeliveryAdviceID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM Warehouses WHERE LocationID = @LocationID FOR XML PATH('')) ,1,1,'') " + "\r\n";//The best way is get the @WarehouseIDList from table DeliveryAdviceDetails, but we don't want the stored procedure read from DeliveryAdviceDetails to save the resource
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdviceDetails.DeliveryAdviceDetailID, DeliveryAdviceDetails.DeliveryAdviceID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, DeliveryAdviceDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, " + "\r\n";
            queryString = queryString + "                   ROUND(CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) + DeliveryAdviceDetails.Quantity, 0) AS QuantityAvailable, DeliveryAdviceDetails.Quantity, DeliveryAdviceDetails.ListedPrice, DeliveryAdviceDetails.DiscountPercent, DeliveryAdviceDetails.UnitPrice, DeliveryAdviceDetails.VATPercent, DeliveryAdviceDetails.GrossPrice, DeliveryAdviceDetails.Amount, DeliveryAdviceDetails.VATAmount, DeliveryAdviceDetails.GrossAmount, DeliveryAdviceDetails.IsBonus, DeliveryAdviceDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        DeliveryAdviceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID AND DeliveryAdviceDetails.CommodityID = Commodities.CommodityID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryString = queryString + "                  (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n"; //SUM(QuantityBeginQuantityEndREC) 

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetDeliveryAdviceViewDetails", queryString);
        }

        private void DeliveryAdviceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            //queryString = queryString + "       EXEC        DeliveryAdviceUpdateQuotation @EntityID, @SaveRelativeOption " + "\r\n";

            //queryString = queryString + "       SET         @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            //queryString = queryString + "       EXEC        UpdateWarehouseBalance @SaveRelativeOption, 0, @EntityID, 0, 0 ";

            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceSaveRelative", queryString);
        }

        private void DeliveryAdvicePostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM DeliveryAdvices INNER JOIN DeliveryAdvices AS ServiceInvoices ON DeliveryAdvices.DeliveryAdviceID = @EntityID AND DeliveryAdvices.ServiceInvoiceID = ServiceInvoices.DeliveryAdviceID AND DeliveryAdvices.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdvicePostSaveValidate", queryArray);
        }







        private void DeliveryAdviceEditable()
        {
            string[] queryArray = new string[0];//BE CAUTION: DeliveryAdvice SHOULD BE NOT ALLOWED TO CHANGE ServiceInvoiceID BY USER, IN ORDER THIS Procedure CAN CONTROL DeliveryAdviceEditable VIA ServiceInvoiceID. IF USER REALLY NEED TO CHANGE: PLEASE USE THIS WORKARROUND INSTEAD: REQUEST USER TO DELETE THIS DeliveryAdvice, AND THEN CREATE NEW ONE WITH NEW FOREIGN KEY ServiceInvoiceID!

            //queryArray[0] = "                 DECLARE @ServiceInvoiceID Int " + "\r\n";
            //queryArray[0] = queryArray[0] + " SELECT TOP 1 @ServiceInvoiceID = ServiceInvoiceID FROM DeliveryAdvices WHERE DeliveryAdviceID = @EntityID " + "\r\n";
            //queryArray[0] = queryArray[0] + " IF NOT @ServiceInvoiceID IS NULL" + "\r\n";
            //queryArray[0] = queryArray[0] + "       SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdvices WHERE DeliveryAdviceID = @ServiceInvoiceID AND IsFinished = 1 " + "\r\n";

            //queryArray[1] = " SELECT TOP 1 @FoundEntity = DeliveryAdviceID FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @EntityID AND NOT AccountInvoiceID IS NULL ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("DeliveryAdviceEditable", queryArray);
        }


        private void DeliveryAdviceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("DeliveryAdvices", "DeliveryAdviceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.DeliveryAdvice));
            this.totalSalesPortalEntities.CreateTrigger("DeliveryAdviceInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
