using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Reports
{
    public class SaleReports
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public SaleReports(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.DeliveryAdviceJournal();
        }

        private void DeliveryAdviceJournal()
        {
            string queryString;

            queryString = " @DeliveryAdviceID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalDeliveryAdviceID int      SET @LocalDeliveryAdviceID = @DeliveryAdviceID" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      DeliveryAdvices.DeliveryAdviceID, DeliveryAdvices.EntryDate, DeliveryAdvices.Reference, DeliveryAdvices.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, DeliveryAdvices.ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.Quantity, DeliveryAdviceDetails.QuantityIssue, DeliveryAdviceDetails.Quantity - DeliveryAdviceDetails.QuantityIssue AS QuantityRemains, DeliveryAdviceDetails.FreeQuantity, DeliveryAdviceDetails.FreeQuantityIssue, DeliveryAdviceDetails.FreeQuantity - DeliveryAdviceDetails.FreeQuantityIssue AS FreeQuantityRemains, DeliveryAdviceDetails.GrossAmount, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueID, GoodsIssueDetails.EntryDate AS GoodsIssueEntryDate, GoodsIssueDetails.Quantity AS GoodsIssueQuantity, GoodsIssueDetails.FreeQuantity AS GoodsIssueFreeQuantity, " + "\r\n";
            queryString = queryString + "                   DeliveryAdviceDetails.InActive, DeliveryAdviceDetails.InActivePartial, DeliveryAdviceDetails.InActiveIssue, ISNULL(VoidDeliveryAdvices.Name, '') + IIF(DeliveryAdvices.VoidTypeID <> 0 AND DeliveryAdviceDetails.VoidTypeID <> 0, ', ', '') + ISNULL(VoidDeliveryAdviceDetails.Name, '') AS VoidDeliveryAdviceName, VoidGoodsIssues.Name AS VoidGoodsIssueName " + "\r\n";
            
            queryString = queryString + "       FROM        DeliveryAdvices " + "\r\n";
            queryString = queryString + "                   INNER JOIN DeliveryAdviceDetails ON (DeliveryAdvices.DeliveryAdviceID = @LocalDeliveryAdviceID OR (@LocalDeliveryAdviceID = 0 AND DeliveryAdvices.EntryDate >= @LocalFromDate AND DeliveryAdvices.EntryDate <= @LocalToDate)) AND DeliveryAdvices.DeliveryAdviceID = DeliveryAdviceDetails.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON DeliveryAdvices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers AS Receivers ON DeliveryAdvices.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON DeliveryAdviceDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsIssueDetails ON DeliveryAdviceDetails.DeliveryAdviceDetailID = GoodsIssueDetails.DeliveryAdviceDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidDeliveryAdvices ON DeliveryAdvices.VoidTypeID = VoidDeliveryAdvices.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidDeliveryAdviceDetails ON DeliveryAdviceDetails.VoidTypeID = VoidDeliveryAdviceDetails.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes AS VoidGoodsIssues ON GoodsIssueDetails.VoidTypeID = VoidGoodsIssues.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("DeliveryAdviceJournal", queryString);
        }


    }
}
