using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WirelessMedia.Data;
using WirelessMedia.Models.ViewModel;

namespace WirelessMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProductViewModel ProductVM { get; set; }

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            ProductVM = new ProductViewModel()
            {
                Category = _db.Category,
                Product = new Models.Product()
            };
        }

        //GET - ACTION METHOD
        public async Task<IActionResult> Index()
        {
            var productList = await _db.Product.Include(z => z.Category).ToListAsync();

            return View(productList);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(ProductVM);
        }
    }
}
