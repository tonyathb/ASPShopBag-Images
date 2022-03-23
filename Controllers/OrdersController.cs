using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPShopBag.Data;
using Microsoft.AspNetCore.Identity;
using ASPShopBag.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPShopBag.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _sigInManager;
        // private readonly RoleManager<IdentityRole> _roleManager;

        public OrdersController(ApplicationDbContext context,
                                UserManager<User> userManager)//,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        public async Task<IActionResult> Test()
        {
            var userLoged = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddToRoleAsync(userLoged, Roles.Admin.ToString());   //"Admin");
            var roles = _userManager.GetRolesAsync(userLoged);
            return Content("OK !!!");
        }
        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()//GetMyOrders()
        {
            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var currentUser = _userManager.GetUserId(User);
                var myOrders = _context.Orders
                               .Include(o => o.Product)
                               .Include(u => u.User)
                               .Where(x => x.UserId == currentUser.ToString())
                               .ToListAsync();

                return View(await myOrders);
            }

        }
        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1()
        {
            var applicationDbContext = _context.Orders
                .Include(o => o.Product)
                .Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(u => u.User) //ako go nqma nqmam dostyp do poletata na User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create()
        {
            OrdersVM model = new OrdersVM();

            model.Products = _context.Products.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = (x.Id == model.ProductId)
            }
            ).ToList();


            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,OrderedOn")] OrdersVM order)
        {
            if (!ModelState.IsValid)
            {
                //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
                OrdersVM model = new OrdersVM();
                model.Products = _context.Products.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = (x.Id == model.ProductId)
                }
                ).ToList();
                return View(model);
            }
            Order modelToDB = new Order
            {
                ProductId = order.ProductId,
                UserId = _userManager.GetUserId(User),
                OrderedOn = DateTime.Now
            };
            _context.Add(modelToDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //Zarejdam OrdersVM
            OrdersVM model = new OrdersVM();
            model.Products = _context.Products.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = (x.Id == model.ProductId)
            }
            ).ToList();
            return View(model);
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            //return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId,OrderedOn")] OrdersVM order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(order);
            }
            //record in DB
            Order modeFromDB = new Order
            {
                Id = id,
                UserId = _userManager.GetUserId(User),
                ProductId = order.ProductId,
                OrderedOn = DateTime.Now
            };
            try
            {
                _context.Update(modeFromDB);//(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(modeFromDB.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Orders/Edit/5

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(u => u.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            // .FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
