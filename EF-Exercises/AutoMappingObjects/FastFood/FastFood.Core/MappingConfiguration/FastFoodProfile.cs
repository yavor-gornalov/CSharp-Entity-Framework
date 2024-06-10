namespace FastFood.Core.MappingConfiguration
{
	using AutoMapper;
	using FastFood.Core.ViewModels.Categories;
	using FastFood.Core.ViewModels.Employees;
	using FastFood.Core.ViewModels.Items;
	using FastFood.Core.ViewModels.Orders;
	using FastFood.Models;
	using ViewModels.Positions;

	public class FastFoodProfile : Profile
	{
		public FastFoodProfile()
		{
			//Positions
			CreateMap<CreatePositionInputModel, Position>()
				.ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

			CreateMap<Position, PositionsAllViewModel>()
				.ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

			//Categories
			CreateMap<CreateCategoryInputModel, Category>()
				.ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName));

			CreateMap<Category, CategoryAllViewModel>();

			CreateMap<Category, CreateItemViewModel>()
				.ForMember(x => x.CategoryId, y => y.MapFrom(s => s.Id))
				.ForMember(x => x.CategoryName, y => y.MapFrom(s => s.Name));


			//EmployeesPositions
			CreateMap<Position, RegisterEmployeeViewModel>()
				.ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id))
				.ForMember(x => x.PositionName, y => y.MapFrom(s => s.Name));

			//Employees
			CreateMap<RegisterEmployeeInputModel, Employee>();

			CreateMap<Employee, RegisterEmployeeViewModel>();

			CreateMap<Employee, EmployeesAllViewModel>()
				.ForMember(x => x.Position, y => y.MapFrom(s => s.Position.Name));


			//Items
			CreateMap<Category, CreateItemViewModel>()
				.ForMember(x => x.CategoryId, y => y.MapFrom(s => s.Id))
				.ForMember(x => x.CategoryName, y => y.MapFrom(s => s.Name));

			CreateMap<CreateItemInputModel, Item>();

			CreateMap<Item, ItemsAllViewModels>()
				.ForMember(x => x.Category, y => y.MapFrom(s => s.Category.Name));

			//Orders
			CreateMap<Order, OrderAllViewModel>()
				.ForMember(x => x.OrderId, y => y.MapFrom(s => s.Id))
				.ForMember(x => x.Employee, y => y.MapFrom(s => s.Employee.Name));
		}
	}
}
