using Microsoft.AspNetCore.Http;
using t4tea.service.advertise.Dtos;

namespace t4tea.service.advertise
{
    public interface IAdvertiseServices
    {
        Task<int> DeleteAdvertise(int id);
        Task<AdvertiseDto> GetAdvertiseById(int id);
        Task<IReadOnlyList<AdvertiseDto>> GetAllAdvertises();
        Task<AdvertiseDto> UpdateAdvertise(int id, addAdvertise advertiseDto);
        void DeleteImage(string imagePath);
        Task<string> SaveImage(IFormFile image);
        Task<AdvertiseDto> AddAdvertise(addAdvertise advertiseDto);

    }
}
