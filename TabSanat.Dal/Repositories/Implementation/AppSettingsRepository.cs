using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation.Base;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;

namespace TabSanat.Dal.Repositories.Implementation
{
    public class AppSettingsRepository : Repository<AppSettings>, IAppSettingsRepository
    {
        public AppSettingsRepository(TabDbContext context) : base(context)
        {

        }
    }
}
