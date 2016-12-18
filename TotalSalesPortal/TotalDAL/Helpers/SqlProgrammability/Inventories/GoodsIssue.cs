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
            this.GoodsIssueApproved();
                       
            this.GoodsIssueToggleApproved();
                       
            this.GoodsIssueInitReference();

            this.GoodsIssueSheet();
        }

        private void GetGoodsIssueIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, CAST(GoodsIssues.EntryDate AS DATE) AS EntryDate, GoodsIssues.Reference, Locations.Code AS LocationCode, Customers.Code + ', ' + Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, DeliveryAdvices.Reference AS DeliveryAdviceReference, ISNULL(DeliveryAdvices.EntryDate, NULL) AS DeliveryAdviceEntryDate, GoodsIssues.TotalQuantity, GoodsIssues.TotalFreeQuantity, GoodsIssues.TotalGrossAmount, GoodsIssues.Description " + "\r\n";
            queryString = queryString + "       FROM        GoodsIssues INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate AND GoodsIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsIssues.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Customers ON GoodsIssues.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   DeliveryAdvices ON GoodsIssues.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueIndexes", queryString);
        }

        private void GetPendingDeliveryAdvices()
        {
            string queryString = " @LocationID int, @GoodsIssueID int, @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          DeliveryAdvices.DeliveryAdviceID, DeliveryAdvices.Reference AS DeliveryAdviceReference, DeliveryAdvices.EntryDate AS DeliveryAdviceEntryDate, DeliveryAdvices.Description, DeliveryAdvices.Remarks, " + "\r\n";
            queryString = queryString + "                       DeliveryAdvices.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       DeliveryAdvices.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.AddressNo AS ReceiverAddressNo, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            DeliveryAdvices " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON (@SearchText = '' OR DeliveryAdvices.Reference LIKE '%' + @SearchText + '%' OR Customers.Code LIKE '%' + @SearchText + '%' OR Customers.Name LIKE '%' + @SearchText + '%' OR Customers.VATCode LIKE '%' + @SearchText + '%') AND DeliveryAdvices.LocationID = @LocationID AND DeliveryAdvices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON DeliveryAdvices.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           DeliveryAdvices.DeliveryAdviceID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT DeliveryAdviceID FROM DeliveryAdviceDetails WHERE InActive = 0 AND InActivePartial = 0 AND InActiveIssue = 0 AND ROUND(Quantity - QuantityIssue, 0) > 0 OR ROUND(FreeQuantity - FreeQuantityIssue, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT DeliveryAdviceID FROM GoodsIssueDetails WHERE GoodsIssueID = @GoodsIssueID)  " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdvices", queryString);
        }

        private void GetPendingDeliveryAdviceCustomers()
        {
            string queryString = " @LocationID int, @GoodsIssueID int, @SearchText nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.AddressNo AS ReceiverAddressNo, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, ReceiverID FROM " + "\r\n";
            queryString = queryString + "                              (SELECT CustomerID, ReceiverID FROM DeliveryAdviceDetails WHERE InActive = 0 AND InActivePartial = 0 AND InActiveIssue = 0 AND LocationID = @LocationID AND (ROUND(Quantity - QuantityIssue, 0) > 0 OR ROUND(FreeQuantity - FreeQuantityIssue, 0) > 0) " + "\r\n";
            queryString = queryString + "                               UNION ALL " + "\r\n";
            queryString = queryString + "                               SELECT CustomerID, ReceiverID FROM GoodsIssues WHERE GoodsIssueID = @GoodsIssueID) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                      )CustomerReceiverUNION  " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON (@SearchText = '' OR Customers.Code LIKE '%' + @SearchText + '%' OR Customers.Name LIKE '%' + @SearchText + '%' OR Customers.VATCode LIKE '%' + @SearchText + '%') AND CustomerReceiverUNION.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON CustomerReceiverUNION.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingDeliveryAdviceCustomers", queryString);
        }



        private void GetGoodsIssueViewDetails()
        {
            string queryString; string queryEdit; string queryNew;

            queryNew = "                SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, 0 AS GoodsIssueDetailID, 0 AS GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, NULL AS VoidTypeID, CAST(NULL AS nvarchar(50)) AS VoidTypeCode, CAST(NULL AS nvarchar(50)) AS VoidTypeName, NULL AS VoidClassID, " + "\r\n";
            queryNew = queryNew + "                     DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, 0) AS QuantityRemains, ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue, 0) AS FreeQuantityRemains, IIF(Commodities.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + ", DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity, CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2))) AS QuantityAvailable, DeliveryAdviceDetails.ControlFreeQuantity, " + "\r\n";
            queryNew = queryNew + "                     0.0 AS Quantity, 0.0 AS FreeQuantity, DeliveryAdviceDetails.ListedPrice, DeliveryAdviceDetails.DiscountPercent, DeliveryAdviceDetails.UnitPrice, DeliveryAdviceDetails.VATPercent, DeliveryAdviceDetails.GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, DeliveryAdviceDetails.IsBonus, DeliveryAdviceDetails.Remarks " + "\r\n";

            queryNew = queryNew + "     FROM            DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Commodities ON DeliveryAdviceDetails.CommodityID = Commodities.CommodityID AND DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0 AND (ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue, 0) > 0 OR ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue, 0) > 0)  INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryNew = queryNew + "                     (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryNew = queryNew + "     WHERE           (@DeliveryAdviceID = 0 OR DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID) AND ((@CustomerID = 0 AND @ReceiverID = 0) OR (DeliveryAdviceDetails.CustomerID = @CustomerID AND DeliveryAdviceDetails.ReceiverID = @ReceiverID)) " + "\r\n"; //AND DeliveryAdviceDetailMaster.Approved = 1 


            queryEdit = "               SELECT          DeliveryAdviceDetails.DeliveryAdviceID, DeliveryAdviceDetails.EntryDate AS DeliveryAdviceDate, GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.GoodsIssueID, DeliveryAdviceDetails.DeliveryAdviceDetailID, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryEdit = queryEdit + "                   DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, ROUND(DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity, 0) AS QuantityRemains, ROUND(DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue + GoodsIssueDetails.FreeQuantity, 0) AS FreeQuantityRemains, IIF(Commodities.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + ", DeliveryAdviceDetails.Quantity + DeliveryAdviceDetails.FreeQuantity, ROUND(CAST(ISNULL(CommoditiesAvailable.QuantityAvailable, 0) AS decimal(18, 2)) + GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity, 0)) AS QuantityAvailable, DeliveryAdviceDetails.ControlFreeQuantity, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.Amount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, GoodsIssueDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssueDetails ON GoodsIssueDetails.GoodsIssueID = @GoodsIssueID AND DeliveryAdviceDetails.DeliveryAdviceDetailID = GoodsIssueDetails.DeliveryAdviceDetailID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Warehouses ON DeliveryAdviceDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN" + "\r\n";
            queryEdit = queryEdit + "                   VoidTypes ON GoodsIssueDetails.VoidTypeID = VoidTypes.VoidTypeID LEFT JOIN" + "\r\n";
            queryEdit = queryEdit + "                   (SELECT WarehouseID, CommodityID, SUM(QuantityBegin) AS QuantityAvailable FROM @WarehouseJournalTable GROUP BY WarehouseID, CommodityID) CommoditiesAvailable ON DeliveryAdviceDetails.WarehouseID = CommoditiesAvailable.WarehouseID AND DeliveryAdviceDetails.CommodityID = CommoditiesAvailable.CommodityID " + "\r\n";

            queryEdit = queryEdit + "   WHERE           (@DeliveryAdviceID = 0 OR DeliveryAdviceDetails.DeliveryAdviceID = @DeliveryAdviceID) AND ((@CustomerID = 0 AND @ReceiverID = 0) OR (DeliveryAdviceDetails.CustomerID = @CustomerID AND DeliveryAdviceDetails.ReceiverID = @ReceiverID)) " + "\r\n";


            SqlProgrammability.Inventories.Inventories inventories = new Inventories(this.totalSalesPortalEntities);

            queryString = " @GoodsIssueID Int, @DeliveryAdviceID Int, @CustomerID Int, @ReceiverID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";


            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999) " + "\r\n";

            queryString = queryString + "       IF (@GoodsIssueID > 0) ";
            queryString = queryString + "           SELECT  @EntryDate = EntryDate FROM GoodsIssues WHERE GoodsIssueID = @GoodsIssueID " + "\r\n";
            queryString = queryString + "       IF (@EntryDate IS NULL) ";
            queryString = queryString + "           SELECT  @EntryDate = GetDate() " + "\r\n";

            queryString = queryString + "       IF (@DeliveryAdviceID > 0) ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM DeliveryAdviceDetails WHERE DeliveryAdviceID = @DeliveryAdviceID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           SELECT      @WarehouseIDList = '', @CommodityIDList = '' " + "\r\n";

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
            queryString = queryString + "       SET             DeliveryAdviceDetails.QuantityIssue = ROUND(DeliveryAdviceDetails.QuantityIssue + GoodsIssueDetails.Quantity * @SaveRelativeOption, 0), DeliveryAdviceDetails.FreeQuantityIssue = ROUND(DeliveryAdviceDetails.FreeQuantityIssue + GoodsIssueDetails.FreeQuantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            GoodsIssueDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       DeliveryAdviceDetails ON ((DeliveryAdviceDetails.InActive = 0 AND DeliveryAdviceDetails.InActivePartial = 0 AND DeliveryAdviceDetails.InActiveIssue = 0) OR @SaveRelativeOption = -1) AND (GoodsIssueDetails.Quantity <> 0 OR GoodsIssueDetails.FreeQuantity <> 0) AND GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceDetailID = DeliveryAdviceDetails.DeliveryAdviceDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = (SELECT COUNT(*) FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID AND (GoodsIssueDetails.Quantity <> 0 OR GoodsIssueDetails.FreeQuantity <> 0)) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      DeliveryAdvices SET DeliveryAdvices.TotalQuantityIssue = TotalDeliveryAdviceDetails.TotalQuantityIssue, DeliveryAdvices.TotalFreeQuantityIssue = TotalDeliveryAdviceDetails.TotalFreeQuantityIssue FROM DeliveryAdvices INNER JOIN (SELECT DeliveryAdviceID, SUM(QuantityIssue) AS TotalQuantityIssue, SUM(FreeQuantityIssue) AS TotalFreeQuantityIssue FROM DeliveryAdviceDetails WHERE DeliveryAdviceID IN (SELECT DeliveryAdviceID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) GROUP BY DeliveryAdviceID) TotalDeliveryAdviceDetails ON DeliveryAdvices.DeliveryAdviceID = TotalDeliveryAdviceDetails.DeliveryAdviceID; " + "\r\n";

            queryString = queryString + "               UPDATE      DeliveryAdviceDetails SET DeliveryAdviceDetails.InActiveIssue = 0 WHERE DeliveryAdviceDetails.DeliveryAdviceDetailID IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) " + "\r\n";
            queryString = queryString + "               UPDATE      DeliveryAdviceDetails SET DeliveryAdviceDetails.InActiveIssue = DeliveryAdviceInActiveIssue.InActiveIssue " + "\r\n";
            queryString = queryString + "               FROM        DeliveryAdviceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                          (SELECT GoodsIssueDetails.DeliveryAdviceDetailID, MAX(CAST(VoidTypes.InActive AS int)) AS InActiveIssue FROM GoodsIssueDetails INNER JOIN VoidTypes ON GoodsIssueDetails.VoidTypeID = VoidTypes.VoidTypeID AND GoodsIssueDetails.GoodsIssueID <> (@EntityID * -@SaveRelativeOption) AND GoodsIssueDetails.DeliveryAdviceDetailID IN (SELECT DeliveryAdviceDetailID FROM GoodsIssueDetails WHERE GoodsIssueID = @EntityID) GROUP BY GoodsIssueDetails.DeliveryAdviceDetailID) AS DeliveryAdviceInActiveIssue " + "\r\n"; //THIS GoodsIssueDetails.GoodsIssueID <> (@EntityID * -@SaveRelativeOption) => EXCLUSIVE @EntityID WHEN UNDO
            queryString = queryString + "                           ON DeliveryAdviceDetails.DeliveryAdviceDetailID = DeliveryAdviceInActiveIssue.DeliveryAdviceDetailID " + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = 'Đề nghị giao hàng không tồn tại hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            //queryString = queryString + "       SET             @SaveRelativeOption = -@SaveRelativeOption" + "\r\n";
            //queryString = queryString + "       EXEC            UpdateWarehouseBalance @SaveRelativeOption, 0, 0, @EntityID, 0 ";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueSaveRelative", queryString);

        }

        private void GoodsIssuePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'D.A Date: ' + CAST(DeliveryAdvices.EntryDate AS nvarchar) FROM GoodsIssueDetails INNER JOIN DeliveryAdvices ON GoodsIssueDetails.GoodsIssueID = @EntityID AND GoodsIssueDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID AND GoodsIssueDetails.EntryDate < DeliveryAdvices.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over quantity: ' + CAST(ROUND(Quantity - QuantityIssue, 0) AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(FreeQuantity - FreeQuantityIssue, 0) AS nvarchar) FROM DeliveryAdviceDetails WHERE (ROUND(Quantity - QuantityIssue, 0) < 0) OR (ROUND(FreeQuantity - FreeQuantityIssue, 0) < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssuePostSaveValidate", queryArray);
        }


        private void GoodsIssueApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM GoodsIssues WHERE GoodsIssueID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssueApproved", queryArray);
        }


        private void GoodsIssueEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM ReceiptDetails WHERE GoodsIssueID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsIssueID FROM AccountInvoiceDetails WHERE GoodsIssueID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("GoodsIssueEditable", queryArray);
        }


        private void GoodsIssueToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsIssues  SET Approved = @Approved, ApprovedDate = GetDate() WHERE GoodsIssueID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           UPDATE          GoodsIssueDetails  SET Approved = @Approved WHERE GoodsIssueID = @EntityID ; " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = 'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueToggleApproved", queryString);
        }


        private void GoodsIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsIssues", "GoodsIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsIssue));
            this.totalSalesPortalEntities.CreateTrigger("GoodsIssueInitReference", simpleInitReference.CreateQuery());
        }


        private void GoodsIssueSheet()
        {
            string queryString = " @GoodsIssueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalGoodsIssueID int    SET @LocalGoodsIssueID = @GoodsIssueID" + "\r\n";

            queryString = queryString + "       SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.Telephone AS ReceiverTelephone, Receivers.AttentionName AS ReceiverAttentionName, Receivers.AddressNo AS ReceiverAddressNo, " + "\r\n";
            queryString = queryString + "                       Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.GrossAmount " + "\r\n";
            
            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsIssueDetails ON GoodsIssues.GoodsIssueID = @LocalGoodsIssueID AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers AS Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GoodsIssueSheet", queryString);

        }

    }
}
