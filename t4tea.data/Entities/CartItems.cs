using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class CartItems:BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public string? OrderRequestId { get; set; }
        public OrderRequest? OrderRequest { get; set; } // علاقة مع الكائن OrderRequest

        public int Quantity { get; set; }
    }
}
