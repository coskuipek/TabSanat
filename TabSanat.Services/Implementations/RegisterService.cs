using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class RegisterService:IRegisterService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegisterService(IRegistrationRepository RegistrationRepository)
        {
            _registrationRepository = RegistrationRepository;
        }

        public void AddRegistration(Registration registration)
        {
            _registrationRepository.Add(registration);
        }
        public void DeleteRegistration(Registration registration)
        {
            _registrationRepository.RemoveEntity(registration);
        }


        public decimal CalculatePrice(Course course, int NrOfLessonsStudentWillJoin)
        {
            var courseTotalPrice = course.OneLessonPrice * NrOfLessonsStudentWillJoin;
            return courseTotalPrice;
        }


       

        public async Task<IQueryable<Registration>> GetAllAsync(Expression<Func<Registration, bool>> filter = null, Func<IQueryable<Registration>, IOrderedQueryable<Registration>> orderBy = null, params Expression<Func<Registration, object>>[] includes)
        {
            return await _registrationRepository.GetAllAsync(filter, orderBy, includes);
        }


        public async Task<Registration> GetRegistrationAsync(Expression<Func<Registration, bool>> predicate, params Expression<Func<Registration, object>>[] includes)
        {
            return await _registrationRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

    }
}
