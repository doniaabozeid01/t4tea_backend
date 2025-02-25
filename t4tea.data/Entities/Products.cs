using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using t4tea.data.Enum;

namespace t4tea.data.Entities
{
    public class Products : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; } = 100;
        public float? Rate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewPrice { get; set; }
        public Flavour Flavour { get; set; }
        public int categoryId { get; set; }
        [JsonIgnore]
        public Categories category { get; set; }
        public List<Benifits> benifits { get; set; }
        public List<Images> images { get; set; }
        [JsonIgnore]
        public List<FavouriteProducts> favourites { get; set; }
        public List<Reviews> reviews { get; set; }



    }
}
