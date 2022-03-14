using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactShope.Data;
using ReactShope.Entity;
using ReactShope.Extension;
using ReactShope.RequestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReactShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly UserManager<User> _userManager;

        public ProductController(StoreContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts([FromQuery]ProductParams param)
        {
            var query = _context.Products
                   .Sort(param.OrderBy)
                   .Search(param.SearchTerm)
                   .Filter(param.Brands, param.Types)
                  .AsQueryable();
            var products = await PagedList<Product>
                                .ToPagedList(query, param.PageNumber,param.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            //var user = new User
            //{
            //    UserName = "Ashish",
            //    Email = "user1@user.com"
            //};

            //await _userManager.CreateAsync(user, "User@123");
            //await _userManager.AddToRoleAsync(user, "Member");

            //user.UserName = "Unni";
            //user.Email = "admin2@admin2.com";

            //await _userManager.CreateAsync(user, "Admin2@123");
            //await _userManager.AddToRolesAsync(user, new[] { "Admin", "Member" });


            return products;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return Ok(product);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }
    }
}
