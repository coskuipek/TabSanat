using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface ISaleService
    {
        void SaveSaleToDatabase(Sale sale);
        void DeleteSaleFromDatabase(Sale sale);
        Task<Sale> GetSaleAsync(Expression<Func<Sale, bool>> predicate, params Expression<Func<Sale, object>>[] includes);
        Task<IQueryable<Sale>> GetAllAsync(Expression<Func<Sale, bool>> filter = null, Func<IQueryable<Sale>, IOrderedQueryable<Sale>> orderBy = null, params Expression<Func<Sale, object>>[] includes);
        Task<List<Sale>> FilterSales(IQueryable<Sale> sales, DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser);
    }
}
