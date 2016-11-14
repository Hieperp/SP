using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class GoodsIssueTypeRepository : GenericRepository<GoodsIssueType>, IGoodsIssueTypeRepository
    {
        public GoodsIssueTypeRepository(TotalSalesPortalEntities totalSalesPortalEntities)
            : base(totalSalesPortalEntities)
        {
        }

        public IList<GoodsIssueType> SearchGoodsIssueTypes(string searchText)
        {
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<GoodsIssueType> goodsIssueTypes = this.TotalSalesPortalEntities.GoodsIssueTypes.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).OrderByDescending(or => or.Name).Take(20).ToList();
            this.TotalSalesPortalEntities.Configuration.ProxyCreationEnabled = true;

            return goodsIssueTypes;
        }
    }
}