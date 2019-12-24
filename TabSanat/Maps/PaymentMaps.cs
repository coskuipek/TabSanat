using System.Collections.Generic;
using TabSanat.Model;
using TabSanat.ViewModels.Display;

namespace TabSanat.Maps
{
    public class PaymentMaps
    {
        public static List<PaymentViewModel> PaymentIndexMap(IEnumerable<Payment> payments)
        {
            List<PaymentViewModel> listModel = new List<PaymentViewModel>();

            if (payments == null)
                return listModel;


            foreach (var payment in payments)
            {
                PaymentViewModel model = new PaymentViewModel()
                {
                    Id = payment.Id,
                    StudentName = payment.Student.FullName,
                    StudentId = payment.StudentId,
                    CourseName = payment.Registration.Course.Name,
                    CourseId = payment.Registration.CourseId,
                    PaymentDate = payment.PaymentDate,
                    PaymentTypeName = payment.PaymentType.Name,
                    UserName = $"{payment.AppUser.FirstName} {payment.AppUser.LastName}",
                    IsGiveBack = payment.IsGiveBack,
                    Price = payment.IsGiveBack ? payment.Price *= -1 : payment.Price,
                    Taksit = payment.Taksit

                };
                listModel.Add(model);
            }
            return listModel;
        }
    }
}
