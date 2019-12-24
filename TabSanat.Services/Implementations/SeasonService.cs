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
    public class SeasonService:ISeasonService
    {
        private readonly ISeasonRepository _seasonRepository;

        public SeasonService(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public void AddSeason(Season season)
        {
            _seasonRepository.Add(season);
        }
        public void FakeDeleteSeason(Season season)
        {
            season.IsDeleted = true;
        }

        public void DeleteSeason(Guid seasonId)
        {
            _seasonRepository.Remove(seasonId);
        }


        public async Task<IQueryable<Season>> GetAllAsync(Expression<Func<Season, bool>> filter = null, Func<IQueryable<Season>, IOrderedQueryable<Season>> orderBy = null, params Expression<Func<Season, object>>[] includes)
        {
            return await _seasonRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Season> GetSeasonAsync(Expression<Func<Season, bool>> predicate, params Expression<Func<Season, object>>[] includes)
        {
            return await _seasonRepository.GetFirstOrDefaultAsync(predicate, includes);
        }
    }
}
