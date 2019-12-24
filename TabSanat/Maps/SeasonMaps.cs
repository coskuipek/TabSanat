using System.Collections.Generic;
using TabSanat.Model;
using TabSanat.ViewModels.Display;

namespace TabSanat.Maps
{
    public class SeasonMaps
    {
        public List<SeasonViewModel> SeasonIndexMap(IEnumerable<Season> seasons)
        {
            List<SeasonViewModel> listModel = new List<SeasonViewModel>();
            foreach (var season in seasons)
            {
                SeasonViewModel model = new SeasonViewModel()
                {
                    Id = season.Id,
                    Name = season.Name,
                    StartDate = season.StartDate,
                    EndDate = season.EndDate,
                    SeasonEnded = season.SeasonEnded
                };
                listModel.Add(model);
            }
            return listModel;
        }
    }
}
