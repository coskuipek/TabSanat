using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class ExpenseService: IExpenseService
    {

        private readonly IExpenseRepository _expenseRepository;


        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public IQueryable<Expense> GetAll()
        {
            return _expenseRepository.GetAll();
        }

        public async Task<IQueryable<Expense>> GetAllAsync(Expression<Func<Expense, bool>> filter = null, Func<IQueryable<Expense>, IOrderedQueryable<Expense>> orderBy = null, params Expression<Func<Expense, object>>[] includes)
        {
            return await _expenseRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Expense> GetExpenseAsync(Expression<Func<Expense, bool>> predicate, params Expression<Func<Expense, object>>[] includes)
        {
            return await _expenseRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public void AddExpenseToDatabase(Expense expense)
        {
            _expenseRepository.Add(expense);
        }

        public void DeleteExpenseFromDatabase(Guid expenseId)
        {
            _expenseRepository.Remove(expenseId);
        }
    }
}
