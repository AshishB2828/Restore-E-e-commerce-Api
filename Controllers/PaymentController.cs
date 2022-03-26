using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactShope.Data;
using ReactShope.DTOs;
using ReactShope.Extension;
using ReactShope.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServices _paymentServices;
        private readonly StoreContext _context;

        public PaymentController(PaymentServices paymentServices, StoreContext context)
        {
            _paymentServices = paymentServices;
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrPaymentUpdate()
        {
            var basket = await _context.Baskets
                    .RetrieveBasketWithItems(User.Identity.Name)
                    .FirstOrDefaultAsync();

            if (basket == null) return NotFound();

            var intent = await _paymentServices.createOrUpdatePaymentIntent(basket);

            if (intent == null) 
                return BadRequest(new ProblemDetails { Title = "Problem with Creating Intent" });
            basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
            basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

            _context.Update(basket);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
                return BadRequest(new ProblemDetails { Title = "Problem Updating Basket Intent" });
            return basket.MapBasketToDto();
        }
    }
}
