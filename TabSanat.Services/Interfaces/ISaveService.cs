using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface ISaveService
    {
        string FixName(string stringToFix);
        Task<int> Completeasync(string description, AppUser user);
        Task<int> Completeasync();
    }
}
