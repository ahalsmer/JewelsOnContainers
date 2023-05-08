using Microsoft.AspNetCore.Mvc;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service) 
        { 
            _service = service;
        }
        public async Task<IActionResult> Index(int? page, int? brandFilterApplied, int? typeFilterApplied)
        {
            int itemsOnPage = 10;
            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brandFilterApplied, typeFilterApplied);
            var vm = new CatalogIndexViewModel
            {
                Brands = await _service.GetBrandsAsync(),
                Types = await _service.GetTypesAsync(),
                CatalogItems = catalog.Data,
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = catalog.PageIndex,
                    TotalItems = catalog.Count,
                    ItemsPerPage = catalog.PageSize,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage),
                },
                BrandFilterApplied = brandFilterApplied,
                TypesFilterApplied = typeFilterApplied
            };
            return View(vm);
        }
    }
}
