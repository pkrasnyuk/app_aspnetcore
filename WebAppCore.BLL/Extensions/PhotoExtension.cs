using System.IO;
using Microsoft.AspNetCore.Http;
using WebAppCore.BLL.Models;

namespace WebAppCore.BLL.Extensions
{
    public static class PhotoExtension
    {
        public static FileModel UploadFile(this FileModel model, IFormFile file)
        {
            if (file != null)
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    model.Source = stream.ToArray();
                    model.FileName = file.FileName;
                }

            return model;
        }
    }
}