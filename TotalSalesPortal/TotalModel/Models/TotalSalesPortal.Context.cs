﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TotalSalesPortalEntities : DbContext
    {
        public TotalSalesPortalEntities()
            : base("name=TotalSalesPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ModuleDetail> ModuleDetails { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<OrganizationalUnit> OrganizationalUnits { get; set; }
        public virtual DbSet<OrganizationalUnitUser> OrganizationalUnitUsers { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<CommodityCategory> CommodityCategories { get; set; }
        public virtual DbSet<CommodityType> CommodityTypes { get; set; }
        public virtual DbSet<CustomerCategory> CustomerCategories { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EntireTerritory> EntireTerritories { get; set; }
        public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }
        public virtual DbSet<DeliveryAdviceDetail> DeliveryAdviceDetails { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<PriceCategory> PriceCategories { get; set; }
        public virtual DbSet<GoodsIssueDetail> GoodsIssueDetails { get; set; }
        public virtual DbSet<AccountInvoiceDetail> AccountInvoiceDetails { get; set; }
        public virtual DbSet<AccountInvoice> AccountInvoices { get; set; }
        public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<DeliveryAdvice> DeliveryAdvices { get; set; }
        public virtual DbSet<GoodsIssue> GoodsIssues { get; set; }
        public virtual DbSet<GoodsIssueType> GoodsIssueTypes { get; set; }
        public virtual DbSet<AccessControl> AccessControls { get; set; }
    
        public virtual ObjectResult<Nullable<int>> GetAccessLevel(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("GetAccessLevel", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetApprovalPermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetApprovalPermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetUnApprovalPermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetUnApprovalPermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<string> DeliveryAdviceEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeliveryAdviceEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<string> DeliveryAdvicePostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeliveryAdvicePostSaveValidate", entityIDParameter);
        }
    
        public virtual int DeliveryAdviceSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeliveryAdviceSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual ObjectResult<DeliveryAdviceIndex> GetDeliveryAdviceIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DeliveryAdviceIndex>("GetDeliveryAdviceIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<DeliveryAdviceViewDetail> GetDeliveryAdviceViewDetails(Nullable<int> deliveryAdviceID)
        {
            var deliveryAdviceIDParameter = deliveryAdviceID.HasValue ?
                new ObjectParameter("DeliveryAdviceID", deliveryAdviceID) :
                new ObjectParameter("DeliveryAdviceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DeliveryAdviceViewDetail>("GetDeliveryAdviceViewDetails", deliveryAdviceIDParameter);
        }
    
        public virtual ObjectResult<CommodityAvailable> GetCommodityAvailables(Nullable<int> locationID, Nullable<int> customerID, Nullable<int> priceCategoryID, Nullable<int> promotionID, Nullable<System.DateTime> entryDate, string searchText)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var priceCategoryIDParameter = priceCategoryID.HasValue ?
                new ObjectParameter("PriceCategoryID", priceCategoryID) :
                new ObjectParameter("PriceCategoryID", typeof(int));
    
            var promotionIDParameter = promotionID.HasValue ?
                new ObjectParameter("PromotionID", promotionID) :
                new ObjectParameter("PromotionID", typeof(int));
    
            var entryDateParameter = entryDate.HasValue ?
                new ObjectParameter("EntryDate", entryDate) :
                new ObjectParameter("EntryDate", typeof(System.DateTime));
    
            var searchTextParameter = searchText != null ?
                new ObjectParameter("SearchText", searchText) :
                new ObjectParameter("SearchText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CommodityAvailable>("GetCommodityAvailables", locationIDParameter, customerIDParameter, priceCategoryIDParameter, promotionIDParameter, entryDateParameter, searchTextParameter);
        }
    
        public virtual ObjectResult<Promotion> GetPromotionByCustomers(Nullable<int> customerID)
        {
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Promotion>("GetPromotionByCustomers", customerIDParameter);
        }
    
        public virtual ObjectResult<Promotion> GetPromotionByCustomers(Nullable<int> customerID, MergeOption mergeOption)
        {
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Promotion>("GetPromotionByCustomers", mergeOption, customerIDParameter);
        }
    
        public virtual ObjectResult<GoodsIssueIndex> GetGoodsIssueIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GoodsIssueIndex>("GetGoodsIssueIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<GoodsIssueViewDetail> GetGoodsIssueViewDetails(Nullable<int> goodsIssueID, Nullable<int> deliveryAdviceID, Nullable<int> customerID, Nullable<bool> isReadonly)
        {
            var goodsIssueIDParameter = goodsIssueID.HasValue ?
                new ObjectParameter("GoodsIssueID", goodsIssueID) :
                new ObjectParameter("GoodsIssueID", typeof(int));
    
            var deliveryAdviceIDParameter = deliveryAdviceID.HasValue ?
                new ObjectParameter("DeliveryAdviceID", deliveryAdviceID) :
                new ObjectParameter("DeliveryAdviceID", typeof(int));
    
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var isReadonlyParameter = isReadonly.HasValue ?
                new ObjectParameter("IsReadonly", isReadonly) :
                new ObjectParameter("IsReadonly", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GoodsIssueViewDetail>("GetGoodsIssueViewDetails", goodsIssueIDParameter, deliveryAdviceIDParameter, customerIDParameter, isReadonlyParameter);
        }
    
        public virtual ObjectResult<PendingDeliveryAdviceCustomer> GetPendingDeliveryAdviceCustomers(Nullable<int> locationID, Nullable<int> goodsIssueID, string customerName)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var goodsIssueIDParameter = goodsIssueID.HasValue ?
                new ObjectParameter("GoodsIssueID", goodsIssueID) :
                new ObjectParameter("GoodsIssueID", typeof(int));
    
            var customerNameParameter = customerName != null ?
                new ObjectParameter("CustomerName", customerName) :
                new ObjectParameter("CustomerName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingDeliveryAdviceCustomer>("GetPendingDeliveryAdviceCustomers", locationIDParameter, goodsIssueIDParameter, customerNameParameter);
        }
    
        public virtual ObjectResult<PendingDeliveryAdvice> GetPendingDeliveryAdvices(Nullable<int> locationID, Nullable<int> goodsIssueID, string deliveryAdviceReference)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var goodsIssueIDParameter = goodsIssueID.HasValue ?
                new ObjectParameter("GoodsIssueID", goodsIssueID) :
                new ObjectParameter("GoodsIssueID", typeof(int));
    
            var deliveryAdviceReferenceParameter = deliveryAdviceReference != null ?
                new ObjectParameter("DeliveryAdviceReference", deliveryAdviceReference) :
                new ObjectParameter("DeliveryAdviceReference", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingDeliveryAdvice>("GetPendingDeliveryAdvices", locationIDParameter, goodsIssueIDParameter, deliveryAdviceReferenceParameter);
        }
    
        public virtual ObjectResult<string> GoodsIssueEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GoodsIssueEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<string> GoodsIssuePostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GoodsIssuePostSaveValidate", entityIDParameter);
        }
    
        public virtual int GoodsIssueSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GoodsIssueSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual ObjectResult<string> AccountInvoicePostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("AccountInvoicePostSaveValidate", entityIDParameter);
        }
    
        public virtual int AccountInvoiceSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AccountInvoiceSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual ObjectResult<AccountInvoiceIndex> GetAccountInvoiceIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AccountInvoiceIndex>("GetAccountInvoiceIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<AccountInvoiceViewDetail> GetAccountInvoiceViewDetails(Nullable<int> accountInvoiceID)
        {
            var accountInvoiceIDParameter = accountInvoiceID.HasValue ?
                new ObjectParameter("AccountInvoiceID", accountInvoiceID) :
                new ObjectParameter("AccountInvoiceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AccountInvoiceViewDetail>("GetAccountInvoiceViewDetails", accountInvoiceIDParameter);
        }
    
        public virtual ObjectResult<PendingGoodsIssue> GetPendingGoodsIssues(Nullable<int> goodsIssueID, string aspUserID, Nullable<int> locationID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> accountInvoiceID, string goodsIssueDetailIDs)
        {
            var goodsIssueIDParameter = goodsIssueID.HasValue ?
                new ObjectParameter("GoodsIssueID", goodsIssueID) :
                new ObjectParameter("GoodsIssueID", typeof(int));
    
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var accountInvoiceIDParameter = accountInvoiceID.HasValue ?
                new ObjectParameter("AccountInvoiceID", accountInvoiceID) :
                new ObjectParameter("AccountInvoiceID", typeof(int));
    
            var goodsIssueDetailIDsParameter = goodsIssueDetailIDs != null ?
                new ObjectParameter("GoodsIssueDetailIDs", goodsIssueDetailIDs) :
                new ObjectParameter("GoodsIssueDetailIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingGoodsIssue>("GetPendingGoodsIssues", goodsIssueIDParameter, aspUserIDParameter, locationIDParameter, fromDateParameter, toDateParameter, accountInvoiceIDParameter, goodsIssueDetailIDsParameter);
        }
    
        public virtual ObjectResult<CustomerReceivable> GetCustomerReceivables(Nullable<int> locationID, Nullable<int> receiptID, string customerName)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var receiptIDParameter = receiptID.HasValue ?
                new ObjectParameter("ReceiptID", receiptID) :
                new ObjectParameter("ReceiptID", typeof(int));
    
            var customerNameParameter = customerName != null ?
                new ObjectParameter("CustomerName", customerName) :
                new ObjectParameter("CustomerName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CustomerReceivable>("GetCustomerReceivables", locationIDParameter, receiptIDParameter, customerNameParameter);
        }
    
        public virtual ObjectResult<GoodsIssueReceivable> GetGoodsIssueReceivables(Nullable<int> locationID, Nullable<int> receiptID, string goodsIssueReference)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var receiptIDParameter = receiptID.HasValue ?
                new ObjectParameter("ReceiptID", receiptID) :
                new ObjectParameter("ReceiptID", typeof(int));
    
            var goodsIssueReferenceParameter = goodsIssueReference != null ?
                new ObjectParameter("GoodsIssueReference", goodsIssueReference) :
                new ObjectParameter("GoodsIssueReference", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GoodsIssueReceivable>("GetGoodsIssueReceivables", locationIDParameter, receiptIDParameter, goodsIssueReferenceParameter);
        }
    
        public virtual ObjectResult<ReceiptIndex> GetReceiptIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReceiptIndex>("GetReceiptIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<ReceiptViewDetail> GetReceiptViewDetails(Nullable<int> receiptID, Nullable<int> goodsIssueID, Nullable<int> customerID, Nullable<bool> isReadonly)
        {
            var receiptIDParameter = receiptID.HasValue ?
                new ObjectParameter("ReceiptID", receiptID) :
                new ObjectParameter("ReceiptID", typeof(int));
    
            var goodsIssueIDParameter = goodsIssueID.HasValue ?
                new ObjectParameter("GoodsIssueID", goodsIssueID) :
                new ObjectParameter("GoodsIssueID", typeof(int));
    
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            var isReadonlyParameter = isReadonly.HasValue ?
                new ObjectParameter("IsReadonly", isReadonly) :
                new ObjectParameter("IsReadonly", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReceiptViewDetail>("GetReceiptViewDetails", receiptIDParameter, goodsIssueIDParameter, customerIDParameter, isReadonlyParameter);
        }
    
        public virtual ObjectResult<string> ReceiptEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ReceiptEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<string> ReceiptPostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ReceiptPostSaveValidate", entityIDParameter);
        }
    
        public virtual int ReceiptSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReceiptSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetUnVoidablePermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetUnVoidablePermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetVoidablePermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetVoidablePermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    }
}
