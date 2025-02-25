using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class ShippingAndDiscount : BaseEntity
    {
        public int Id { get; set; }
        public decimal OverAllDiscount { get; set; } = decimal.Zero;
        public string? ShippingPrice { get; set; } = string.Empty;
    }
}
