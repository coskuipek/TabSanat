using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class GroupService: IGroupService
    {

        private readonly IGroupRepository _groupRepository;


        public GroupService(IGroupRepository GroupRepository)
        {
            _groupRepository = GroupRepository;
        }

        public IQueryable<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }

        public async Task<IQueryable<Group>> GetAllAsync(Expression<Func<Group, bool>> filter = null, Func<IQueryable<Group>, IOrderedQueryable<Group>> orderBy = null, params Expression<Func<Group, object>>[] includes)
        {
            return await _groupRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Group> GetGroupAsync(Expression<Func<Group, bool>> predicate, params Expression<Func<Group, object>>[] includes)
        {
            return await _groupRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public void AddGroup(Group Group)
        {
            _groupRepository.Add(Group);
        }

        public void DeleteGroup(Guid GroupId)
        {
            _groupRepository.Remove(GroupId);
        }
    }
}
