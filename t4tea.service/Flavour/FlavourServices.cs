using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.Flavour.Dtos;
using t4tea.service.product.Dtos;
using t4tea.service.Review.Dtos;

namespace t4tea.service.Flavour
{
    public class FlavourServices : IFlavourServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlavourServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetFlavourDto> AddFlavour(addFlavourDto flavourDto)
        {
            if (flavourDto != null)
            {
                var mappedFlavour = _mapper.Map<Flavours>(flavourDto);
                await _unitOfWork.Repository<Flavours>().AddAsync(mappedFlavour);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                return new GetFlavourDto
                {
                    Id = mappedFlavour.Id,
                    Name = mappedFlavour.Name,
                };
            }
            return null;
        }






        public async Task<int> DeleteFlavour (int id)
        {
            var flavour = await _unitOfWork.Repository<Flavours>().GetByIdAsync(id);
            if (flavour != null)
            {
                _unitOfWork.Repository<Flavours>().Delete(flavour);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return 0;
                }
                else
                {
                    return status;
                }
            }
            return 0;
        }





        public async Task<IReadOnlyList<GetFlavourDto>> GetAllFlavours()
        {
            var flavours = await _unitOfWork.Repository<Flavours>().GetAllAsync();
            if (flavours.Any()) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedFlavours = _mapper.Map<IReadOnlyList<GetFlavourDto>>(flavours);
                return mappedFlavours;
            }
            return null;
        }





        public async Task<GetFlavourDto> GetFlavourById(int id)
        {
            var flavour = await _unitOfWork.Repository<Flavours>().GetByIdAsync(id);
            if (flavour != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedFlavour = _mapper.Map<GetFlavourDto>(flavour);
                return mappedFlavour;
            }
            return null;
        }





        public async Task<GetFlavourDto> UpdateFlavour(int id, addFlavourDto flavourDto)
        {
            var flavour = await _unitOfWork.Repository<Flavours>().GetByIdAsync(id);
            if (flavour != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedFlavour = _mapper.Map<addFlavourDto>(flavourDto);

                flavour.Name = mappedFlavour.Name;



                _unitOfWork.Repository<Flavours>().Update(flavour);
                var status = await _unitOfWork.CompleteAsync();
                if (status == 0)
                {
                    return null;
                }
                return new GetFlavourDto
                {
                    Id = id,
                    Name = flavour.Name,
                };
            }
            return null;
        }





    }
}
