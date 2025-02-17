using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;

namespace t4tea.service.favourite.Dtos
{
    public class AddFavouriteDto
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}
