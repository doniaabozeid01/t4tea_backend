using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.service.shipAndDis.Dtos
{
    public class AddShippingAndDiscountDto
    {
        public decimal OverAllDiscount { get; set; }
        public string? ShippingPrice { get; set; }
    }
}
