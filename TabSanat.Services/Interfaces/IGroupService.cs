using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IQueryable<Group>> GetAllAsync(Expression<Func<Group, bool>> filter = null, Func<IQueryable<Group>, IOrderedQueryable<Group>> orderBy = null, params Expression<Func<Group, object>>[] includes);
        IQueryable<Group> GetAll();
        Task<Group> GetGroupAsync(Expression<Func<Group, bool>> predicate, params Expression<Func<Group, object>>[] includes);
        void AddGroup(Group group);
        void DeleteGroup(Guid GroupId);
    }
}
