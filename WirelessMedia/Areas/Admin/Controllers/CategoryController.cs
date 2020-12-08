using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WirelessMedia.Data;
using WirelessMedia.Models;

namespace WirelessMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController( ApplicationDbContext db)
        {
            _db = db;
        }

        //GET Action Method
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.ToListAsync());
        }
        
        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //if valid
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _db.Category.FindAsync(id);

                if(category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
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
                var category = await _db.Category.FindAsync(id);

                if(category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(category);
                }
            }
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Category.FindAsync(id);
            

            if (category == null)
            {
                return View();
            }
            else
            {
                _db.Category.Remove(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }            
        }       
    }
}
