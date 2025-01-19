using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Areas.Login.Models;
using Restaurant_Management_System.BAL;
using Restaurant_Management_System.DAL;

namespace Restaurant_Management_System.Areas.Login.Controllers
{
    [Area("Login")]
    public class LoginController : Controller
    {
        private readonly DAL_Login dal;
        public LoginController()
        {
            dal = new DAL_Login();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel lm)
        {
            List<string> value = dal.loginUser(lm);
            String role_of_user = value[2];
            if (Convert.ToBoolean(value[0]) == false)
            {
                TempData["not_found_msg"] = "Sorry !! You are not registered....";
                return RedirectToAction("Index", "Register", new {  area="Register" });
            }
            int Is_Active_state = Convert.ToInt32(Convert.ToBoolean(value[3]));
            if (Convert.ToBoolean(value[0]) == true)
            {
                HttpContext.Session.SetString("UserName", value[1]);

                HttpContext.Session.SetString("UserID", value[4]);
                if (role_of_user == "User" && Is_Active_state != 0)
                {
                    HttpContext.Session.SetString("Role", role_of_user);
                    //CV.UserID();
                    return RedirectToAction("Index", "User", new { area = "User" });
                }
                else if (role_of_user == "Admin")
                {
                    HttpContext.Session.SetString("Role", role_of_user);
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                else
                {
                    if(Is_Active_state == 0)
                    {
                        HttpContext.Session.Clear();
                        ViewBag.is_active_msg = "Sorry !! You are not permitted....";
                    }
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Register",new { area="Register" });
        }

    }
}
