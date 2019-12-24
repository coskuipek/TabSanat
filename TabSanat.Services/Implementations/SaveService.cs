using System;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Dal.Uow;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class SaveService:ISaveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHistoryRepository _historyRepository;

        public SaveService(IUnitOfWork unitOfWork, IHistoryRepository historyRepository)
        {
            _unitOfWork = unitOfWork;
            _historyRepository = historyRepository;
        }
        public async Task<int> Completeasync()
        {
            return await _unitOfWork.Completeasync();
        }
        public async Task<int> Completeasync(string description, AppUser user)
        {
            _historyRepository.Add(new History() {
                AppUser = user,
                Description = description,
                DateTime = DateTime.Now
            });
            return await _unitOfWork.Completeasync();
        }

        public string FixName(string stringToFix)
        {
            if (stringToFix == null)
                return null;
            
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringToFix.ToLower());
        }
    }
}
