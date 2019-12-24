using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TabSanat.Controllers
{
    [Authorize]
    public class HataController : Controller
    {
        [Route("Hata/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Aradığınız sayfaya ulaşılamıyor.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Bir hata oluştu.";
                    break;
            }
            return View("NotFound");
        }
        [Route("YetkisizGiris")]
        public IActionResult YetkisizGiris()
        {

            return View("Yetki");
        }
    }
}