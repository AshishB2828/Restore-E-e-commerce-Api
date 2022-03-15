using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactShope.Data;
using ReactShope.DTOs;
using ReactShope.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetriveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            return MapBasketToDto(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetriveBasket(GetBuyerId());
            if(basket == null)
            {
                basket = CreateBasket();
            }
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            basket.AddItem(product, quantity);

            var result = await _context.SaveChangesAsync() > 0;
            if(result)return CreatedAtRoute("GetBasket", MapBasketToDto(basket));
            return BadRequest(new ProblemDetails { Title = "Problem Saving Item to basket " });
        }

       

        [HttpDelete]
        public async Task<ActionResult>RemoveBasketItem(int productId, int quantity )
        {
            var basket = await RetriveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            basket.RemoveItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }

        //Rerive
        private async Task<Basket> RetriveBasket(string buyerId)
        {

            if (String.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _context.Baskets
                           .Include(i => i.Items)
                           .ThenInclude(i => i.Product)
                           .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }

        //
        private Basket CreateBasket()
        {

            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(30),
                    // HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }
            
            
            var basket = new Basket { BuyerId = buyerId };
            _context.Baskets.Add(basket);

            return basket;
        }

        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items
                              .Select(i => new BasketItemDto
                              {
                                  ProductId = i.ProductId,
                                  Name = i.Product.Name,
                                  Price = i.Product.Price,
                                  PictureUrl = i.Product.PictureUrl,
                                  Type = i.Product.Type,
                                  Brand = i.Product.Brand,
                                  Quantity = i.Quantity
                              })
                              .ToList()
            };
        }
    }
}
