using Microsoft.AspNetCore.Mvc;
using Restaurant_Management_System.Areas.Admin.Models;
using Restaurant_Management_System.Areas.Admin.ViewModel;
using Restaurant_Management_System.Areas.Register.Models;
using Restaurant_Management_System.BAL;
using Restaurant_Management_System.DAL;

namespace Restaurant_Management_System.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    public class AdminController : Controller
    {
        #region Configuration
        private readonly DAL_Admin dal;
        IWebHostEnvironment env; // for image uploading

        #region Controller
        public AdminController(IWebHostEnvironment env)
        {
            this.env = env;
            dal = new DAL_Admin();
        }
        #endregion

        #endregion

        #region Index Page
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region User List
        public IActionResult UserList()
        {
            List<RegisterationModel> userList = dal.SelectAllUsers();
            return View(userList);
        }
        #endregion

        #region User Details
        public IActionResult Details(int id)
        {
            RegisterationModel user = dal.SelectUserByID(id);
            return View(user);
        }
        #endregion

        #region Activate/Deactivate User
        public IActionResult Edit(int id)
        {
            RegisterationModel user = dal.SelectUserByID(id);
            int state = dal.SelectStateOfUser(id);
            ViewBag.state = state;
            return View(user);
        }
        public void ToggleIsActive(int id, int is_active)
        {
            dal.ActivateDeactivateUser(id, is_active);
        }
        #endregion

        #region Delete User
        public IActionResult Delete(int id)
        {
            dal.DeleteUser(id);
            return RedirectToAction("UserList");
        }
        #endregion

        #region Add Pizza 
        public IActionResult AddPizza()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPizza(MenuViewModel pizza)
        {
            dal.AddPizza(pizza,env);
            return View();
        }
        #endregion

        #region Pizza List
        public IActionResult PizzaList()
        {
            List<MenuModel> pizzaList = dal.SelectAllPizzas();
            return View(pizzaList);
        }
        #endregion

        #region Pizza Details
        public IActionResult PizzaDetails(int id)
        {
            MenuModel pizza = dal.SelectPizzaByID(id);
            return View(pizza);
        }
        #endregion

        #region Pizza Edit 
        public IActionResult EditPizza(int id)
        {
            MenuViewModel oldPizza = dal.SelectPizzaByViewModelID(id,env);
            return View(oldPizza);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPizza(int id,MenuViewModel newPizza)
        {
            dal.EditPizza(id,newPizza,env);
            return View();
        }
        #endregion

        #region Delete Pizza
        public IActionResult DeletePizza(int id) 
        {
            dal.DeletePizza(id);
            return RedirectToAction("PizzaList");
        }
        #endregion

        public IActionResult HistoryList()
        {
            List<OrderHistoryModel> historyList = dal.GetAllHistory();
            return View(historyList);
        }

        [HttpPost]
        public JsonResult UpdateStatus(int UserID,string status)
        {
            try
            {
                dal.UpdateStatus(UserID,status);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
