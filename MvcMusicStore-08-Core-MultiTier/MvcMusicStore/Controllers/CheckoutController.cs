using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcMusicStore.Helpers;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApiHelper apiHelper;
        const string PromoCode = "FREE";


        public CheckoutController(IConfiguration _config)
        {
            apiHelper = new ApiHelper(_config);
        }

       
      
        public ActionResult AddressAndPayment()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(IFormCollection values)
        {
            var order = new Order();
            bool success = TryUpdateModelAsync<Order>(order).Result;

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    #region old code
                    //Save Order
                    //storeDB.Orders.Add(order);
                    //storeDB.SaveChanges();

                    ////Process the order
                    //var cart = ShoppingCart.GetCart(this.HttpContext, storeDB);
                    //cart.CreateOrder(order);

                    //return RedirectToAction("Complete",
                    //    new { id = order.OrderId });
                    #endregion

                    try
                    {
                        int orderId = await apiHelper.PostAsync<Order, int>("/api/Checkout/AddressAndPayment", order);
                        return RedirectToAction("Complete", new { id = orderId });
                    }
                    catch (Exception)
                    {
                        //Log
                        return View(order);
                    }
                }

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete

        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                bool isValid = await apiHelper.PostAsync<int, bool>("/api/Checkout/Complete", id);
                if (isValid)
                {
                    int retId = id;
                    return RedirectToAction("Complete", new { id = retId });
                }
                return View("Invalid checkout");
            }
            catch (Exception ex)
            {
                //Log
                return View("Error: " + ex.Message);
            }


        }
    }
}
