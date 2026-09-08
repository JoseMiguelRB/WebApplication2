using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Entities;
using WebApplication2.Data;
using System.Linq;


public class ProductsController : Controller
{

    private readonly IRepository _repository;

    public ProductsController(IRepository repository)
    {
        _repository = repository;
    }

    // GET: PRODUCTS
    public IActionResult Index()
    {
        return View(_repository.GetProducts());
    }

    // GET: PRODUCTS/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _repository.GetProduct(id.Value);
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
            _repository.AddProduct(product);
            await _repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: PRODUCTS/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _repository.GetProduct(id.Value);
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
                _repository.UpdateProduct(product);
                await _repository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.ProductExists(product.Id))
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

        var product = _repository.GetProduct(id.Value);
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
        var product = _repository.GetProduct(id);
        _repository.RemoveProduct(product);

        await _repository.SaveAllAsync();
        return RedirectToAction(nameof(Index));
    }

}
