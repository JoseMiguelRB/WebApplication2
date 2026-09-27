using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Data.Entities;
using WebApplication2.Helpers;
using WebApplication2.Web.Models;


public class ProductsController : Controller
{

    private readonly IProductRepository _productRepository;
    private readonly IUserHelper _userHelper;

    public ProductsController(IProductRepository productRepository, IUserHelper userHelper)

    {
        _productRepository = productRepository;
        _userHelper = userHelper;

    }

    // GET: PRODUCTS
    public IActionResult Index()
    {
        return View(_productRepository.GetAll().OrderBy(p => p.Name));
    }

    // GET: PRODUCTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: PRODUCTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
            if (ModelState.IsValid)
            {

            var path = string.Empty;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot\\images\\products", file);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                path = $"~/images/products/{file}";
            }

            var product = this.ToProduct(model, path);
            product.User = await _userHelper.GetUserByEmailAsync("romerojosemiguelbello@gmail.com");
            await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    // GET: PRODUCTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }
        var model = this.ToProductViewModel(product);
        return View(model);
    }

    private Product ToProduct(ProductViewModel model, string path)
    {
        return new Product
        {
            Id = model.Id,
            ImageUrl = path,
            IsAvailable = model.IsAvailable,
            LastPurchase = model.LastPurchase,
            LastSale = model.LastSale,
            Name = model.Name,
            Price = model.Price,
            Stock = model.Stock,
            User = model.User
        };
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductViewModel model)
    {

        if (ModelState.IsValid)
        {
            try
            {
                var path = model.ImageUrl;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";
                    path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\products",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/products/{file}";
                }

                var product = this.ToProduct(model, path);

                product.User = await _userHelper.GetUserByEmailAsync("romerojosemiguelbello@gmail.com");
                await _productRepository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productRepository.ExistAsync(model.Id))
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
        return View(model);
    }

    // GET: PRODUCTS/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }
        var model = this.ToProductViewModel(product);
        return View(product);
    }

    private ProductViewModel ToProductViewModel(Product product)
    {
        return new ProductViewModel
        {
            Id = product.Id,
            IsAvailable = product.IsAvailable,
            LastPurchase = product.LastPurchase,
            LastSale = product.LastSale,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Name = product.Name,
            Stock = product.Stock,
            User = product.User
        };
    }

    // POST: PRODUCTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        await _productRepository.DeleteAsync(product);
        return RedirectToAction(nameof(Index));
    }

}
