using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryBorrower.Models;

namespace LibraryBorrower.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly LibraryDbContext _context;

        public CategoriesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        //Shows the items in category. 
        public async Task<IActionResult> Index()
        {
            return View(await _context.category.OrderBy(m => m.CategoryName).ToListAsync());
        }

        // GET: Categories/Details/5
        //Details of a specific library item. It gets the id and displays the data. 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.ID == id);
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
        //This is where I add a new Category to Category Table. I have a conditional statement
        //to check if the category already exists in the table. If the category exists, the
        //item wont be inserted and you get an error message. 
        public async Task<IActionResult> Create([Bind("ID,CategoryName")] Category category)
        {

            //var checkDuplicate = _context.category.Where(x => x.CategoryName == category.CategoryName);

            if (!_context.category.Any(c => c.CategoryName == category.CategoryName))
            {
                if (ModelState.IsValid)
                {
                    _context.AddRange(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.AddModelError(string.Empty, "Value already exists!");
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.category.FindAsync(id);
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
        //Edits the category item. 
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryName")] Category category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {

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

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Deletes the item in both parent and child table. If the item is in libraryitem as a category
        //it is deleted there as well with a Cascade delete. I have set the Cascade delete in Migrations. 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.category.FindAsync(id);
            _context.category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.category.Any(e => e.ID == id);
        }
    }
}
