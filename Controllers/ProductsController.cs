using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPShopBag.Data;
using ASPShopBag.Models;

namespace ASPShopBag.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductsVM modelVM = new ProductsVM()
            {
                Name=product.Name,
                Description=product.Description,
                Price=product.Price,
                Type=product.Type
            };

            return View(modelVM);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Type")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //1. зареждам искания id от БД .... за промяна на стойностите
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            //2. Създавм модела, с който ще визуализирам за промяна на стойностите
            //3. Пълня от БД в полетата на екрана
            ProductsVM model = new ProductsVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Type = product.Type
            };

            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Type")] ProductsVM product)
        {
            //1. Намирам записа в БД
            Product modelToDB = await _context.Products.FindAsync(id);
            if (modelToDB == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                //презареждаме страницата
                return View(modelToDB);
            }
            //2. Прехвърлям всичко в модела за БД .... готвим се за запис в БД
            //modelToDB.Id = product.Id;// не се пипа при промяна
            modelToDB.Name = product.Name;
            modelToDB.Price = product.Price;
            modelToDB.Description = product.Description;
            modelToDB.Type = product.Type;

            //3. ЗАПИС в БД
            try
            {
                _context.Update(modelToDB);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(modelToDB.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //4. Извикваме Details на актуализирания запис
            return RedirectToAction("Details", new { id = id });

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
