using WebApplication2.Data.Entities;
using System.Linq;

namespace WebApplication2.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();
    }
}
