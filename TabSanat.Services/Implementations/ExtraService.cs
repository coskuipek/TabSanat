using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class ExtraService: IExtraService
    {

        private readonly IExtraRepository _extraRepository;
        private readonly ISaleRepository _saleRepository;

        public ExtraService(IExtraRepository extraRepository, ISaleRepository saleRepository)
        {
            _extraRepository = extraRepository;
            _saleRepository = saleRepository;
        }

        public IQueryable<Extra> GetAll()
        {
            return _extraRepository.GetAll();
        }

        public async Task<IQueryable<Extra>> GetAllAsync(Expression<Func<Extra, bool>> filter = null, Func<IQueryable<Extra>, IOrderedQueryable<Extra>> orderBy = null, params Expression<Func<Extra, object>>[] includes)
        {
            return await _extraRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Extra> GetExtraAsync(Expression<Func<Extra, bool>> predicate, params Expression<Func<Extra, object>>[] includes)
        {
            return await _extraRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public async Task<IQueryable<Sale>> GetSalesOfStudent(Student student)
        {
            return await _saleRepository.GetAllAsync(x => x.Student == student, y => y.OrderByDescending(z => z.Date), x=> x.SaleItems);
        }

        public async Task<IQueryable<Sale>> GetSalesOfExtra(Extra extra)
        {
            var sales = await _saleRepository.GetAllAsync(x=> x.SaleItems.Any(y => y.Extra == extra),y=> y.OrderByDescending(z => z.Date),x=> x.SaleItems, x=> x.Student);
            
            return sales;
        }

        public void AddExtra(Extra extra)
        {
            _extraRepository.Add(extra);
        }

        public void DeleteExtra(Guid extraId)
        {
            _extraRepository.Remove(extraId);
        }
    }
}
