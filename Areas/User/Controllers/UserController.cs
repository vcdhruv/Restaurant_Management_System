using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Restaurant_Management_System.Areas.Admin.Models;
using Restaurant_Management_System.Areas.Admin.ViewModel;
using Restaurant_Management_System.Areas.User.ViewModel;
using Restaurant_Management_System.BAL;
using Restaurant_Management_System.DAL;

namespace Restaurant_Management_System.Areas.User.Controllers
{
    [Area("User")]
    [CheckAccess]
    public class UserController : Controller
    {
        private readonly DAL_Admin admin;
        private readonly DAL_User user;

        public UserController()
        {

            admin = new DAL_Admin();
            user = new DAL_User();
        }
        public IActionResult Index()
        {
            List<MenuModel> pizzaList =  admin.SelectAllPizzas();
            return View(pizzaList);
        }

        [HttpPost]
        public JsonResult AddToCart(int id)
        {
            try
            {
                user.AddToCart(id);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public IActionResult Cart() 
        {
            List<CartViewModel> cartItems = user.SelectAllCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public JsonResult UpdateQuantity(int PizzaID, int Qty )
        {
            try
            {
                user.UpdateQuantity(PizzaID,Qty);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        public IActionResult DeleteCartItem(int ItemID)
        {
            bool is_deleted = user.DeleteCartItem(ItemID);
            return RedirectToAction("Cart");
        }
        public IActionResult PlaceOrder()
        {
            List<OrderViewModel> orders =  user.GetOrders();
            return View(orders);
        }

        [HttpPost]
        public IActionResult CheckOut(int CartID, float payable_amount)
        {
            user.PlaceOrder(CartID, payable_amount);
            return RedirectToAction("PlaceOrder");
        }

        public IActionResult ConfirmOrder()
        {
            OrderViewModel order =  user.getConfirmOrderDetails();
            return View(order);
        }

        public IActionResult OrderHistory()
        {
            List<OrderHistoryModel> history = user.getOrderHistory();
            return View(history);
        }
    }
}
