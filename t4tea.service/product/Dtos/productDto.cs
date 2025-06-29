using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;
using t4tea.data.Enum;

namespace t4tea.service.product.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? Rate { get; set; }
        public float Weight { get; set; } 
        public decimal OldPrice { get; set; }
        public decimal Discount { get; set; }
        //public decimal NewPrice { get; set; } = (OldPrice - (oldPrice))
        public int flavourId { get; set; }
        public FlavourDto Flavour { get; set; }
        public int categoryId { get; set; }
        public CategoryDto Category { get; set; }
        public List<Images> images { get; set; }
        public List<Benifits> benifits { get; set; }
        public List<Reviews> reviews { get; set; }
    }
}
