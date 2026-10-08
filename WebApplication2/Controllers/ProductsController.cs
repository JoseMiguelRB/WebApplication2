using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Helpers;
using WebApplication2.Models;
using WebApplication2.Web.Helpers;
using Microsoft.AspNetCore.Authorization;


public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IUserHelper _userHelper;

    public readonly IImageHelper _imageHelper;
    public readonly IConverterHelper _converterHelper;

    public ProductsController(IProductRepository productRepository, IUserHelper userHelper, IImageHelper imageHelper , IConverterHelper converterHelper)
    {
        _productRepository = productRepository;
        _userHelper = userHelper;
        _imageHelper = imageHelper;
        _converterHelper = converterHelper;
    }

    // GET: Products
    public IActionResult Index()
    {
        return View(_productRepository.GetAll().OrderBy(p => p.Name));
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }

        return View(product);
    }

    // GET: Products/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
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
                path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
            }

            var product = _converterHelper.ToProduct(model, path, true);
            //TODO: Modificar para o user logado
            product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            await _productRepository.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    //private Product ToProduct(ProductViewModel model, string path)
    //{
    //    return new Product
    //    {
    //        Id = model.Id,
    //        ImageUrl = path,
    //        IsAvailable = model.IsAvailable,
    //        LastPurchase = model.LastPurchase,
    //        LastSale = model.LastSale,
    //        Name = model.Name,
    //        Price = model.Price,
    //        Stock = model.Stock,
    //        User = model.User
    //    };
    //}

    // GET: Products/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }
        var model = _converterHelper.ToProductViewModel(product);
        return View(model);
    }

    // POST: Products/Edit/5
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
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                }

                var product = _converterHelper.ToProduct(model, path, false);

                //TODO: Modificar para o user logado
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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

    // GET: Products/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }

        var product = await _productRepository.GetByIdAsync(id.Value);
        if (product == null)
        {
            return new NotFoundViewResult("ProductNotFound");
        }
        var model = _converterHelper.ToProductViewModel(product);
        return View(product);
    }

    //private ProductViewModel ToProductViewModel(Product product)
    //{
    //    return new ProductViewModel
    //    {
    //        Id = product.Id,
    //        IsAvailable = product.IsAvailable,
    //        LastPurchase = product.LastPurchase,
    //        LastSale = product.LastSale,
    //        ImageUrl = product.ImageUrl,
    //        Price = product.Price,
    //        Name = product.Name,
    //        Stock = product.Stock,
    //        User = product.User
    //    };
    //}

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        await _productRepository.DeleteAsync(product);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ProductNotFound()
    {
        return View();
    }
}
