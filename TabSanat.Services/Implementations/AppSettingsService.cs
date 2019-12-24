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
    public class AppSettingsService:IAppSettingsService
    {
        private readonly IAppSettingsRepository _appSettings;

        public AppSettingsService(IAppSettingsRepository appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<IQueryable<AppSettings>> GetAllAsync(Expression<Func<AppSettings, bool>> filter = null, Func<IQueryable<AppSettings>, IOrderedQueryable<AppSettings>> orderBy = null, params Expression<Func<AppSettings, object>>[] includes)
        {
            return await _appSettings.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<AppSettings> GetSettingAsync(Expression<Func<AppSettings, bool>> predicate, params Expression<Func<AppSettings, object>>[] includes)
        {
            return await _appSettings.GetFirstOrDefaultAsync(predicate, includes);
        }
    }
}
