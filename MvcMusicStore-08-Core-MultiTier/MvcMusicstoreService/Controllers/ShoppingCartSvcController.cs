using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicstoreService.Controllers
{
    [Route("api/ShoppingCart/[action]")]
    [ApiController]
    public class ShoppingCartSvcController : ControllerBase
    {
        MusicStoreEntities storeDB;
        ShoppingCart shoppingCart;

        public ShoppingCartSvcController(MusicStoreEntities _storeDB, ShoppingCart _shoppingCart)
        {
            storeDB = _storeDB;
            shoppingCart = _shoppingCart;
        }

        //
        // GET: /ShoppingCart/CartItems
        [HttpGet]
        public ActionResult<List<Cart>> CartItems(int id)
        {
            return shoppingCart.GetCartItems();
        }

        // GET: /ShoppingCart/Total
        [HttpGet]
        public ActionResult<decimal> Total(int id)
        {
            return shoppingCart.GetTotal();
        }


        // GET: /ShoppingCart/Count
        [HttpGet]
        public ActionResult<int> Count(int id)
        {
            return shoppingCart.GetCount();
        }

        //
        // GET: /Store/AddToCart/5

        [HttpPost]
        public ActionResult AddToCart(Cart cart)
        {

            // Retrieve the album from the database
            var addedAlbum = storeDB.Albums
                .Single(album => album.AlbumId == cart.AlbumId);

            // Add it to the shopping cart
            shoppingCart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return Ok();
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult<int> RemoveFromCart(Cart cart)
        {
            // Remove the item from the cart
            int itemCount = shoppingCart.RemoveFromCart(cart.RecordId);
            return itemCount;
        }
    }
}