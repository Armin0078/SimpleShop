using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Models;

namespace MyFirstShop.Pages.Admin
{
    public class IndexModel : PageModel
    {
       private MyFirstShopContext _context;

        public IEnumerable<Product> Products { get; set; }

        public IndexModel(MyFirstShopContext context)
        {
            _context = context;       
        }
        public void OnGet()
        {
			Products = _context.Products.Include(c=>c.Item);

		}

		public void OnPost()
		{

		}
	}
}
