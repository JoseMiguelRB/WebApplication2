using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Entities;
using WebApplication2.Data;
using System.Linq;
using WebApplication2.Helpers;


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
    public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
            product.User = await _userHelper.GetUserByEmailAsync("romerojosemiguelbello@gmail.com");
            await _productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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
        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                product.User = await _userHelper.GetUserByEmailAsync("romerojosemiguelbello@gmail.com");
                await _productRepository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productRepository.ExistAsync(product.Id))
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
        return View(product);
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

        return View(product);
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
