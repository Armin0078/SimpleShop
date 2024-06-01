using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyFirstShop.Data;
using MyFirstShop.Models;
using System.Diagnostics;
using System.Security.Claims;
using ZarinpalSandbox;

namespace MyFirstShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MyFirstShopContext _context;



        public HomeController(ILogger<HomeController> logger, MyFirstShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var products = _context.Products.Include(c => c.Item)
                .SingleOrDefault(p => p.Id == id);


            if (products == null)
            {
                return NotFound();
            }

            var categories = _context.Products.Where(p => p.Id == id)
                .SelectMany(c => c.CategoryToProduct)
                .Select(ca => ca.Category)
                .ToList();

            var vm = new DetailsViewModel()
            {
                Product = products,
                Categories = categories
            };


            return View(vm);
        }

        [Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var product = _context.Products.Include(p => p.Item).SingleOrDefault(p => p.ItemId == itemId);
            if (product != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _context.orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinally);
                if (order != null)
                {
                    var orderDetail =
                        _context.ordersDetial.FirstOrDefault(d =>
                            d.OrderId == order.OrderId && d.ProductId == product.Id);
                    if (orderDetail != null)
                    {
                        orderDetail.Count += 1;
                    }
                    else
                    {
                        _context.ordersDetial.Add(new OrderDetial()
                        {
                            OrderId = order.OrderId,
                            ProductId = product.Id,
                            Price = product.Item.price,
                            Count = 1
                        });
                    }
                }
                else
                {
                    order = new Order()
                    {
                        IsFinally = false,
                        CreateDate = DateTime.Now,
                        UserId = userId
                    };
                    _context.orders.Add(order);
                    _context.SaveChanges();
                    _context.ordersDetial.Add(new OrderDetial()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.price,
                        Count = 1
                    });
                }

                _context.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }

        [Authorize]
        public IActionResult RemoveFromCart(int detailId)
        {
            var orderDetial = _context.ordersDetial.FirstOrDefault(d=> d.ProductId == detailId);

            var orderCount = _context.ordersDetial
                .FirstOrDefault(f=>f.ProductId == detailId);

            if (orderCount != null)
            {
                if (orderCount.Count == 1)
                {
                    _context.Remove(orderDetial);
                    _context.SaveChanges();
                }
                else
                {
                    orderCount.Count -= 1;
                    _context.SaveChanges();
                }
            }
            

            return RedirectToAction("ShowCart");
        }

        [Authorize]
        public ActionResult ShowCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _context.orders.Where(o=>o.UserId == userId && !o.IsFinally)
                .Include(o=> o.OrderDetials)
                .ThenInclude(c=>c.Product).FirstOrDefault();

            return View(order);
        }

        //[Authorize]
        //public IActionResult Payment()
        //{
        //    var order = _context.orders.SingleOrDefault(o => !o.IsFinally);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    // I have to correct this one.
        //    var payment = new Payment(order.OrderDetials.First(c=>((int)c.Price)));

        //    var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
        //    "https://localhost:7077/Home/OnlinePayment" + order.OrderId, "armin@gmail.com", "0679786452");

        //    if (res.Result.Status == 100)
        //    {
        //        return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" +
        //            res.Result.Authority);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }

        //    return View();
        //}

        //[Authorize]
        //public IActionResult OnlinePayment()
        //{
        //	return View();
        //}

        public IActionResult Privacy()
        {
			return View();

		}

		



		public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


