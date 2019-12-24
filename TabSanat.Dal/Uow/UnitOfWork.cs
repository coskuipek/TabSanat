using System.Threading.Tasks;
using TabSanat.Dal.Data;

namespace TabSanat.Dal.Uow
{
    public class UnitOfWork:IUnitOfWork
    {
        private TabDbContext _tabDbContext;

        public UnitOfWork(TabDbContext tabDbContext)
        {
            _tabDbContext = tabDbContext;
        }
        public int Complete()
        {
            return _tabDbContext.SaveChanges();
        }
        public async Task<int> Completeasync()
        {
            return await _tabDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _tabDbContext.Dispose();
        }
    }
}
