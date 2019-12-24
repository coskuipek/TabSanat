using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IAppSettingsService
    {
        Task<AppSettings> GetSettingAsync(Expression<Func<AppSettings, bool>> predicate, params Expression<Func<AppSettings, object>>[] includes);
        Task<IQueryable<AppSettings>> GetAllAsync(Expression<Func<AppSettings, bool>> filter = null, Func<IQueryable<AppSettings>, IOrderedQueryable<AppSettings>> orderBy = null, params Expression<Func<AppSettings, object>>[] includes);
    }
}
