using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class Flavours
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Products> products { get; set; }

    }
}
