using AdminDashboard.Models;
using AutoMapper;
using Talabat.Core.Entities;

namespace AdminDashboard.Helppers
{
    public class MappsProfile : Profile
    {
        public MappsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
