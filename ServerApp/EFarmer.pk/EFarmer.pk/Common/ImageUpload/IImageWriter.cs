using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ImageWriter
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file);
    }
}
