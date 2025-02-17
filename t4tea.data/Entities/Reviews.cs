using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class Reviews : BaseEntity
    {
        public int Id { get; set; }
        public float? Rating { get; set; } = 5;
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Products Product { get; set; }
    }
}
