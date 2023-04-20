using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.ViewModels;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _context;
        private readonly IConfiguration _config;
        public CatalogController(CatalogContext context, IConfiguration config) 
        { 
            _context = context;
            _config = config;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogTypes() 
        {
            var types = await _context.CatalogTypes.ToListAsync();
            return Ok(types);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogBrands() 
        { 
            var brands = await _context.CatalogBrands.ToListAsync();
            return Ok(brands);
        }

        // Taking the format of [action]/{pageIndex}/{pageSize} due to the order of parameters.
        [HttpGet("[action]")]
        // Since you are passing in a parameter from an API, use [FromQuery] which means a ? in a url.
        public async Task<IActionResult> Items([FromQuery] int pageIndex = 0,
                                               [FromQuery] int pageSize = 6) 
        {
            var itemsCount = _context.Catalog.LongCountAsync();
            var items = await _context.Catalog
                // Ascending order of the catalog Name
                .OrderBy(c => c.Name)
                // Skipping to the next page. pageIndex will start at 0, so this will start with none being skipped.
                .Skip(pageIndex * pageSize)
                // Take the remaining elements within the amount set in your pageSize.
                .Take(pageSize)
                // Convert those elemennts into a list.
                .ToListAsync();
            // Change the default picture urls in your table with a method that takes in and manipulates items.
            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Data = items,
                Count = itemsCount.Result
            };

            return Ok(model);
        }

        [HttpGet("[action]/filter")]
        // Since you are passing in a parameter from an API, use [FromQuery] which means a ? in a url.
        public async Task<IActionResult> Items([FromQuery] int? catalogTypeId,
                                               [FromQuery] int? catalogBrandId,
                                               [FromQuery] int pageIndex = 0,
                                               [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<CatalogItem>) _context.Catalog;
            if (catalogTypeId.HasValue) 
            {
                // Filter by CatalogTypeId, such as "Wedding Rings" or Type 1.
                query = query.Where(c => c.CatalogTypeId == catalogTypeId.Value);
            }
            if (catalogBrandId.HasValue)
            {
                // Filter by CatalogTypeId, such as "Wedding Rings" or Type 1.
                query = query.Where(c => c.CatalogBrandId == catalogBrandId.Value);
            }

            var itemsCount = _context.Catalog.LongCountAsync();
            var items = await _context.Catalog
                // Ascending order of the catalog Name
                .OrderBy(c => c.Name)
                // Skipping to the next page. pageIndex will start at 0, so this will start with none being skipped.
                .Skip(pageIndex * pageSize)
                // Take the remaining elements within the amount set in your pageSize.
                .Take(pageSize)
                // Convert those elemennts into a list.
                .ToListAsync();
            // Change the default picture urls in your table with a method that takes in and manipulates items.
            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Data = items,
                Count = itemsCount.Result
            };

            return Ok(model);
        }

        private List<CatalogItem> ChangePictureUrl(List<CatalogItem> items)
        {
            items.ForEach(item => item.PictureUrl = item.PictureUrl
                .Replace("http://externalcatalogbaseurltobereplaced", 
                _config["ExternalBaseUrl"]));
            return items;
        }
    }
}
