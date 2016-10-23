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

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.Amount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, AccountInvoiceDetails.Remarks" + "\r\n";
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

            queryString = " @GoodsIssueID Int, @AspUserID nvarchar(128), @LocationID Int, @FromDate DateTime, @ToDate DateTime, @AccountInvoiceID Int, @GoodsIssueDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@GoodsIssueID <> 0) " + "\r\n"; //IF @GoodsIssueID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsIssuesBuildSQL(false ) + "\r\n";

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

            queryString = queryString + "       SELECT      GoodsIssueDetails.GoodsIssueDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Name AS CustomerName, Customers.AddressNo, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.VATPercent, GoodsIssueDetails.GrossPrice, GoodsIssueDetails.Amount, GoodsIssueDetails.VATAmount, GoodsIssueDetails.GrossAmount, GoodsIssueDetails.IsBonus, CAST(1 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        GoodsIssueDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON GoodsIssueDetails.GoodsIssueID " + (isGoodsIssueID ? " = @GoodsIssueID " : " IN (SELECT GoodsIssues.GoodsIssueID FROM                  GoodsIssues              WHERE GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate AND GoodsIssues.LocationID = @LocationID AND GoodsIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel = 2))      AND (GoodsIssueDetails.AccountInvoiceID IS NULL " + (isAccountInvoiceID ? " OR GoodsIssueDetails.AccountInvoiceID = @AccountInvoiceID" : "") + ")" + (isGoodsIssueDetailIDs ? " AND GoodsIssueDetails.GoodsIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsIssueDetailIDs))" : "")) + " AND GoodsIssueDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON GoodsIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }


        private void AccountInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           UPDATE      GoodsIssueDetails" + "\r\n";
            queryString = queryString + "           SET         GoodsIssueDetails.AccountInvoiceID = AccountInvoiceDetails.AccountInvoiceID " + "\r\n";
            queryString = queryString + "           FROM        GoodsIssueDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                       AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND GoodsIssueDetails.GoodsIssueDetailID = AccountInvoiceDetails.GoodsIssueDetailID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           UPDATE      GoodsIssueDetails" + "\r\n";
            queryString = queryString + "           SET         AccountInvoiceID = NULL " + "\r\n";
            queryString = queryString + "           WHERE       AccountInvoiceID = @EntityID " + "\r\n";

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
