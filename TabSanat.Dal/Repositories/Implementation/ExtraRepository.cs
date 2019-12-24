using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation.Base;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;

namespace TabSanat.Dal.Repositories.Implementation
{
    public class ExtraRepository : Repository<Extra>, IExtraRepository
    {
        public ExtraRepository(TabDbContext context) : base(context)
        {

        }
    }
}
