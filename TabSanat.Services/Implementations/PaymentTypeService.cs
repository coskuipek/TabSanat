using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class PaymentTypeService: IPaymentTypeService
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }

        public void AddPaymentType(PaymentType paymentType)
        {
            _paymentTypeRepository.Add(paymentType);
        }

        public void DeletePaymentType(Guid paymentTypeId)
        {
            _paymentTypeRepository.Remove(paymentTypeId);
        }


        public async Task<IQueryable<PaymentType>> GetAllAsync(Expression<Func<PaymentType, bool>> filter = null, Func<IQueryable<PaymentType>, IOrderedQueryable<PaymentType>> orderBy = null, params Expression<Func<PaymentType, object>>[] includes)
        {
            return await _paymentTypeRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<PaymentType> GetTypeAsync(Expression<Func<PaymentType, bool>> predicate, params Expression<Func<PaymentType, object>>[] includes)
        {
            return await _paymentTypeRepository.GetFirstOrDefaultAsync(predicate, includes);
        }
    }
}
