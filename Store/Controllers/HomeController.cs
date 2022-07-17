using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ProductsContext db;
        public HomeController(ILogger<HomeController> logger, ProductsContext context)
        {
            _logger = logger;
            db = context;
            // добавим начальные данные для тестирования
            if (!db.Types.Any())
            {
                Store.Models.Type drink = new Store.Models.Type { Name = "Drink" };
                Store.Models.Type diary = new Store.Models.Type { Name = "Diary" };
                Store.Models.Type meat = new Store.Models.Type { Name = "Meat" };
                Store.Models.Type fruit = new Store.Models.Type { Name = "Fruit" };

                Product product1 = new Product { Name = "Pepsi", Type = drink, Price = 1 };
                Product product2 = new Product { Name = "Wather", Type = drink, Price = 1 };
                Product product3 = new Product { Name = "Chicken", Type = meat, Price = 2 };
                Product product4 = new Product { Name = "Pork", Type = meat, Price = 3 };
                Product product5 = new Product { Name = "Beef", Type = meat, Price = 4 };
                Product product6 = new Product { Name = "Milk", Type = diary, Price = 2 };
                Product product7 = new Product { Name = "Joghurt", Type = diary, Price = 2 };
                Product product8 = new Product { Name = "Apple", Type = fruit, Price = 1 };

                db.Types.AddRange(drink, meat, diary, fruit);
                db.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8);
                db.SaveChanges();
            }
        }

        public ActionResult Index(int? type, string? name, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Product> product = db.Products.Include(p => p.Type);

            if (type != null && type != 0)
            {
                product = product.Where(p => p.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                product = product.Where(p => p.Name!.Contains(name));
            }

            product = sortOrder switch
            {
                SortState.NameDesc => product.OrderByDescending(s => s.Name),
                SortState.PriceAsc => product.OrderBy(s => s.Price),
                SortState.PriceDesc => product.OrderByDescending(s => s.Price),
                SortState.TypeAsc => product.OrderBy(s => s.Type!.Name),
                SortState.TypeDesc => product.OrderByDescending(s => s.Type!.Name),
                _ => product.OrderBy(s => s.Name),
            };

            List<Store.Models.Type> companies = db.Types.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            companies.Insert(0, new Store.Models.Type { Name = "Все", Id = 0 });

            ProductListViewModel viewModel = new ProductListViewModel
            {
                Products = product.ToList(),
                Types = new SelectList(companies, "Id", "Name", type),
                Name = name,
                SortViewModel = new SortViewModel(sortOrder)
            };
            return View(viewModel);
        }
        public ActionResult ProductList(int? type, string? name)
        {
            IQueryable<Product> users = db.Products.Include(p => p.Type);
            if (type != null && type != 0)
            {
                users = users.Where(p => p.TypeId == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(p => p.Name!.Contains(name));
            }

            List<Store.Models.Type> companies = db.Types.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            companies.Insert(0, new Store.Models.Type { Name = "Все", Id = 0 });

            ProductListViewModel viewModel = new ProductListViewModel
            {
                Products = users.ToList(),
                Types = new SelectList(companies, "Id", "Name", type),
                Name = name
            };
            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product product = new Product { Id = id.Value };
                db.Entry(product).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
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
            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}