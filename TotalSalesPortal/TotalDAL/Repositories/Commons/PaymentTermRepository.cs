
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class PaymentTermRepository : IPaymentTermRepository
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public PaymentTermRepository(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public IList<PaymentTerm> GetAllPaymentTerms()
        {
            return this.totalSalesPortalEntities.PaymentTerms.ToList();
        }
    }
}

