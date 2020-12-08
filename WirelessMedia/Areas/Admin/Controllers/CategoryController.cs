﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WirelessMedia.Data;

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
    }
}