using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Enum;

namespace t4tea.service.product.Dtos
{
    public class addProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; } = 100;

        public float? Rate { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Discount { get; set; }
        //public decimal NewPrice { get; set; }
        public int flavourId { get; set; }
        public int categoryId { get; set; }
    }
}
