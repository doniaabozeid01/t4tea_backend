using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.service.Order.Dtos
{
    public class UpdateOrderStatusRequest
    {
        public data.Enum.OrderStatus Status { get; set; } // الحالة الجديدة

    }
}
