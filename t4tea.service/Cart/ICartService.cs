using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.Cart.Dtos;

namespace t4tea.service.Cart
{
    public interface ICartService
    {
        Task<GetCart> GetCartById(int id);
        Task<IReadOnlyList<GetCart>> GetCartByUserId(string id);
        Task<GetCart> GetCartByIdWithoutInclude(int id);
        Task<IReadOnlyList<GetCart>> GetAllCarts();
        Task<AddCart> AddCart(AddCart cartDto);
        Task<GetCart> UpdateCart(int id, AddCart cartDto);
        //Task DeleteTeaAfterDeleteItsImages(int id); // بتمسح صور اللشاي صاحب ال id المبعوت
        Task<int> DeleteCart(int id); // بتمسح صوره بال id بتاعها
    }
}
