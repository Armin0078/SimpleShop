using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Models;

namespace MyFirstShop.Pages.Admin
{

    public class EditModel : PageModel
    {
        private MyFirstShopContext _context;

        public EditModel(MyFirstShopContext context)
        {
            _context = context;
        }
        [BindProperty]
        public AddEditProductViewModel Product { get; set; }

		[BindProperty]
		public List<int> selectedGroups { get; set; }

        public List<int> GroupsProduct { get; set; }
        public void OnGet(int id)
        {
            var product = _context.Products.Include(p => p.Item)
                .Where(c => c.Id == id)
                .Select(s => new AddEditProductViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    QuantityInStock = s.Item.quantityInStock,
                    Price = s.Item.price
                }).FirstOrDefault();


            Product = product;

			product.categories = _context.Categories.ToList();

            GroupsProduct = _context.CategoryToProducts.Where(c => c.ProductId == id)
                .Select(s => s.CategoryId).ToList();       
		} 
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var product = _context.Products.Find(Product.Id);
            var item = _context.Items.First(p=>p.Id==product.ItemId);

            product.Name = Product.Name;
            product.Description = Product.Description;
            item.price = Product.Price;
            item.quantityInStock = Product.QuantityInStock;

            _context.SaveChanges();

			if (Product.Picture?.Length > 0)
			{
				string filePath = Path.Combine(Directory.GetCurrentDirectory(),
					"wwwroot",
					"images",
					product.Id + Path.GetExtension(Product.Picture.FileName));

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					Product.Picture.CopyTo(stream);

				}
			}

            _context.CategoryToProducts.Where(c => c.ProductId == product.Id).ToList()
                .ForEach(g => _context.CategoryToProducts.Remove(g));

			if (selectedGroups.Any() && selectedGroups.Count > 0)
			{
				foreach (var gr in selectedGroups)
				{
					_context.CategoryToProducts.Add(new CategoryToProduct()
					{
						CategoryId = gr,
						ProductId = Product.Id
					});
				}
				_context.SaveChanges();
			}



			return RedirectToPage("Index");
        }
    }
}
