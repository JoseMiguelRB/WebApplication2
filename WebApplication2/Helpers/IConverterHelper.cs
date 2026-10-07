using System;
using WebApplication2.Data.Entities;
using WebApplication2.Models;

namespace WebApplication2.Web.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, string path, bool isNew);
        ProductViewModel ToProductViewModel(Product product);
    }
}
