using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstShop.Data;
using MyFirstShop.Models;

namespace MyFirstShop.Pages.Admin
{
	public class AddModel : PageModel
	{
		private MyFirstShopContext _context;

		public AddModel(MyFirstShopContext context)
		{
			_context = context;
		}

		[BindProperty]
		public AddEditProductViewModel Product { get; set; }
		[BindProperty]
		public List<int> selectedGroups { get; set; }
        public void OnGet()
		{
			Product = new AddEditProductViewModel()
			{
				categories = _context.Categories.ToList()
			};

        }

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
				return Page();

			var item = new Item()
			{
				price = Product.Price,
				quantityInStock = Product.QuantityInStock
			};
			_context.Add(item);
			_context.SaveChanges();

			var pro = new Product()
			{
				Name = Product.Name,
				Item = item,
				Description = Product.Description,
				
			};
			_context.Add(pro);
			_context.SaveChanges();

			pro.ItemId = pro.Id;
			_context.SaveChanges();

			if(Product.Picture?.Length> 0)
			{
				string filePath = Path.Combine(Directory.GetCurrentDirectory(),
					"wwwroot",
					"images",
					pro.Id+Path.GetExtension(Product.Picture.FileName));
					
				using (var stream = new FileStream(filePath,FileMode.Create))
				{
					Product.Picture.CopyTo(stream);

				}
			}

			if(selectedGroups.Any() && selectedGroups.Count > 0)
			{
                foreach (var gr in selectedGroups)
                {
					_context.CategoryToProducts.Add(new CategoryToProduct()
					{
						CategoryId = gr,
						ProductId = pro.Id
					});
                }
				_context.SaveChanges();
            }

			return RedirectToPage("Index");
		}
	}
}
