using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcMusicStore.Helpers;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApiHelper apiHelper;
        public const string CartSessionKey = "CartId";

        public ShoppingCartController(IConfiguration _config)
        {
            apiHelper = new ApiHelper(_config);
        }

        //
        // GET: /ShoppingCart/

        public async Task<ActionResult> Index()
        {
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await apiHelper.GetAsync<List<Cart>>("/api/ShoppingCart/CartItems?cartId=" + GetCartId()),
                CartTotal = await apiHelper.GetAsync<decimal>("/api/ShoppingCart/Total?cartId=" + GetCartId())
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public async Task<ActionResult> AddToCart(int id)
        {
            var cart = new Cart()
            {
                AlbumId = id,
                CartId = GetCartId()
            };
            await apiHelper.PostAsync<Cart>("/api/ShoppingCart/AddToCart", cart);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var cart = new Cart()
            {
                RecordId = id,
                CartId = GetCartId()
            };
            
            int itemCount = await apiHelper.PostAsync<Cart, int>("/api/ShoppingCart/RemoveFromCart", cart);
            string albumName = await apiHelper.GetAsync<string>("/api/Store/AlbumName?id=" + id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = HtmlEncoder.Default.Encode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = await apiHelper.GetAsync<decimal>("/api/ShoppingCart/Total?cartId=" + GetCartId()),
                CartCount = await apiHelper.GetAsync<int>("/api/ShoppingCart/Count?cartId=" + GetCartId()),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
            //return RedirectToAction("Index");
        }


        private string GetCartId()
        {
            var context = this.HttpContext;
            context.Session.LoadAsync().Wait();
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                    context.Session.CommitAsync().Wait();
                }
            }

            return context.Session.GetString(CartSessionKey);
        }
    }
}