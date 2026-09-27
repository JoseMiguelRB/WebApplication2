using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApplication2.Data.Entities;

namespace WebApplication2.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
