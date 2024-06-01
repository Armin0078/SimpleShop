using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;

namespace MyFirstShop.Controllers
{
	public class ProductController : Controller
	{
		private MyFirstShopContext _context;

        public ProductController(MyFirstShopContext context)
        {
			_context = context;

		}

		[Route("Group/{id}/{name}")]
        public IActionResult ShowProductByGroupid(int id , string name)
		{
			ViewData["GroupName"] = name;

			var products = _context.CategoryToProducts
				.Where(c=> c.CategoryId == id)
				.Include(d => d.Product)
				.Select(f => f.Product).ToList();

			return View(products);
		}
	}
}
