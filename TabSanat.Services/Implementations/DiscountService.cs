using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class DiscountService:IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public void AddDiscount(Discount discount)
        {
            _discountRepository.Add(discount);
        }

        public void DeleteDiscount(Guid discountId)
        {
            _discountRepository.Remove(discountId);
        }
        public decimal ApplyDiscount(Discount discount, decimal coursePrice)
        {
            if (discount!= null)
            {
                if (discount.IsFixedAmount)
                    coursePrice -= discount.AmountOfDiscount;

                else
                    coursePrice -= ((coursePrice / 100) * discount.AmountOfDiscount);
            }
           
            return coursePrice;
        }

        public async Task<IQueryable<Discount>> GetAllAsync(Expression<Func<Discount, bool>> filter = null, Func<IQueryable<Discount>, IOrderedQueryable<Discount>> orderBy = null, params Expression<Func<Discount, object>>[] includes)
        {
            return await _discountRepository.GetAllAsync(filter, orderBy, includes);
        }


        public async Task<Discount> GetDiscountAsync(Expression<Func<Discount, bool>> predicate, params Expression<Func<Discount, object>>[] includes)
        {
            return await _discountRepository.GetFirstOrDefaultAsync(predicate, includes);
        }
    }
}
