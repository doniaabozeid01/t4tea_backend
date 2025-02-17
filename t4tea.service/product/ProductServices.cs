using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using t4tea.data.Entities;
using t4tea.data.Enum;
using t4tea.repository.Interfaces;
using t4tea.service.category.Dtos;
using t4tea.service.product.Dtos;

namespace t4tea.service.product
{
    public class ProductServices : IProductServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ProductDto> AddProduct(addProductDto productDto)
        {

            var product = _mapper.Map<Products>(productDto);

            await _unitOfWork.Repository<Products>().AddAsync(product);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Rate = productDto.Rate,
                OldPrice = productDto.OldPrice,
                Discount = productDto.Discount,
                //NewPrice = productDto.NewPrice,
                Flavour = productDto.Flavour,
                categoryId = productDto.categoryId
            };
        }


        public async Task<ProductDto> UpdateProduct(int id, addProductDto prodDto)
        {
            var product = await _unitOfWork.Repository<Products>().GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            product.Name = prodDto.Name;
            product.Description = prodDto.Description;
            product.Rate = prodDto.Rate;
            product.OldPrice = prodDto.OldPrice;
            product.Discount = prodDto.Discount;
            //product.NewPrice = prodDto.NewPrice;
            product.Flavour = prodDto.Flavour;
            product.categoryId = prodDto.categoryId;

            _unitOfWork.Repository<Products>().Update(product);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new ProductDto
            {
                Id = id,
                Name = prodDto.Name,
                Description = prodDto.Description,
                Rate = prodDto.Rate,
                OldPrice = prodDto.OldPrice,
                Discount = prodDto.Discount,
                //NewPrice = prodDto.NewPrice,
                Flavour = prodDto.Flavour,
                categoryId = prodDto.categoryId
            };
        }




        public async Task<IReadOnlyList<ProductDto>> GetAllProducts()
        {
            var products = await _unitOfWork.Repository<Products>().GetAllProductsAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return mappedProducts;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _unitOfWork.Repository<Products>().GetProductByIdAsync(id);
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }

        public async Task<int> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Products>().GetByIdAsync(id);

            if (product == null)
            {
                return 0;
            }

            _unitOfWork.Repository<Products>().Delete(product);
            return await _unitOfWork.CompleteAsync();
        }





        //public async Task<IReadOnlyList<getOffers>> GetAllOffers(int type)
        //{
        //    var offers = await _unitOfWork.Repository<Offer>().GetAllOffersBasedOnType(type);
        //    var mappedOffers = _mapper.Map<IReadOnlyList<getOffers>>(offers);
        //    return mappedOffers;
        //}




        public async Task<IReadOnlyList<ProductDto>> GetAllProductWithOriginalOffer()
        {
            var offers = await _unitOfWork.Repository<Products>().GetAllProductWithOriginalOffer();
            var mappedOffers = _mapper.Map<IReadOnlyList<ProductDto>>(offers);
            return mappedOffers;
        }



        public async Task<IReadOnlyList<ProductDto>> GetAllProductWithVIPOffer()
        {
            var offers = await _unitOfWork.Repository<Products>().GetAllProductWithVIPOffer();
            var mappedOffers = _mapper.Map<IReadOnlyList<ProductDto>>(offers);
            return mappedOffers;
        }










        public async Task<ProductDto> changeDiscount(int id, decimal discount)
        {
            var product = await _unitOfWork.Repository<Products>().GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            
            product.Discount = discount;

            _unitOfWork.Repository<Products>().Update(product);
            var status = await _unitOfWork.CompleteAsync();

            if (status == 0)
            {
                return null;
            }

            return new ProductDto
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Rate = product.Rate,
                OldPrice = product.OldPrice,
                Discount = discount,
                Flavour = product.Flavour,
                categoryId = product.categoryId
            };
        }


















    }
}
