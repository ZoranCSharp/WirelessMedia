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

        [TempData]
        public string StatusMessage { get; set; }

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            ProductVM = new ProductViewModel()
            {                
                Product = new Models.Product(),
                Category = _db.Category,
                StatusMessage = StatusMessage
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

        //POST - CREATE
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
              
                var productName = _db.Product.Where(z => z.Name == ProductVM.Product.Name && z.Manufacturer == ProductVM.Product.Manufacturer && z.CategoryId == ProductVM.Product.CategoryId);

                if(productName.Count() > 0)
                {
                    StatusMessage = "Error : There is already product " + productName.First().Name + " whose manufacturer is " + productName.First().Manufacturer + ". Please choose different category.";               
                }
                else
                {
                    _db.Product.Add(ProductVM.Product);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ProductViewModel product = new ProductViewModel()
            {
                Product = ProductVM.Product,
                Category = _db.Category,
                StatusMessage = StatusMessage
            };

            return View(product);

        }
    }
}
