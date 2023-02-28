using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryBorrower.Models;
using Microsoft.Extensions.Primitives;

namespace LibraryBorrower.Controllers
{
    public class LibraryItemsController : Controller
    {
        private readonly LibraryDbContext _context;

        public LibraryItemsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: LibraryItems
        //This is the index page. This is where the table is loaded and viewed. I have the LibraryItem
        //table loaded into the view and it works, but I cant load into the categoryname at all. 
        //I have been trying everything, have been trying and trying. The LibraryItem have a FK to
        //Category and I can load as you can see in the CategoryName in the table, but the data is not shown.
        //You can search for types and filter them. If you want to get back to see everything, just 
        //dont have any values in the textbox and do filter. 
        public async Task<IActionResult> Index(string SearchString)
        {
            var typeSearch = from m in _context.libraryItem select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                typeSearch = typeSearch.Where(s => s.Type.Contains(SearchString));
                return View(typeSearch);
            }

            return View(await _context.libraryItem.OrderBy(m => m.categoryID.CategoryName).ToListAsync());
            //return View();
        }

        // GET: LibraryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.libraryItem
                .FirstOrDefaultAsync(m => m.ID == id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // GET: LibraryItems/Create
        public IActionResult Create()
        {
            List<Category> Category = new List<Category>();
            Category = (from kategory in _context.category
                        select kategory).ToList();

            Category.Insert(0, new Category { ID = 0, CategoryName = "Select" });

            ViewBag.ListofCategory = Category;
            return View();
        }

        // POST: LibraryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Creates an item and adds it into the LibraryItem table. The select type is hard coded
        //with values inserted into a Select Dropdown list where the user can select which type to add.
        //I had problems adding CategoryName when you create an item. The model have a FK to the Category
        //table and i can populate the dropdown list with values from the Category table but i can't add it
        //into the LibraryItem table when I create the item. I have been working hard and hard to find a
        //good solution but the One-To-Many or rather Many-To-Many relationship wont work. I think its
        //something to do with the onmodelcreating method in the DBContext but i have tried everything
        //and it still doesnt work. 

        //Also i tried to implement a Acronym/Abbreviation in the title column in the LibraryItem table, but
        //dont know where to implement that.
        public async Task<IActionResult> Create([Bind("ID,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Borrower,BorrowDate,Type,CategoryName")] LibraryItem libraryItem, string types)
        {
            var list = new List<string>() { "book", "reference book", "audio book", "dvd" };
            ViewBag.list = list;

            List<Category> Category = new List<Category>();
            Category = (from kategory in _context.category
                        select kategory).ToList();

            Category.Insert(0, new Category { ID = 0, CategoryName = "Select" });

            ViewBag.ListofCategory = Category;

            if (ModelState.IsValid)
            {
                _context.AddRange(libraryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryItem);
        }

        // GET: LibraryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.libraryItem.FindAsync(id);
            if (libraryItem == null)
            {
                return NotFound();
            }
            else if(libraryItem.IsBorrowable == true)
            {
                return View(libraryItem);
            }
            return View(libraryItem);
        }

        // POST: LibraryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Updates a item that is choosen for edits. In the edit page you can check in and checkout books
        //and add name who was borrowing and and which date. When that is done the fields in table are updated.
        //It was meant that it should include categoryname in the edit as well, but couldnt get that to work. 
        //It was also meant that if the item is not checked for IsBorrowable that it should be greyed out, but
        //couldnt get any solution to work. 
        public async Task<IActionResult> Edit(int id, [Bind("ID,categoryID,Title,Author,Pages,RunTimeMinutes,IsBorrowable,Borrower,BorrowDate,Type")] LibraryItem libraryItem, string save, string CheckOut, string CheckIn)
        {
            if (id != libraryItem.ID)
            {
                return NotFound();
            }

            //var canBorrow = _context.libraryItem.Where(b => b.IsBorrowable == true);

            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(libraryItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LibraryItemExists(libraryItem.ID))
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
            }
            else if (!string.IsNullOrEmpty(CheckOut) && libraryItem.IsBorrowable == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(libraryItem);
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LibraryItemExists(libraryItem.ID))
                        {
                            //ModelState.AddModelError(string.Empty, "The item cant be borrowed!");
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (!string.IsNullOrEmpty(CheckIn))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        libraryItem.BorrowDate = null;
                        libraryItem.Borrower = null;
                        _context.Update(libraryItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LibraryItemExists(libraryItem.ID))
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
            }
            return View(libraryItem);
        }

        // GET: LibraryItems/Delete/5
        //Deletes a library item
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryItem = await _context.libraryItem
                .FirstOrDefaultAsync(m => m.ID == id);
            if (libraryItem == null)
            {
                return NotFound();
            }

            return View(libraryItem);
        }

        // POST: LibraryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Deletes a library item with a confirmation. 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libraryItem = await _context.libraryItem.FindAsync(id);
            _context.libraryItem.Remove(libraryItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryItemExists(int id)
        {
            return _context.libraryItem.Any(e => e.ID == id);
        }
    }
}
