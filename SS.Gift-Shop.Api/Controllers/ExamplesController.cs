using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.GiftShop.Application.Examples.Models;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Application.Services;

namespace SS.GiftShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController : ControllerBase
    {
        private readonly IExampleService _exampleService;

        public ExamplesController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ExampleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetExamplePageQuery query)
        {
            var user = User.Identity.Name;
            var result = await _exampleService.GetPage(query);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _exampleService.Get(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddOrderDetail model)
        {
            await _exampleService.Add(model);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, AddOrderDetail model)
        {
            //await _exampleService.Update(id, model);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // await _exampleService.Delete(id);
            return Ok();
        }
    }
}
