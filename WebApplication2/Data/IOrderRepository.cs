using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Entities;

namespace WebApplication2.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

    }
}
