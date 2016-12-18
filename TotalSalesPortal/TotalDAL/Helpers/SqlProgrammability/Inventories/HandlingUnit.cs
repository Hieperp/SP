using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class HandlingUnit
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public HandlingUnit(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetHandlingUnitIndexes();

            this.GetHandlingUnitViewDetails();

            this.GetHUPendingGoodsIssues();
            this.GetHUPendingGoodsIssueCustomers();
            this.GetHUPendingGoodsIssueDetails();

            this.HandlingUnitSaveRelative();
            this.HandlingUnitPostSaveValidate();

            this.HandlingUnitInitReference();
        }

        private void GetHandlingUnitIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      HandlingUnits.HandlingUnitID, CAST(HandlingUnits.EntryDate AS DATE) AS EntryDate, HandlingUnits.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, HandlingUnits.Description, HandlingUnits.TotalQuantity, HandlingUnits.TotalWeight, HandlingUnits.RealWeight " + "\r\n";
            queryString = queryString + "       FROM        HandlingUnits INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON HandlingUnits.EntryDate >= @FromDate AND HandlingUnits.EntryDate <= @ToDate AND HandlingUnits.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.HandlingUnit + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = HandlingUnits.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON HandlingUnits.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitIndexes", queryString);
        }

        private void GetHandlingUnitViewDetails()
        {
            string queryString;

            queryString = " @HandlingUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      HandlingUnitDetails.HandlingUnitDetailID, HandlingUnitDetails.HandlingUnitID, HandlingUnitDetails.GoodsIssueID, HandlingUnitDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   HandlingUnitDetails.Quantity, HandlingUnitDetails.Weight, HandlingUnitDetails.TotalWeight, HandlingUnitDetails.Remarks" + "\r\n";

            queryString = queryString + "       FROM        HandlingUnitDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails ON HandlingUnitDetails.HandlingUnitID = @HandlingUnitID AND HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHandlingUnitViewDetails", queryString);
        }










        private void GetHUPendingGoodsIssues()
        {
            string queryString = " @LocationID int, @HandlingUnitID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       Receivers.Code AS GoodsIssueReceiverCode, Receivers.Name AS GoodsIssueReceiverName " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            queryString = queryString + "       WHERE           GoodsIssues.GoodsIssueID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT GoodsIssueID FROM GoodsIssueDetails WHERE ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT GoodsIssueID FROM HandlingUnitDetails WHERE HandlingUnitID = @HandlingUnitID)  " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHUPendingGoodsIssues", queryString);
        }

        private void GetHUPendingGoodsIssueCustomers()
        {
            string queryString = " @LocationID int, @HandlingUnitID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "                       Receivers.CustomerID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Receivers.VATCode AS ReceiverVATCode, Receivers.AttentionName AS ReceiverAttentionName, Receivers.Telephone AS ReceiverTelephone, Receivers.AddressNo AS ReceiverAddressNo, ReceiverEntireTerritories.EntireName AS ReceiverEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, ReceiverID FROM " + "\r\n";
            queryString = queryString + "                              (SELECT CustomerID, ReceiverID FROM GoodsIssueDetails WHERE LocationID = @LocationID AND ROUND(Quantity + FreeQuantity - QuantityHandlingUnit, 0) > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL " + "\r\n";
            queryString = queryString + "                               SELECT CustomerID, ReceiverID FROM HandlingUnit WHERE HandlingUnitID = @HandlingUnitID) CustomerReceiverPENDING " + "\r\n";
            queryString = queryString + "                      )CustomerReceiverUNION  " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerReceiverUNION.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON CustomerReceiverUNION.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ReceiverEntireTerritories ON Receivers.TerritoryID = ReceiverEntireTerritories.TerritoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHUPendingGoodsIssueCustomers", queryString);
        }



        private void GetHUPendingGoodsIssueDetails()
        {
            string queryString;

            queryString = " @HandlingUnitID Int, @GoodsIssueID Int, @CustomerID Int, @ReceiverID Int, @CommodityTypeID int, @AspUserID nvarchar(128), @LocationID Int, @FromDate DateTime, @ToDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssue(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetHUPendingGoodsIssueDetails", queryString);
        }

        private string GetPGIDsBuildSQLGoodsIssue(bool isGoodsIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueGoodsIssueDetailIDs(isGoodsIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPGIDsBuildSQLGoodsIssueGoodsIssueDetailIDs(isGoodsIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLGoodsIssueGoodsIssueDetailIDs(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@HandlingUnitID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPGIDsBuildSQLNew(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLNew(isGoodsIssueID, isGoodsIssueDetailIDs) + " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM HandlingUnitDetails WHERE HandlingUnitID = @HandlingUnitID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPGIDsBuildSQLEdit(isGoodsIssueID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLNew(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AddressNo, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, Commodities.Weight, 0.0 AS Amount, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "GoodsIssueDetails.CustomerID = @CustomerID AND GoodsIssueDetails.ReceiverID = @ReceiverID ") + " AND ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit, 0) > 0 AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.CommodityTypeID != " + (int)GlobalEnums.CommodityTypeID.Services + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssueDetails.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }

        private string GetPGIDsBuildSQLEdit(bool isGoodsIssueID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AddressNo, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, ROUND(GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   HandlingUnitDetails.Quantity, HandlingUnitDetails.Weight, HandlingUnitDetails.TotalWeight, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN HandlingUnitDetails ON HandlingUnitDetails.HandlingUnitID = @HandlingUnitID AND GoodsIssueDetails.GoodsIssueDetailID = HandlingUnitDetails.GoodsIssueDetailID" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Receivers ON GoodsIssueDetails.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            return queryString;
        }

        private void HandlingUnitSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE          GoodsIssueDetails " + "\r\n";
            queryString = queryString + "       SET             GoodsIssueDetails.QuantityHandlingUnit = ROUND(GoodsIssueDetails.QuantityHandlingUnit + HandlingUnitDetails.Quantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            HandlingUnitDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails ON HandlingUnitDetails.HandlingUnitID = @EntityID AND HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("HandlingUnitSaveRelative", queryString);
        }

        private void HandlingUnitPostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(GoodsIssueDetails.EntryDate AS nvarchar) FROM HandlingUnitDetails INNER JOIN GoodsIssueDetails ON HandlingUnitDetails.HandlingUnitID = @EntityID AND HandlingUnitDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND HandlingUnitDetails.EntryDate < GoodsIssueDetails.EntryDate ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("HandlingUnitPostSaveValidate", queryArray);
        }



        private void HandlingUnitInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("HandlingUnits", "HandlingUnitID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.HandlingUnit));
            this.totalSalesPortalEntities.CreateTrigger("HandlingUnitInitReference", simpleInitReference.CreateQuery());
        }
    }
}