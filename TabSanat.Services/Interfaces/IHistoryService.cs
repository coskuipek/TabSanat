using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<IQueryable<History>> GetAllAsync(Expression<Func<History, bool>> filter = null, Func<IQueryable<History>, IOrderedQueryable<History>> orderBy = null, params Expression<Func<History, object>>[] includes);
        Task<History> GetHistoryAsync(Expression<Func<History, bool>> predicate, params Expression<Func<History, object>>[] includes);
        void DeleteHistory(Guid historyId);
    }
}
