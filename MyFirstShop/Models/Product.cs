namespace MyFirstShop.Models
{
    public class Product
    {
        //public Product()
        //{
        //    Categoties = new List<Category>();
        //}

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ItemId { get; set; }

        //public List<Category> Categoties { get; set; }

        public ICollection<CategoryToProduct> CategoryToProduct { get; set; }

        public Item Item { get; set; }

        public List<OrderDetial> orderDetials { get; set; }
    }
}
