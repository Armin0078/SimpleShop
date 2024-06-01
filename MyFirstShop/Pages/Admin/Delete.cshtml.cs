using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstShop.Data;
using MyFirstShop.Models;

namespace MyFirstShop.Pages.Admin
{
	public class DeleteModel : PageModel
	{
		private MyFirstShopContext _context;

		public DeleteModel(MyFirstShopContext context)
		{
			_context = context;
		}
		[BindProperty]
		public Product Product { get; set; }
		public void OnGet(int id)
		{
			Product = _context.Products.FirstOrDefault(p => p.Id == id);
		}


		public IActionResult OnPost()
		{
			var item = _context.Items.First(p => p.Id == Product.Id);
			var product = _context.Products.Find(Product.Id);

			_context.Items.Remove(item);
			_context.Products.Remove(product);
			_context.SaveChanges();


			string filePath = Path.Combine(Directory.GetCurrentDirectory(),
					"wwwroot",
					"images",
					product.Id + ".jpg");

			string filePath2 = Path.Combine(Directory.GetCurrentDirectory(),
					"wwwroot",
					"images",
					product.Id + ".png");

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
			if (System.IO.File.Exists(filePath2))
			{
				System.IO.File.Delete(filePath2);
			}


			return RedirectToAction("Index");
		}
	}
}
