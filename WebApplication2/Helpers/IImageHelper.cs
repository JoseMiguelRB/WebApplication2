using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace WebApplication2.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
