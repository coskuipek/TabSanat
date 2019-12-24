using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IPaymentService
    {
        void SavePaymentToDatabase(Payment payment, Registration registration);
        void DeletePaymentFromDatabase(Payment payment, Registration registration);
        Task<IQueryable<Payment>> GetAllAsync(Expression<Func<Payment, bool>> filter = null, Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null, params Expression<Func<Payment, object>>[] includes);
        Task<Payment> GetPaymentAsync(Expression<Func<Payment, bool>> predicate, params Expression<Func<Payment, object>>[] includes);
        Task<List<Payment>> FilterPayments(IQueryable<Payment> payments, DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser, bool showfuture);
    }
}
