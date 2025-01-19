using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Areas.Register.Models;
using Restaurant_Management_System.DAL;

namespace Restaurant_Management_System.Areas.Register.Controllers
{
    [Area("Register")]
    public class RegisterController : Controller
    {
        private readonly DAL_Registration dal;

        public RegisterController()
        {
            dal = new DAL_Registration();
        }
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RegisterationModel lm)
        {
            try
            {
                String role_of_user = dal.addUser(lm);
                HttpContext.Session.SetString("UserName", lm.Name);
                HttpContext.Session.SetString("Role", lm.Role);
                if (HttpContext.Session.GetString("UserName") != null)
                {
                    if (role_of_user == "User")
                    {
                        return RedirectToAction("Index", "User", new { area = "User" });
                    }
                    else if (role_of_user == "Admin")
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }

                }
                return View();
            }
            catch
            {
                return View();
            }
        }

    }
}
