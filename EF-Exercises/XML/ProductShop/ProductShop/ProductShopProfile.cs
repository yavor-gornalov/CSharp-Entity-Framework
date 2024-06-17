using AutoMapper;
using ProductShop.DTOs.Export;
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
			CreateMap<Product, ExportProductDTO>()
				.ForMember(x => x.BuyerFullName, y => y.MapFrom(s => s.Buyer.FirstName + ' ' + s.Buyer.LastName));
			CreateMap<User, ExportUserDTO>();
		}
	}
}
