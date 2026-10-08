using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data.Entities;
using WebApplication2.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            await _context.Database.EnsureCreatedAsync();
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Custumer");

            var user = await _userHelper.GetUserByEmailAsync("romerojosemiguelbello@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Jose",
                    LastName = "Miguel",
                    Email = "romerojosemiguelbello@gmail.com",
                    UserName = "romerojosemiguelbello@gmail.com",
                    PhoneNumber = "936232131"
                };
                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user  in seeder");
                }
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Products.Any())
            {
                AddProduct("Iphone X", user);
                AddProduct("Magic Mause", user);
                AddProduct("Iwatch", user);
                AddProduct("Ipad mini", user);
                await _context.SaveChangesAsync();
            }

        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
