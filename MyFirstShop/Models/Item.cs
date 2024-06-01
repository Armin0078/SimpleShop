namespace MyFirstShop.Models
{
    public class Item
    {
        public int Id { get; set; } 
        
        public decimal price { get; set; }

         public int quantityInStock { get; set;}

		public Product product { get; set; }
	}
}
