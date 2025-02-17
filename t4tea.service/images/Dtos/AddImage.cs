using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace t4tea.service.images.Dtos
{
    public class AddImage
    {
        public IFormFile? ImagePath { get; set; }
        public int ProductId { get; set; }
    }
}
