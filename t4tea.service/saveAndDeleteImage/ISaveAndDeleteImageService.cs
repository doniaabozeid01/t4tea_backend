using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace t4tea.service.saveAndDeleteImage
{
    public interface ISaveAndDeleteImageService
    {
        Task<string> UploadToCloudinary(IFormFile file);
        bool DeleteFromCloudinary(string publicId);
        string ExtractPublicIdFromUrl(string imageUrl);
    }
}
