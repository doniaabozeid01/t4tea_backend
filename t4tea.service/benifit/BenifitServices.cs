using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.benifit.Dtos;
using t4tea.service.product.Dtos;

namespace t4tea.service.benifit
{
    public class BenifitServices :IBenifitServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BenifitServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<BenifitDto> AddProductBenifits(AddBenifits benifitDto)
        {

            var benifit = _mapper.Map<Benifits>(benifitDto);

            await _unitOfWork.Repository<Benifits>().AddAsync(benifit);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new BenifitDto
            {
                Id  = benifit.Id,
                description = benifitDto.description,
                productId = benifitDto.productId,
            };
        }


        public async Task<BenifitDto> UpdateProductBenifits(int id, AddBenifits benifitDto)
        {
            var benifit = await _unitOfWork.Repository<Benifits>().GetByIdAsync(id);

            if (benifit == null)
            {
                return null;
            }

            benifit.description = benifitDto.description;
            benifit.productId = benifitDto.productId;

            _unitOfWork.Repository<Benifits>().Update(benifit);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new BenifitDto
            {
                Id = id,
                description = benifitDto.description,
                productId = benifitDto.productId,
            };
        }




        public async Task<IReadOnlyList<BenifitDto>> GetAllProductBenifits()
        {
            var benifits = await _unitOfWork.Repository<Benifits>().GetAllAsync();
            var mappedBenifits = _mapper.Map<IReadOnlyList<BenifitDto>>(benifits);
            return mappedBenifits;
        }

        public async Task<BenifitDto> GetProductBenifitsById(int id)
        {
            var benifit = await _unitOfWork.Repository<Benifits>().GetByIdAsync(id);
            var mappedBenifit = _mapper.Map<BenifitDto>(benifit);
            return mappedBenifit;
        }

        public async Task<int> DeleteProductBenifits(int id)
        {
            var benifit = await _unitOfWork.Repository<Benifits>().GetByIdAsync(id);

            if (benifit == null)
            {
                return 0;
            }

            _unitOfWork.Repository<Benifits>().Delete(benifit);
            return await _unitOfWork.CompleteAsync();
        }



    }
}
