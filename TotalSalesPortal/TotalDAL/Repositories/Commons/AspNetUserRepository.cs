
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public AspNetUserRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<AspNetUser> GetAllAspNetUsers()
        {
            return this.totalSalesPortalEntities.AspNetUsers.ToList();
        }
    }
}
