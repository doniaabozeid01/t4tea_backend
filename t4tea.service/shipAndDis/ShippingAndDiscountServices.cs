using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.Review.Dtos;
using t4tea.service.shipAndDis.Dtos;

namespace t4tea.service.shipAndDis
{
    public class ShippingAndDiscountServices : IShippingAndDiscountServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingAndDiscountServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetShippingAndDiscountDto> AddShippingAndDiscount(AddShippingAndDiscountDto shippingDto)
        {
            if (shippingDto != null)
            {
                var mappedShip = _mapper.Map<ShippingAndDiscount>(shippingDto);
                await _unitOfWork.Repository<ShippingAndDiscount>().AddAsync(mappedShip);
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                return new GetShippingAndDiscountDto
                {
                    Id = mappedShip.Id,
                    ShippingPrice = shippingDto.ShippingPrice,
                    OverAllDiscount = shippingDto.OverAllDiscount,
                };
            }
            return null;
        }

        public async Task<int> DeleteShippingAndDiscount(int id)
        {
            var ship = await _unitOfWork.Repository<ShippingAndDiscount>().GetByIdAsync(id);
            if (ship != null)
            {
                _unitOfWork.Repository<ShippingAndDiscount>().Delete(ship);
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

        public async Task<IReadOnlyList<GetShippingAndDiscountDto>> GetAllShippingAndDiscount()
        {
            var ships = await _unitOfWork.Repository<ShippingAndDiscount>().GetAllAsync();
            if (ships.Any()) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedShips = _mapper.Map<IReadOnlyList<GetShippingAndDiscountDto>>(ships);
                return mappedShips;
            }
            return null;
        }

        public async Task<GetShippingAndDiscountDto> GetShippingAndDiscountById(int id)
        {
            var ship = await _unitOfWork.Repository<ShippingAndDiscount>().GetByIdAsync(id);
            if (ship != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedShip = _mapper.Map<GetShippingAndDiscountDto>(ship);
                return mappedShip;
            }
            return null;
        }

        public async Task<GetShippingAndDiscountDto> UpdateShippingAndDiscount(int id, AddShippingAndDiscountDto shippingDto)
        {
            var ship = await _unitOfWork.Repository<ShippingAndDiscount>().GetByIdAsync(id);
            if (ship != null) // يعني فيه عالاقل تقييم واحد او بمعني تاني فيه تقييمات 
            {
                var mappedShip = _mapper.Map<AddShippingAndDiscountDto>(shippingDto);

                ship.ShippingPrice = mappedShip.ShippingPrice;
                ship.OverAllDiscount = mappedShip.OverAllDiscount;


                _unitOfWork.Repository<ShippingAndDiscount>().Update(ship);
                var status = await _unitOfWork.CompleteAsync();
                if (status == 0)
                {
                    return null;
                }
                return new GetShippingAndDiscountDto
                {
                    Id = id,
                    ShippingPrice = mappedShip.ShippingPrice,
                    OverAllDiscount = mappedShip.OverAllDiscount,
                };
            }
            return null;
        }
    }
}
