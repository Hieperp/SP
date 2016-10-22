using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{

    public class GoodsIssue
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public GoodsIssue(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsIssueIndexes();

            this.GetPendingDeliveryAdvices();
            this.GetPendingDeliveryAdviceCustomers();

            this.GetGoodsIssueViewDetails();
            this.GoodsIssueSaveRelative();
            this.GoodsIssuePostSaveValidate();

            this.GoodsIssueEditable();

            this.GoodsIssueInitReference();
        }

        private void GetGoodsIssueIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, CAST(GoodsIssues.EntryDate AS DATE) AS EntryDate, GoodsIssues.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdvices.EntryDate AS DeliveryAdviceEntryDate, GoodsIssues.TotalQuantity, GoodsIssues.TotalGrossAmount, GoodsIssues.Description " + "\r\n";
            queryString = queryString + "       FROM        GoodsIssues INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate AND GoodsIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsIssues.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Customers ON GoodsIssues.CustomerID = Customers.CustomerID INNER JOIN" + "\r\n";
            queryString = queryString + "                   DeliveryAdvices ON GoodsIssues.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueIndexes", queryString);
        }

        private void GetPendingDeliveryAdvices()
        {
            string queryString = " @LocationID int, @GoodsIssueID int, @DeliveryAdviceReference nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          DeliveryAdvices.DeliveryAdviceID, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdvices.EntryDate AS DeliveryAdviceEntryDate, DeliveryAdvices.Description, DeliveryAdvices.Remarks, " + "\r\n";
            queryString = queryString + "                       DeliveryAdvices.CustomerID, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            DeliveryAdvices INNER JOIN Customers ON (@DeliveryAdviceReference = '' OR DeliveryAdvices.Reference LIKE '%' + @DeliveryAdviceReference + '%') AND DeliveryAdvices.LocationID = @LocationID AND DeliveryAdvices.CustomerID = Customers.CustomerID INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           DeliveryAdvices.DeliveryAdviceID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT DeliveryAdviceID FROM DeliveryAdviceDetails WHERE ROUND(Quantity - QuantityIssue, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT DeliveryAdviceID FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID)  " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdvices", queryString);
        }

        private void GetPendingDeliveryAdviceCustomers()
        {
            string queryString = " @LocationID int, @GoodsIssueID int, @CustomerName nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            Customers INNER JOIN EntireTerritories ON (@CustomerName = '' OR Customers.Name LIKE '%' + @CustomerName + '%') AND Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           CustomerID IN   " + "\r\n";

            queryString = queryString + "                      (SELECT CustomerID FROM DeliveryAdviceDetails WHERE LocationID = @LocationID AND ROUND(Quantity - QuantityIssue, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT CustomerID FROM GoodsIssues WHERE GoodsIssueID = @GoodsIssueID) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdviceCustomers", queryString);
        }



        private void GetGoodsIssueViewDetails()
        {
            string queryString; string queryEdit; string queryNew;

            queryNew = "                SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, 0 AS GoodsIssueDetailID, 0 AS GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, " + "\r\n";
            queryNew = queryNew + "                     DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, 0) AS QuantityRemains, CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) AS QuantityAvailable, " + "\r\n";
            queryNew = queryNew + "                     0.0 AS Quantity, DeliveryAdviceDetails.ListedPrice, DeliveryAdviceDetails.DiscountPercent, DeliveryAdviceDetails.UnitPrice, DeliveryAdviceDetails.VATPercent, DeliveryAdviceDetails.GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, DeliveryAdviceDetails.IsBonus, DeliveryAdviceDetails.Remarks " + "\r\n";

            queryNew = queryNew + "     FROM            DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Commodities ON DeliveryAdviceDetails.CommodityID = Commodities.CommodityID AND ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, 0) > 0  INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryNew = queryNew + "                     (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n"; 

            queryNew = queryNew + "     WHERE           (@DeliveryAdviceID = 0 OR DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID) AND (@CustomerID = 0 OR DeliveryAdviceDetails.CustomerID = @CustomerID) " + "\r\n"; //AND DeliveryAdviceDetailMaster.Approved = 1 


            queryEdit = "               SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, " + "\r\n";
            queryEdit = queryEdit + "                   DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity, 0) AS QuantityRemains, ROUND(CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) + GoodsIssueDetails.Quantity, 0) AS QuantityAvailable, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.Amount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, GoodsIssueDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssueDetails ON GoodsIssueDetails.GoodsIssueID = @GoodsIssueID AND DeliveryAdviceDetails.DeliveryAdviceDetailID = GoodsIssueDetails.DeliveryAdviceDetailID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryEdit = queryEdit + "                   (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n"; 

            queryEdit = queryEdit + "   WHERE           (@DeliveryAdviceID = 0 OR DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID) AND (@CustomerID = 0 OR DeliveryAdviceDetails.CustomerID = @CustomerID) " + "\r\n";


            SqlProgrammability.Inventories.Inventories inventories = new Inventories(this.totalSalesPortalEntities);

            queryString = " @GoodsIssueID Int, @DeliveryAdviceID Int, @CustomerID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";


            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)      DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";

            queryString = queryString + "       IF (@GoodsIssueID > 0) ";
            queryString = queryString + "           SELECT  @EntryDate = EntryDate, @LocationID = LocationID FROM GoodsIssues WHERE GoodsIssueID = @GoodsIssueID " + "\r\n";
            queryString = queryString + "       IF (@EntryDate IS NULL) ";
            queryString = queryString + "           SELECT  @EntryDate = EntryDate, @LocationID = LocationID FROM DeliveryAdvices WHERE DeliveryAdviceID = @DeliveryAdviceID " + "\r\n";

            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@WarehouseJournalTable", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0") + "\r\n";


            queryString = queryString + " IF (@GoodsIssueID <= 0) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryNew + "\r\n";
            queryString = queryString + "           ORDER BY DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";
            queryString = queryString + " ELSE " + "\r\n";

            queryString = queryString + "       IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryNew + " AND DeliveryAdviceDetails.DeliveryAdviceDetailID NOT IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID) " + "\r\n";
            queryString = queryString + "               UNION ALL " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY DeliveryAdviceDetails.EntryDate, DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueViewDetails", queryString);

        }


        private void GoodsIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE          DeliveryAdviceDetails " + "\r\n";
            queryString = queryString + "       SET             DeliveryAdviceDetails.QuantityIssue = ROUND(DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            GoodsIssueDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       DeliveryAdviceDetails ON GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceDetailID = DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";

            //queryString = queryString + "       SET             @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            //queryString = queryString + "       EXEC            UpdateWarehouseBalance @SaveRelativeOption, 0, 0, @EntityID, 0 ";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueSaveRelative", queryString);

        }

        private void GoodsIssuePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'D.A Date: ' + CAST(DeliveryAdvices.EntryDate AS nvarchar) FROM GoodsIssueDetails INNER JOIN DeliveryAdvices ON GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID AND GoodsIssueDetails.EntryDate < DeliveryAdvices.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityIssue, 0) AS nvarchar) FROM DeliveryAdviceDetails WHERE (ROUND(Quantity - QuantityIssue, 0) < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssuePostSaveValidate", queryArray);
        }


        private void GoodsIssueEditable()
        {
            string[] queryArray = new string[1];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = VoucherID FROM GoodsReceipts WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.GoodsIssue + " AND VoucherID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssueEditable", queryArray);
        }

        private void GoodsIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsIssues", "GoodsIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsIssue));
            this.totalSalesPortalEntities.CreateTrigger("GoodsIssueInitReference", simpleInitReference.CreateQuery());
        }


    }
}
