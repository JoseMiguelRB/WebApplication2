using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Data.Entities;

namespace WebApplication2.Data
{
    public interface IRepository
    {
        void AddProduct(Product product);
        Product GetProduct(int id);
        IEnumerable<Product> GetProducts();
        bool ProductExists(int id);
        void RemoveProduct(Product product);
        void UpdateProduct(Product product);
        Task<bool> SaveAllAsync();
    }
}