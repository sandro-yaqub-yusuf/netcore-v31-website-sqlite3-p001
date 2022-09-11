using AutoMapper;
using KITAB.Products.Domain.Models;
using KITAB.Products.Web.ViewModels;

namespace KITAB.Products.Web.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
