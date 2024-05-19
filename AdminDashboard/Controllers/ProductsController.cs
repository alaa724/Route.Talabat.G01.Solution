using AdminDashboard.Helppers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;

namespace AdminDashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IMapper _mapper;

        public ProductsController(
            IUniteOfWork uniteOfWork,
            IMapper mapper
            )
        {
            _uniteOfWork = uniteOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // Get All Products
            var products = await _uniteOfWork.Repository<Product>().GetAllAsync();

            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

            return View(mappedProduct);
        }
		public async Task<IActionResult> Create()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
                
        //    }
        //}


    }
}
