using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStudentRepository _studentRepository;

        public PaymentService(IPaymentRepository paymentRepository, IStudentRepository studentRepository)
        {
            _paymentRepository = paymentRepository;
            _studentRepository = studentRepository;
        }

        public void SavePaymentToDatabase(Payment payment,Registration registration)
        {
            payment.Registration = registration;
            _paymentRepository.Add(payment);
            if (payment.StudentId != null && payment.StudentId != Guid.Empty)
            {
                if (!payment.IsGiveBack)
                {
                    registration.Student.Balance -= payment.Price;
                    registration.PaymentLeft -= payment.Price;
                }
                else
                {
                    registration.Student.Balance += payment.Price;
                    registration.PaymentLeft += payment.Price;
                }
            }
        }

        public void DeletePaymentFromDatabase(Payment payment ,Registration registration)
        {
            _paymentRepository.Remove(payment.Id);
            if (payment.StudentId != null && payment.StudentId != Guid.Empty)
            {
                if (!payment.IsGiveBack)
                {
                    registration.Student.Balance += payment.Price;
                    registration.PaymentLeft += payment.Price;
                }
                else
                {
                    registration.Student.Balance -= payment.Price;
                    registration.PaymentLeft -= payment.Price;
                }
            }
                
           
        }

        public async Task<IQueryable<Payment>> GetAllAsync(Expression<Func<Payment, bool>> filter = null, Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null, params Expression<Func<Payment, object>>[] includes)
        {
            return await _paymentRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Payment> GetPaymentAsync(Expression<Func<Payment, bool>> predicate, params Expression<Func<Payment, object>>[] includes)
        {
            return await _paymentRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public async Task<List<Payment>> FilterPayments(IQueryable<Payment> payments, DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser, bool showfuture)
        {
            if (startdate != DateTime.MinValue)
                payments = payments.Where(x => x.TimeToShow >= startdate);
            if (enddate != DateTime.MinValue)
                payments = payments.Where(x => x.TimeToShow <= enddate);
            if (paymenttype != null && paymenttype != Guid.Empty)
                payments = payments.Where(x => x.PaymentTypeId == paymenttype);
            if (appuser != null)
                payments = payments.Where(x => x.AppUserId == appuser);
            if (!showfuture)
                payments = payments.Where(x => x.TimeToShow < DateTime.Now.AddDays(1));
            

            return await payments.ToListAsync();
        }
    }
}
