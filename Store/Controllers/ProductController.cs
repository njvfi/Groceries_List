using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        ProductsContext db;

        public ProductController(ILogger<ProductController> logger, ProductsContext context)
        {
            _logger = logger;
            db = context;
        }

        public async Task<ActionResult> Index(int? id)
        {
            if (id != null)
            {
                Product? product = await db.Products.Include(pr => pr.Type).FirstOrDefaultAsync(p => p.Id == id);
                if (product != null) return View(product);
            }

            return NotFound();
        }

        #region Creation
        public IActionResult Create()
        {
            List<TodoList.Models.Type> companies = db.Types.ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product product = new Product { Id = id.Value };
                db.Entry(product).State = EntityState.Deleted;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }


        #region Edition
        public async Task<IActionResult> Edit(int? id)
        {
            List<TodoList.Models.Type> companies = db.Types.ToList();

            if (id != null)
            {
                Product? product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null) return View(product);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            db.Products.Update(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "home");
        }
        #endregion
    }
}
