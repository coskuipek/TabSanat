using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IQueryable<Expense>> GetAllAsync(Expression<Func<Expense, bool>> filter = null, Func<IQueryable<Expense>, IOrderedQueryable<Expense>> orderBy = null, params Expression<Func<Expense, object>>[] includes);
        Task<Expense> GetExpenseAsync(Expression<Func<Expense, bool>> predicate, params Expression<Func<Expense, object>>[] includes);

        void AddExpenseToDatabase(Expense expense);
        void DeleteExpenseFromDatabase(Guid ExpenseId);
    }
}
