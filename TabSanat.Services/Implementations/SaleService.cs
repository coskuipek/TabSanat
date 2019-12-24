using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        public void SaveSaleToDatabase(Sale sale)
        {
            _saleRepository.Add(sale);
        }
        public void DeleteSaleFromDatabase(Sale sale)
        {
            _saleRepository.RemoveEntity(sale);
        }

        public async Task<IQueryable<Sale>> GetAllAsync(Expression<Func<Sale, bool>> filter = null, Func<IQueryable<Sale>, IOrderedQueryable<Sale>> orderBy = null, params Expression<Func<Sale, object>>[] includes)
        {
            return await _saleRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Sale> GetSaleAsync(Expression<Func<Sale, bool>> predicate, params Expression<Func<Sale, object>>[] includes)
        {
            return await _saleRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public async Task<List<Sale>> FilterSales(IQueryable<Sale> sales, DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser)
        {
            if (startdate != DateTime.MinValue)
                sales = sales.Where(x => x.Date >= startdate);
            if (enddate != DateTime.MinValue)
                sales = sales.Where(x => x.Date <= enddate);
            if (paymenttype != null && paymenttype != Guid.Empty)
                sales = sales.Where(x => x.PaymentTypeId == paymenttype);
            if (appuser != null)
                sales = sales.Where(x => x.AppUserId == appuser);

            return await sales.ToListAsync();
        }
    }
}
