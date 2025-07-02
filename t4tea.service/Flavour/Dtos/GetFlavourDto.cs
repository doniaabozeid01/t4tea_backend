using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;

namespace t4tea.service.Flavour.Dtos
{
    public class GetFlavourDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Products> products { get; set; }
    }
}
