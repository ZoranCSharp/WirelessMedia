using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IWebHostEnvironment _hostingEnvironment;

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

                    string listVM = JsonConvert.SerializeObject(ProductVM.Product);

                    System.IO.File.WriteAllText(@".\JSON\product.json", JsonConvert.SerializeObject(productName));

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
            if (id == null)
            {
                return NotFound();
            }
            ProductVM.Product = await _db.Product.Include(z => z.Category)
                                           .SingleOrDefaultAsync(z => z.Id == id);

            if(ProductVM.Product == null)
            {
                return NotFound();
            }
            
            return View(ProductVM);
        }

        //POST - EDIT
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {            
                if(id == null)
                {
                    return NotFound();
                }

            if (!ModelState.IsValid)
            {
                return View(ProductVM);
            }
            
            var productFromDB = await _db.Product.FindAsync(ProductVM.Product.Id);
                    productFromDB.Name = ProductVM.Product.Name;
                    productFromDB.Description = ProductVM.Product.Description;
                    productFromDB.Manufacturer = ProductVM.Product.Manufacturer;
                    productFromDB.Supplier = ProductVM.Product.Supplier;
                    productFromDB.Price = ProductVM.Product.Price;
                    productFromDB.CategoryId = ProductVM.Product.CategoryId;
            
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

           
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProductVM.Product = await _db.Product.Include(z => z.Category)
                                         .SingleOrDefaultAsync(z => z.Id == id);

            if(ProductVM.Product == null)
            {
                return NotFound();
            }

            return View(ProductVM);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProductVM.Product = await _db.Product.Include(z => z.Category).SingleOrDefaultAsync(z => z.Id == id);

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            _db.Product.Remove(ProductVM.Product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - DESCRIPTION
        public async Task<IActionResult> Description(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProductVM.Product = await _db.Product.SingleOrDefaultAsync(z => z.Id == id);

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            return View(ProductVM);


        }
    }
}
