using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.favourite.Dtos;

namespace t4tea.service.favourite
{
    public interface IFavouriteServices
    {
        Task<FavouriteDto> AddFavouriteProduct(AddFavouriteDto favouriteDto);
        Task<IReadOnlyList<FavouriteDto>> GetProductFavouriteByUserId(string id);
        Task<FavouriteDto> GetProductFavouriteById(int id);
        Task<int> DeleteProductFavourite(int id);

    }
}
