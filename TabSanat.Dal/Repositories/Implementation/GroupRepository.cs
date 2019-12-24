using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation.Base;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;

namespace TabSanat.Dal.Repositories.Implementation
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(TabDbContext context) : base(context)
        {

        }
    }
}
