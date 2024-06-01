using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Models;

namespace MyFirstShop.Pages.Admin.ManageUsers
{
    public class IndexModel : PageModel
    {
        private readonly MyFirstShop.Data.MyFirstShopContext _context;

        public IndexModel(MyFirstShop.Data.MyFirstShopContext context)
        {
            _context = context;
        }

        public IList<Users> Users { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                Users = await _context.Users.ToListAsync();
            }
        }
    }
}
