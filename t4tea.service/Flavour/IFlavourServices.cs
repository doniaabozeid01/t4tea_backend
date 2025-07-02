using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.Flavour.Dtos;

namespace t4tea.service.Flavour
{
    public interface IFlavourServices
    {

        Task<GetFlavourDto> UpdateFlavour(int id, addFlavourDto flavourDto);

        Task<GetFlavourDto> GetFlavourById(int id);
        Task<IReadOnlyList<GetFlavourDto>> GetAllFlavours();
        Task<int> DeleteFlavour(int id);
        Task<GetFlavourDto> AddFlavour(addFlavourDto flavourDto);

    }
}
