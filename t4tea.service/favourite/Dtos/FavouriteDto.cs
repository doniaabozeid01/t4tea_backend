using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;

namespace t4tea.service.favourite.Dtos
{
    public class FavouriteDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
