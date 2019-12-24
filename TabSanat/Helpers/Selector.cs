using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TabSanat.Model;

namespace TabSanat.Helpers
{
    public class Selector
    {
        public List<SelectListItem> RegisterSelect(IQueryable<Registration> registrations)
        {
            List<SelectListItem> selects = new List<SelectListItem>(
                registrations.Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = $"{x.Course.Name} {Translator.DayName(x.Course.DayOfWeek)} {x.Group.Name} ({x.PaymentLeft} TL)",
                                        Value = x.Id.ToString()
                                    }).ToList());

            return selects;
        }

        public List<SelectListItem> ExtraSelect(IQueryable<Extra> extras)
        {
            List<SelectListItem> selects = new List<SelectListItem>(
                extras.Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = $"{x.Name} ({x.PriceToSell} TL)",
                                        Value = x.Id.ToString()
                                    }));
            return selects;
        }
    }
}
