using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Application.Services;

namespace SS.GiftShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ProductModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetProductPageQuery query)
        {
            var result = await _productService.GetPage(query);

            return Ok(result);
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid productId)
        {
            var result = await _productService.Get(productId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddProductModel model)
        {
            await _productService.Add(model);
            return Ok();
        }

        [HttpPut("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid productId, UpdateProductModel model)
        {
            await _productService.Update(productId, model);
            return Ok();
        }

        [HttpDelete("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid productId)
        {
            await _productService.Delete(productId);
            return Ok();
        }
    }
}
