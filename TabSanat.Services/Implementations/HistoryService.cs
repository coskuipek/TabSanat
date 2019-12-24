using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class HistoryService: IHistoryService
    {

        private readonly IHistoryRepository _historyRepository;


        public HistoryService(IHistoryRepository historyRepository, ISaleRepository saleRepository)
        {
            _historyRepository = historyRepository;

        }



        public async Task<IQueryable<History>> GetAllAsync(Expression<Func<History, bool>> filter = null, Func<IQueryable<History>, IOrderedQueryable<History>> orderBy = null, params Expression<Func<History, object>>[] includes)
        {
            return await _historyRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<History> GetHistoryAsync(Expression<Func<History, bool>> predicate, params Expression<Func<History, object>>[] includes)
        {
            return await _historyRepository.GetFirstOrDefaultAsync(predicate, includes);
        }


        public void DeleteHistory(Guid historyId)
        {
            _historyRepository.Remove(historyId);
        }
    }
}
