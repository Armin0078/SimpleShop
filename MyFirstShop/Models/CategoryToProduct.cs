using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyFirstShop.Models
{
	public class CategoryToProduct
	{
        public int CategoryId { get; set; }
		public int ProductId { get; set; }

        // Navigation

        public Product Product { get; set; }
        public Category Category { get; set; }


	}
}
