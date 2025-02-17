using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.service.benifit.Dtos;

namespace t4tea.service.benifit
{
    public interface IBenifitServices
    {
        Task<int> DeleteProductBenifits(int id);
        Task<BenifitDto> GetProductBenifitsById(int id);
        Task<IReadOnlyList<BenifitDto>> GetAllProductBenifits();
        Task<BenifitDto> UpdateProductBenifits(int id, AddBenifits benifitDto);
        Task<BenifitDto> AddProductBenifits(AddBenifits benifitDto);

    }
}
