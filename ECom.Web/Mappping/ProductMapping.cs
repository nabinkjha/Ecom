using AutoMapper;
using ECom.Web.Models;

namespace ECom.Web.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
