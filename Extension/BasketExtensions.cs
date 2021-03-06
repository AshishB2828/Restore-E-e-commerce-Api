using Microsoft.EntityFrameworkCore;
using ReactShope.DTOs;
using ReactShope.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Extension
{
    public static class BasketExtensions
    {
        public static BasketDto MapBasketToDto(this Basket basket)
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
                                  Quantity = i.Quantity,
                                   
                              })
                              .ToList(),
                ClientSecret = basket.ClientSecret,
                PaymentIntentId = basket.PaymentIntentId
            };
        }


        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
        {

            return query.Include(x => x.Items)
                          .ThenInclude(p => p.Product)
                          .Where(b => b.BuyerId == buyerId);
        }
    }
}
