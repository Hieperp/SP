using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Accounts
{
    public class AccountInvoice
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public AccountInvoice(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetAccountInvoiceIndexes();

            this.GetAccountInvoiceViewDetails();
            this.GetPendingGoodsIssues();

            this.AccountInvoiceSaveRelative();
            this.AccountInvoicePostSaveValidate();

            this.AccountInvoiceInitReference();
        }

        private void GetAccountInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, CAST(AccountInvoices.EntryDate AS DATE) AS EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, AccountInvoices.Description, AccountInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        AccountInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON AccountInvoices.EntryDate >= @FromDate AND AccountInvoices.EntryDate <= @ToDate AND AccountInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.AccountInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = AccountInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccountInvoiceIndexes", queryString);
        }

        private void GetAccountInvoiceViewDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.GoodsIssueID, AccountInvoiceDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.Amount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, AccountInvoiceDetails.Remarks" + "\r\n";           

            queryString = queryString + "       FROM        AccountInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccountInvoiceViewDetails", queryString);
        }

        private void GetPendingGoodsIssues()
        {
            string queryString;
            
            queryString = " @AccountInvoiceID Int, @GoodsIssueID Int, @CustomerID Int, @CommodityTypeID int, @AspUserID nvarchar(128), @LocationID Int, @FromDate DateTime, @ToDate DateTime, @GoodsIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssue(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingGoodsIssues", queryString);
        }
        

        private string GetPendingGoodsIssuesBuildSQLGoodsIssue(bool isGoodsIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomer(isGoodsIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomer(isGoodsIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLGoodsIssueCustomer(bool isGoodsIssueID, bool isCustomerID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CommodityTypeID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityType(isGoodsIssueID, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityType(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isCommodityTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(isGoodsIssueID, isCustomerID, isCommodityTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLGoodsIssueCustomerCommodityTypeGoodsIssueDetailIDs(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@AccountInvoiceID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingGoodsIssuesBuildSQLNew (isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingGoodsIssuesBuildSQLEdit(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingGoodsIssuesBuildSQLNew(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT GoodsIssueDetailID FROM AccountInvoiceDetails WHERE AccountInvoiceID = @AccountInvoiceID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingGoodsIssuesBuildSQLEdit(isGoodsIssueID, isCustomerID, isCommodityTypeID, isGoodsIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLNew(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AddressNo, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, 0.0 AS FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, GoodsIssueDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssueDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "") + (!isGoodsIssueID && isCustomerID ? " GoodsIssueDetails.CustomerID = @CustomerID " : "") + (!isGoodsIssueID && !isCustomerID ? " GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + " AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, 0) > 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, 0) > 0) AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 " + (isCommodityTypeID ? " AND Commodities.CommodityTypeID = @CommodityTypeID" : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + " INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsIssuesBuildSQLEdit(bool isGoodsIssueID, bool isCustomerID, bool isCommodityTypeID, bool isGoodsIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssueDetails.EntryDate, GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AddressNo, ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity, 0) AS QuantityRemains, ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity, 0) AS FreeQuantityRemains, GoodsIssueDetails.ControlFreeQuantity, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.VATPercent, AccountInvoiceDetails.GrossPrice, AccountInvoiceDetails.Amount, AccountInvoiceDetails.VATAmount, AccountInvoiceDetails.GrossAmount, AccountInvoiceDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";            

            queryString = queryString + "       FROM        GoodsIssueDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND GoodsIssueDetails.GoodsIssueDetailID = AccountInvoiceDetails.GoodsIssueDetailID AND " + (isGoodsIssueID ? " GoodsIssueDetails.GoodsIssueID = @GoodsIssueID " : "") + (!isGoodsIssueID && isCustomerID ? " GoodsIssueDetails.CustomerID = @CustomerID " : "") + (!isGoodsIssueID && !isCustomerID ? " GoodsIssueDetails.GoodsIssueID IN (SELECT GoodsIssueID FROM GoodsIssues WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND LocationID = @LocationID AND OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2)) " : "") + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "") + " INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 " + (isCommodityTypeID ? " AND Commodities.CommodityTypeID = @CommodityTypeID" : "") + " INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";

            return queryString;
        }

        private void AccountInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE          GoodsIssueDetails " + "\r\n";
            queryString = queryString + "       SET             GoodsIssueDetails.QuantityInvoice = ROUND(GoodsIssueDetails.QuantityInvoice + AccountInvoiceDetails.Quantity * @SaveRelativeOption, 0), GoodsIssueDetails.FreeQuantityInvoice = ROUND(GoodsIssueDetails.FreeQuantityInvoice + AccountInvoiceDetails.FreeQuantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            AccountInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID " + "\r\n";
            
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AccountInvoiceSaveRelative", queryString);
        }

        private void AccountInvoicePostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(GoodsIssueDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN GoodsIssueDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND (AccountInvoiceDetails.EntryDate < GoodsIssueDetails.EntryDate OR CAST(AccountInvoiceDetails.EntryDate AS DATE) <> CAST(GoodsIssueDetails.EntryDate AS DATE)) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }



        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalSalesPortalEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }
    }
}
