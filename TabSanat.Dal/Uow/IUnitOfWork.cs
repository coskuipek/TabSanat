using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TabSanat.Dal.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> Completeasync();
    }
}
