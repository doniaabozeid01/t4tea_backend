using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;

namespace t4tea.service.Review.Dtos
{
    public class GetReviewDto
    {
        public int Id { get; set; }
        public float? Rating { get; set; } = 5;
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}
