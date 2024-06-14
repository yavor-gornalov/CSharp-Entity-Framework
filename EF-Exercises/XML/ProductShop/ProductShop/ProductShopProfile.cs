using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
	public class ProductShopProfile : Profile
	{
		public ProductShopProfile()
		{
			CreateMap<ImportUserDTO, User>();
			CreateMap<ImportProductDTO, Product>();
			CreateMap<ImportCategoryDTO, Category>();
			CreateMap<ImportCategoryProductDTO, CategoryProduct>();
		}
	}
}
