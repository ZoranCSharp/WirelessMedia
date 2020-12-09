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

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            var productID = await _db.Product.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            ProductViewModel productVM = new ProductViewModel()
            {
                Product = productID,
                Category = _db.Category,
                StatusMessage = StatusMessage
            };

            return View(productVM);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                    var productFromDB = await _db.Product.FindAsync(id);
                    productFromDB.Name = ProductVM.Product.Name;
                    productFromDB.Description = ProductVM.Product.Description;
                    productFromDB.Manufacturer = ProductVM.Product.Manufacturer;
                    productFromDB.Supplier = ProductVM.Product.Supplier;
                    productFromDB.Price = ProductVM.Product.Price;
                    productFromDB.CategoryId = ProductVM.Product.CategoryId;

                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
            }

            ProductViewModel product = new ProductViewModel()
            {
                Product = ProductVM.Product,
                Category = _db.Category,
                StatusMessage = StatusMessage
            };

            return View(product);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _db.Product.FindAsync(id);

                if(product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
                }
            }
        }
    }
}
