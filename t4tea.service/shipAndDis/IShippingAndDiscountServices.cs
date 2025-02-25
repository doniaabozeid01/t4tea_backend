using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.shipAndDis.Dtos;

namespace t4tea.service.shipAndDis
{
    public interface IShippingAndDiscountServices
    {
        Task<GetShippingAndDiscountDto> AddShippingAndDiscount(AddShippingAndDiscountDto shippingDto);
        Task<GetShippingAndDiscountDto> UpdateShippingAndDiscount(int id, AddShippingAndDiscountDto shippingDto);
        Task<int> DeleteShippingAndDiscount(int id);
        Task<IReadOnlyList<GetShippingAndDiscountDto>> GetAllShippingAndDiscount();
        Task<GetShippingAndDiscountDto> GetShippingAndDiscountById(int id);
    }
}
