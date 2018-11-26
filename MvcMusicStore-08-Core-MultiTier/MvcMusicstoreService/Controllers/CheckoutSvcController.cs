using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicstoreService.Controllers
{
    [Route("api/Checkout/[action]")]
    [ApiController]
    public class CheckoutSvcController : ControllerBase
    {
        MusicStoreEntities storeDB;
        ShoppingCart shoppingCart;

        public CheckoutSvcController(MusicStoreEntities _storeDB, ShoppingCart _shoppingCart)
        {
            storeDB = _storeDB;
            shoppingCart = _shoppingCart;
        }



        [HttpPost]
        public ActionResult<int> AddressAndPayment(Order order)
        {
                //Save Order
            storeDB.Orders.Add(order);
            storeDB.SaveChanges();

            //Process the order
            shoppingCart.CreateOrder(order);

            return order.OrderId;
        }

        [HttpPost]
        public ActionResult<bool> Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            return isValid;
        }
    }
}