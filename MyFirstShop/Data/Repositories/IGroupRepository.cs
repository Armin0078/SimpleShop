using MyFirstShop.Models;

namespace MyFirstShop.Data.Repositories
{
	public interface IGroupRepository
	{
		IEnumerable<Category> GetAllCategories();

		IEnumerable<ShowGroupViewModel> GetGroupForShow();
	}

	public class GroupRepository : IGroupRepository
	{
		MyFirstShopContext _context;

        public GroupRepository(MyFirstShopContext context)
        {
			_context = context;

		}

        public IEnumerable<Category> GetAllCategories()
		{
			return _context.Categories;
		}

		public IEnumerable<ShowGroupViewModel> GetGroupForShow()
		{
			return _context.Categories
				.Select(c => new ShowGroupViewModel()
				{
					GroupName = c.Name,
					GroupId = c.Id,
					ProductCount = _context.CategoryToProducts.Count(g => g.CategoryId == c.Id)
				});
		}
	}
}


