using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WirelessMedia.Data;

namespace WirelessMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET - ACTION METHOD
        public async Task<IActionResult> Index()
        {
            var productList = await _db.Product.Include(z => z.Category).ToListAsync();

            return View(productList);
        }

       
    }
}
