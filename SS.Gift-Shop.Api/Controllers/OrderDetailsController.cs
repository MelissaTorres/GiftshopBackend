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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<OrderDetailModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetOrderDetailPageQuery query)
        {
            var result = await _orderDetailService.GetPage(query);

            return Ok(result);
        }

        //[HttpGet("{orderId:int;productId:guid}")]
        [HttpGet("{orderId:guid}")]
        [ProducesResponseType(typeof(OrderDetailModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid orderId)
        {
            var result = await _orderDetailService.Get(orderId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddOrderDetailModel model)
        {
            await _orderDetailService.Add(model);
            return Ok();
        }
    }
}
