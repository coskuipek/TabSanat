using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IPaymentTypeService
    {
        Task<IQueryable<PaymentType>> GetAllAsync(Expression<Func<PaymentType, bool>> filter = null, Func<IQueryable<PaymentType>, IOrderedQueryable<PaymentType>> orderBy = null, params Expression<Func<PaymentType, object>>[] includes);

        Task<PaymentType> GetTypeAsync(Expression<Func<PaymentType, bool>> predicate, params Expression<Func<PaymentType, object>>[] includes);

        void AddPaymentType(PaymentType paymentType);
        void DeletePaymentType(Guid paymentTypeId);
    }
}
