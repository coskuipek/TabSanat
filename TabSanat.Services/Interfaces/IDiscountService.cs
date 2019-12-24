using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<IQueryable<Discount>> GetAllAsync(Expression<Func<Discount, bool>> filter = null, Func<IQueryable<Discount>, IOrderedQueryable<Discount>> orderBy = null, params Expression<Func<Discount, object>>[] includes);

        Task<Discount> GetDiscountAsync(Expression<Func<Discount, bool>> predicate, params Expression<Func<Discount, object>>[] includes);

        void AddDiscount(Discount discount);
        void DeleteDiscount(Guid discountId);
        decimal ApplyDiscount(Discount discount, decimal coursePrice);
    }
}
