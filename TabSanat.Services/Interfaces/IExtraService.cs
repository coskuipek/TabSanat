using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IExtraService
    {
        Task<IQueryable<Extra>> GetAllAsync(Expression<Func<Extra, bool>> filter = null, Func<IQueryable<Extra>, IOrderedQueryable<Extra>> orderBy = null, params Expression<Func<Extra, object>>[] includes);
        IQueryable<Extra> GetAll();
        Task<Extra> GetExtraAsync(Expression<Func<Extra, bool>> predicate, params Expression<Func<Extra, object>>[] includes);
        Task<IQueryable<Sale>> GetSalesOfStudent(Student student);
        Task<IQueryable<Sale>> GetSalesOfExtra(Extra extra);
        void AddExtra(Extra extra);
        void DeleteExtra(Guid extraId);
    }
}
