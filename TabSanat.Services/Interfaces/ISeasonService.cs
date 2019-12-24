using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface ISeasonService
    {
        void AddSeason(Season season);
        Task<Season> GetSeasonAsync(Expression<Func<Season, bool>> predicate, params Expression<Func<Season, object>>[] includes);
        void DeleteSeason(Guid seasonId);
        void FakeDeleteSeason(Season season);
        Task<IQueryable<Season>> GetAllAsync(Expression<Func<Season, bool>> filter = null, Func<IQueryable<Season>, IOrderedQueryable<Season>> orderBy = null, params Expression<Func<Season, object>>[] includes);

    }
}
