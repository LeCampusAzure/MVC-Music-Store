using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcMusicStore.Helpers;
using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMusicStore.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        ApiHelper apiHelper;

        public CartSummaryViewComponent(IConfiguration _config)
        {
            apiHelper = new ApiHelper(_config);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["CartCount"] = await apiHelper.GetAsync<int>("/api/ShoppingCart/Count");
            return View();
        }
    }
}
