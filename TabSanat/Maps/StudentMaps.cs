using System.Collections.Generic;
using TabSanat.Model;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Maps
{
    public class StudentMaps
    {
        public static List<StudentViewModel> StudentIndexMap(IEnumerable<Student> students)
        {
            List<StudentViewModel> listModel = new List<StudentViewModel>();
            foreach (var student in students)
            {
                StudentViewModel model = new StudentViewModel()
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    BirthDate = student.BirthDate,
                    RegisterDate = student.RegisterDate,
                    Email = student.Email,
                    PhoneNo = student.PhoneNo,
                    Balance = student.Balance
                };
                listModel.Add(model);
            }
            return listModel;
        }

        public static StudentViewModel StudentDetailsMap(Student student)
        {
            var model = new StudentViewModel()
            {
                Id = student.Id,
                FullName = student.FullName,
                BirthDate = student.BirthDate,
                Email = student.Email,
                RegisterDate = student.RegisterDate,
                PhotoPath = student.PhotoPath,
                PhoneNo = student.PhoneNo,
                Address = student.Address,
                TcKimlikNo = student.TcKimlikNo,
                FatherFullName = student.FatherFullName,
                FatherPhoneNo = student.FatherPhoneNo,
                FatherJob = student.FatherJob,
                MotherFullName = student.MotherFullName,
                MotherPhoneNo = student.MotherPhoneNo,
                MotherJob = student.MotherJob,
                Institution = student.Institution,
                Balance = student.Balance,
                DiscountName = student.Discount == null ? "Yok" : student.Discount.Name,
                DiscountAmount = student.Discount == null ? "-" : student.Discount.AmountOfDiscount.ToString()
            };


            return model;
        }

        public static Student StudentCreateMap(StudentFormModel model, AppUser user, Discount discount, string uniqueFileName)
        {
            var student = new Student()
            {
                FirstName = FixName(model.FirstName),
                LastName = FixName(model.LastName),
                BirthDate = model.BirthDate,
                RegisterDate = model.RegisterDate,
                PhotoPath = uniqueFileName,
                Discount = discount,
                PhoneNo = model.PhoneNo,
                Email = model.Email,
                Address = model.Address,
                TcKimlikNo = model.TcKimlikNo,
                FatherFullName = FixName(model.FatherFullName),
                FatherPhoneNo = model.FatherPhoneNo,
                FatherJob = model.FatherJob,
                MotherFullName = FixName(model.MotherFullName),
                MotherPhoneNo = model.MotherPhoneNo,
                MotherJob = model.MotherJob,
                Institution = FixName(model.Institution),
                AppUser = user
            };
            return student;
        }
        public static string FixName(string stringToFix)
        {
            if (stringToFix == null)
                return null;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringToFix.ToLower());
        }
    }
}
