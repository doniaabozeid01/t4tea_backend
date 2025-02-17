using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class Benifits :BaseEntity
    {
        public int Id { get; set; }
        public string description { get; set; }
        public int productId { get; set; }
        [JsonIgnore]
        public Products Product { get; set; }

    }
}
