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


        #region Controller
        public HomeController(ILogger<HomeController> logger, ProductsContext context)
        {
            _logger = logger;
            db = context;

            // добавим начальные данные для тестирования
            if (!db.Types.Any())
            {
                Store.Models.Type drink = new Store.Models.Type { Name = "Напитки" };
                Store.Models.Type diary = new Store.Models.Type { Name = "Молочные продукты" };
                Store.Models.Type meat = new Store.Models.Type { Name = "Мясо" };
                Store.Models.Type fruit = new Store.Models.Type { Name = "Фрукты" };

                Product product1 = new Product { Name = "Пепси", Description = "Pepsi («Пе́пси») — газированный безалкогольный напиток, производимый компанией PepsiCo.", Type = drink, Price = 1 };
                Product product2 = new Product { Name = "Вода", Description = "Бонáква (англ. BonAqua) — бутилированная вода, изготовлена компанией (The Coca-Cola Company).", Type = drink, Price = 1 };
                Product product3 = new Product { Name = "Курица", Description = "Курица Бройлер от фирмы Наша Ряба", Type = meat, Price = 2 };
                Product product4 = new Product { Name = "Свинина", Description = "Свининая шея от фирмы Гетьман", Type = meat, Price = 3 };
                Product product5 = new Product { Name = "Говядина", Description = "Говяжие ребра от фирмы Рамфуд", Type = meat, Price = 4 };
                Product product6 = new Product { Name = "Молоко", Description = "Молоко пастеризованное 3,5% жирности от фирмы Milibona", Type = diary, Price = 2 };
                Product product7 = new Product { Name = "Йогурт", Description = "Йогурт питьевой со вкусом персика от фирмы Milibona", Type = diary, Price = 2 };
                Product product8 = new Product { Name = "Яблоко", Description = "Яблого Сорта Браэбурн от фирмы Вертр", Type = fruit, Price = 1 };

                db.Types.AddRange(drink, diary, meat, fruit);
                db.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8);
                db.SaveChanges();
            }
            if (!db.Users.Any())
            {
                Store.Models.User Tom = new Store.Models.User { Name = "Tom", Id = 0, Account = 200 };
            }
        }
        #endregion


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


        public async Task<ActionResult> Product(int? id)
        {
            if (id != null)
            {
                Product? product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null) return View(product);
            }

            return NotFound();
        }


        #region Creation
        public IActionResult Create()
        {
            List<Store.Models.Type> companies = db.Types.ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }

            return NotFound();
        }


        #region Edition
        public async Task<IActionResult> Edit(int? id)
        {
            List<Store.Models.Type> companies = db.Types.ToList();

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
        #endregion


        #region Profile
        /*public async Task<IActionResult> Profile(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null) return View(user);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Profile(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return View();
        }*/
        #endregion


        #region Donation
        public async Task<IActionResult> AddMoney(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null) return View(user);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> AddMoney(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}