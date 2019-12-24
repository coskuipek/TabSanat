using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation.Base;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;

namespace TabSanat.Dal.Repositories.Implementation
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(TabDbContext context) : base(context)
        {

        }
    }
}
