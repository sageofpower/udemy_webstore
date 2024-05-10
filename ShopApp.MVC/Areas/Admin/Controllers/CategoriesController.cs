﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Data;
using ShopApp.DataAccess.Models;
using ShopApp.DataAccess.Repository.IRepository;

namespace ShopApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unit_of_work;

        public CategoriesController(IUnitOfWork unit_of_work)
        {
            _unit_of_work = unit_of_work;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _unit_of_work.CategoryRepository.GetAllAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //          IEnumerable<SelectListItem> CategoryList = _context.Category.Select(u => new SelectListItem
        //          {
        //              Text = u.Name,
        //              Value = u.Id.ToString()
        //          });

        //	return View(await _context.Category.ToListAsync());
        //}


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unit_of_work.CategoryRepository.GetAsync(u => u.Id == id);
            //var category = await _repository.Category
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                _unit_of_work.CategoryRepository.Add(category);
                await _unit_of_work.SaveAsync();
                TempData["success"] = "Category Created";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unit_of_work.CategoryRepository.GetAsync(u => u.Id == id);
            //var category = await _repository.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit_of_work.CategoryRepository.Update(category);
                    await _unit_of_work.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Category Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _unit_of_work.CategoryRepository.GetAsync(u => u.Id == id);
            //var category = await _repository.Category
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unit_of_work.CategoryRepository.GetAsync(u => u.Id == id);
            //var category = await _repository.Category.FindAsync(id);
            if (category != null)
            {
                _unit_of_work.CategoryRepository.Remove(category);
            }

            await _unit_of_work.SaveAsync();
            TempData["success"] = "Category Deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _unit_of_work.CategoryRepository.Exists(e => e.Id == id);
        }
    }
}
