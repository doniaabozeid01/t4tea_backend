using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class Categories : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; } // رابط الصورة
        public IEnumerable<Products> Products { get; set; } = new List<Products>();
    }
}
