using Microsoft.AspNetCore.Mvc;
using ServiceBus.Publisher.Api.Models;
using ServiceBus.Publisher.Api.Services;
using System;
using System.Threading.Tasks;

namespace ServiceBus.Publisher.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel order)
        {
            try
            {
                var response = await _orderService.CreateOrder(order);

                return Ok(response);


            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
