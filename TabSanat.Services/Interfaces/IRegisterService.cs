using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<IQueryable<Registration>> GetAllAsync(Expression<Func<Registration, bool>> filter = null, Func<IQueryable<Registration>, IOrderedQueryable<Registration>> orderBy = null, params Expression<Func<Registration, object>>[] includes);

        Task<Registration> GetRegistrationAsync(Expression<Func<Registration, bool>> predicate, params Expression<Func<Registration, object>>[] includes);

        void AddRegistration(Registration Registration);
        void DeleteRegistration(Registration registration);
        decimal CalculatePrice(Course course, int NrOfLessonsStudentWillJoin);


    }
}
