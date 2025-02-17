using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace t4tea.service.advertise.Dtos
{
    public class addAdvertise
    {
        public IFormFile? ImagePath { get; set; }
    }
}
