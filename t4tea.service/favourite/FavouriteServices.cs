using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.benifit.Dtos;
using t4tea.service.favourite.Dtos;
using t4tea.service.product.Dtos;

namespace t4tea.service.favourite
{
    public class FavouriteServices : IFavouriteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavouriteServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<FavouriteDto> AddFavouriteProduct(AddFavouriteDto favouriteDto)
        {

            var favourite = _mapper.Map<FavouriteProducts>(favouriteDto);

            await _unitOfWork.Repository<FavouriteProducts>().AddAsync(favourite);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new FavouriteDto
            {
                Id = favourite.Id,
                ProductId = favourite.ProductId,
                UserId = favourite.UserId,
                Product = favourite.Product,
                User = favourite.User,
                AddedOn = favourite.AddedOn,
            };
        }




        //public async Task<BenifitDto> UpdateProductBenifits(int id, AddBenifits benifitDto)
        //{
        //    var benifit = await _unitOfWork.Repository<Benifits>().GetByIdAsync(id);

        //    if (benifit == null)
        //    {
        //        return null;
        //    }

        //    benifit.description = benifitDto.description;
        //    benifit.productId = benifitDto.productId;

        //    _unitOfWork.Repository<Benifits>().Update(benifit);
        //    var status = await _unitOfWork.CompleteAsync();

        //    if (status == 0)
        //    {
        //        return null;
        //    }

        //    return new BenifitDto
        //    {
        //        Id = id,
        //        description = benifitDto.description,
        //        productId = benifitDto.productId,
        //    };
        //}




        //public async Task<IReadOnlyList<BenifitDto>> GetAllProductBenifits()
        //{
        //    var benifits = await _unitOfWork.Repository<Benifits>().GetAllAsync();
        //    var mappedBenifits = _mapper.Map<IReadOnlyList<BenifitDto>>(benifits);
        //    return mappedBenifits;
        //}

        public async Task<IReadOnlyList<FavouriteDto>> GetProductFavouriteByUserId(string id)
        {
            var favourite = await _unitOfWork.Repository<FavouriteProducts>().GetProductFavouriteByUserId(id);
            var mappedFavourite = _mapper.Map<IReadOnlyList<FavouriteDto>>(favourite);
            return mappedFavourite;
        }

        public async Task<FavouriteDto> GetProductFavouriteById(int id)
        {
            var favourite = await _unitOfWork.Repository<FavouriteProducts>().GetByIdAsync(id);
            var mappedFavourite = _mapper.Map<FavouriteDto>(favourite);
            return mappedFavourite;
        }

        public async Task<int> DeleteProductFavourite(int id)
        {
            var fav = await _unitOfWork.Repository<FavouriteProducts>().GetByIdAsync(id);

            if (fav == null)
            {
                return 0;
            }

            _unitOfWork.Repository<FavouriteProducts>().Delete(fav);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
