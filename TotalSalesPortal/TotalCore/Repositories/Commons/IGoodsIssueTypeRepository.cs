using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IGoodsIssueTypeRepository : IGenericRepository<GoodsIssueType>
    {
        IList<GoodsIssueType> SearchGoodsIssueTypes(string searchText);
    }
}

