using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.GiftShop.Application.Models;
using SS.GiftShop.Application.Queries;
using SS.GiftShop.Application.Services;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<OrderModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetOrderPageQuery query)
        {
            var result = await _orderService.GetPage(query);

            return Ok(result);
        }

        [HttpGet("{orderId:guid}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid orderId)
        {
            var result = await _orderService.Get(orderId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddOrderModel model)
        {
            var user = User.Identity.Name;
            model.UserId = user;
            await _orderService.Add(model);
            return Ok();
        }
    }
}
