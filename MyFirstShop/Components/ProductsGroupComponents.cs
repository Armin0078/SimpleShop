using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Data.Repositories;
using MyFirstShop.Models;

namespace MyFirstShop.Components
{
	public class ProductsGroupComponents:ViewComponent
	{
        private IGroupRepository _groupRepository;

        public ProductsGroupComponents(IGroupRepository groupRepository)
        {
			_groupRepository = groupRepository;

		}

        public async Task<IViewComponentResult> InvokeAsync()
        { 

            return View("/Views/Components/ProductsGroupComponents.cshtml", _groupRepository.GetGroupForShow());
        }
    }
}
