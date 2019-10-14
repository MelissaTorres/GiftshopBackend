using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.GiftShop.Application.Examples.Models;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Application.Services;

namespace SS.GiftShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<CategoryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetCategoryPageQuery query)
        {
            var result = await _categoryService.GetPage(query);

            return Ok(result);
        }

        [HttpGet("{categoryId:guid}")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid categoryId)
        {
            var result = await _categoryService.Get(categoryId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddCategoryModel model)
        {
            await _categoryService.Add(model);
            return Ok();
        }
    }
}
