using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace t4tea.data.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }
        [JsonIgnore]
        public List<FavouriteProducts> favourites { get; set; }

    }
}
