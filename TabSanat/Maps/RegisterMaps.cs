using System.Collections.Generic;
using TabSanat.Model;
using TabSanat.ViewModels.Display;

namespace TabSanat.Maps
{
    public class RegisterMaps
    {
        public List<RegisterViewModel> RegisterIndexMap(IEnumerable<Registration> registers)
        {
            List<RegisterViewModel> listModel = new List<RegisterViewModel>();
            foreach (var register in registers)
            {
                var registerModel = new RegisterViewModel()
                {
                    Id = register.Id,
                    StudentName = register.Student.FullName,
                    StudentId = register.StudentId,
                    CourseName = register.Course.Name,
                    CourseId = register.CourseId,
                    SeasonName = register.Course.Season.Name,
                    Price = register.Price,
                    PaymentLeft = register.PaymentLeft,
                    NrOfLessonStudentWillJoin = register.NrOfLessonStudentWillJoin,
                    StartToCourseDate = register.StartToCourseDate,
                    RegisterDate = register.RegisterDate,
                    DiscountName = register.Discount == null ? "Yok" : register.Discount.Name,
                    LeaveDate = register.LeaveDate,
                    GroupName = register.Group == null ? "-" : register.Group.Name
                };
                listModel.Add(registerModel);
            }
            return listModel;
        }

        public RegisterViewModel RegisterDetailsMap(Registration registration)
        {
            var model = new RegisterViewModel()
            {
                Id = registration.Id,
                CourseId = registration.CourseId,
                CourseName = registration.Course.Name,
                StudentId = registration.StudentId,
                StudentName = registration.Student.FullName,
                DiscountName = registration.Discount.Name,
                NrOfLessonStudentWillJoin = registration.NrOfLessonStudentWillJoin,
                PaymentLeft = registration.PaymentLeft,
                Price = registration.Price,
                LeaveDate = registration.LeaveDate,
                SeasonName = registration.Course.Season.Name,
                RegisterDate = registration.RegisterDate
            };

            return model;
        }
    }
}
