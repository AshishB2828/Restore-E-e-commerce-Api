using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactShope.Data;
using ReactShope.DTOs;
using ReactShope.Entity;
using ReactShope.Entity.OrderAggregate;
using ReactShope.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreContext _storeContext;

        public OrderController( StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            return await _storeContext.Orders
                    .Include(o => o.OrderItems)
                    .Where(x => x.BuyerId == User.Identity.Name)
                    .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            return await _storeContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {


            var basket = await _storeContext.Baskets.RetrieveBasketWithItems(User.Identity.Name)
                        .FirstOrDefaultAsync();

            if (basket == null)
                return BadRequest(new ProblemDetails { Title = "Cloud not locate basket" });
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _storeContext.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrderd
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrderd = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;

            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee
            };

            _storeContext.Orders.Add(order);
            _storeContext.Baskets.Remove(basket);

            if(orderDto.SaveAddress)
            {
                var user = await _storeContext.Users
                        .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                user.Address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    Country = orderDto.ShippingAddress.Country,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                };
                _storeContext.Update(user);
            }

            var result = await _storeContext.SaveChangesAsync() > 0;

            if (result) 
                return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
            return BadRequest("Problem Creating Order");
        }
    }
}
