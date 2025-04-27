using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.VisualBasic;
using Services_Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            if (product is null)
                return null;
            var result = _mapper.Map<ProductResultDto>(product);
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrands()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductAsync()
        {
            // Get All Products Throught ProductRepository
            // Mapping From IEnumerable<Product> to IEnumerable<ProductResultDto> --> Using Automapper

            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync();

            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto> : Auto Mapper

            var result = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }
    }
}
