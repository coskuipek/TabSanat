using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TabSanat.ViewModels.Form
{
    public class SeasonFormModel : IValidatableObject

    {
        public Guid Id { get; set; }
        //
        [Display(Name = "Sezon Adı")]
        [Required]
        public string Name { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç")]
        public DateTime StartDate { get; set; }
        //
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş")]
        public DateTime EndDate { get; set; }
        //
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (EndDate <= StartDate)
            {
                results.Add(new ValidationResult("Bitiş tarihi, başlangıç tarihinden önce olamaz.", new[] { "EndDate", "StartDate" }));
            }

            return results;
        }

    }

}
