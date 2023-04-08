using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
//using Alpha_Hotel_Project.Migrations;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Threading;

namespace Alpha_Hotel_Project.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public RoomController(AppDbContext appDbContext , UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index(int page=1)
        {
            List<Partner> partners = _appDbContext.Partners.ToList();
            var query = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).AsQueryable();
            RoomViewModel roomViewModel = new RoomViewModel
            {
                Features = _appDbContext.Features.ToList(),
                RoomFeatures = _appDbContext.RoomFeatures.ToList(),
                Partners = partners,
                Rooms = PaginatedList<Room>.Create(query, 4, page)
            };
            return View(roomViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            AppUser member = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            Room room = _appDbContext.Rooms.Include(x => x.Category).Include(x=>x.RoomFeatures).Include(x => x.RoomImages).FirstOrDefault(x => x.Id == id);
            OrderViewModel orderViewModel = new OrderViewModel
            {
                Features = _appDbContext.Features.Where(x=>x.IsDeleted==false).ToList(),
                RoomFeatures = _appDbContext.RoomFeatures.Where(x => x.RoomId == room.Id).ToList(),
                HouseRules = _appDbContext.HouseRules.ToList(),
                PopularRooms = _appDbContext.Rooms.Include(x=>x.RoomImages).OrderByDescending(x=>x.ViewCount).Take(4).ToList(),
                RecentRooms = _appDbContext.Rooms.Include(x=>x.RoomImages).OrderByDescending(x=>x.RoomCreationDate).Take(3).ToList(),
                Partners = _appDbContext.Partners.ToList(),
                Room = room,
                Fullname = member?.Fullname,
                PhoneNumber = member?.PhoneNumber,
                eMail = member?.Email
            };
            room.ViewCount++;
            if (room is null) return View("Error");
            _appDbContext.SaveChanges();
            return View(orderViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> BookingSystem(Guid id, OrderViewModel orderVM)
        {
            AppUser member = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            orderVM.Room = room;
            orderVM.RoomId = room.Id;
            orderVM.HouseRules = _appDbContext.HouseRules.ToList();
            orderVM.RoomFeatures = _appDbContext.RoomFeatures.Where(x=>x.RoomId==room.Id).ToList();
            orderVM.Features = _appDbContext.Features.ToList();
            orderVM.Partners = _appDbContext.Partners.ToList();
            orderVM.PopularRooms = _appDbContext.Rooms.Include(x=>x.RoomImages).OrderByDescending(x=>x.ViewCount).Take(4).ToList();
            orderVM.RecentRooms = _appDbContext.Rooms.Include(x=>x.RoomImages).OrderByDescending(x=>x.RoomCreationDate).Take(3).ToList();
            if(orderVM.Country == "Country") {
                ModelState.AddModelError("", "The Country field is required.");
                return View("detail", orderVM);
            }
            if((orderVM.AdultCount + orderVM.ChildCount) > room.Capacity)
            {
                ModelState.AddModelError("", "Please, check room capacity");
                return View("detail", orderVM);
            }
            if (!ModelState.IsValid) return View("detail" , orderVM);
            int Startdate = orderVM.StartRentDate.DayOfYear;
            int Enddate = orderVM.EndRentDate.DayOfYear;
            string YearDate = orderVM.StartRentDate.ToString("yyyy");
            int dayCount = Enddate - Startdate;
            if (!ModelState.IsValid) return View("detail", orderVM);
            double TotalPrice = dayCount * (room.AdultPrice*orderVM.AdultCount + room.ChildPrice*orderVM.ChildCount);
            foreach (var item in _appDbContext.OrderItems.Include(x => x.Order).Where(x => x.RoomId == orderVM.Room.Id).Where(x => x.Order != null).Where(x=>x.Order.OrderStatus==Enums.OrderStatus.Accepted))
            {
                string year = item.StartRentDate.ToString("yyyy");
                int date = item.StartRentDate.DayOfYear;
                int date2 = item.EndRentDate.DayOfYear;
                for (int i = date; i <= date2; i++)
                {
                    for (int j = Startdate; j <= Enddate; j++)
                    {
                        if (i == j && YearDate == year)
                        {
                            ModelState.AddModelError("", "Already Reserved");
                            return View("detail", orderVM);
                        }
                    }
                }
            }
            if (orderVM.EndRentDate < orderVM.StartRentDate)
            {
                ModelState.AddModelError("EndRentDate", "Please , choice correct date");
                return View("detail", orderVM);
            }
            if (orderVM.StartRentDate < DateTime.Now)
            {
                ModelState.AddModelError("StartRentDate", "Please , choice correct date");
                return View("detail", orderVM);
            }
            if (orderVM.EndRentDate < DateTime.Now)
            {
                ModelState.AddModelError("EndRentDate", "Please , choice correct date");
                return View("detail", orderVM);
            }
            if (dayCount > 30)
            {
                ModelState.AddModelError("EndRentDate", "Up to 30 day reservation allowed");
                return View("detail", orderVM);
            }
            if (orderVM.ChildCount < 0 || orderVM.AdultCount <= 0 || orderVM.ChildCount  > 5 || orderVM.AdultCount > 5)
            {
                ModelState.AddModelError("ChildCount", "Please , select correct count");
                return View("detail", orderVM);
            }
            if (orderVM.StartRentDate.DayOfYear == orderVM.EndRentDate.DayOfYear)
            {
                ModelState.AddModelError("StartRentDate", "Reservation allowed from 1 up to 30 days");
                return View("detail", orderVM);
            }
            OrderItem orderItem = new OrderItem
            {
                RoomId = room.Id,
                TotalPrice = TotalPrice,
                DayCount = dayCount,
                Fullname = orderVM.Fullname,
                Country = orderVM.Country,
                Address = orderVM.Address,
                City = orderVM.City,
                ZipCode = orderVM.ZipCode,
                Note = orderVM.Note,
                PhoneNumber = orderVM.PhoneNumber,
                eMail = orderVM.eMail,
                AdultCount = orderVM.AdultCount,
                ChildCount = orderVM.ChildCount,
                Type = room.Type,
                StartRentDate = orderVM.StartRentDate,
                EndRentDate = orderVM.EndRentDate,
                AppUserId = member?.Id,
            };
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            string orderItemStr = JsonConvert.SerializeObject(orderItem);
            HttpContext.Response.Cookies.Append("OrderItem", orderItemStr);
            CheckOutViewModel checkOutViewModel = new CheckOutViewModel
            {
                OrderItem = orderItem,
                Room = room,
                HouseRules = _appDbContext.HouseRules.ToList()
            };
            return View(checkOutViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Order(CheckOutViewModel checkOutViewModel)
        {
            string orderItemStr = HttpContext.Request.Cookies["OrderItem"];
            if(orderItemStr is not null)
            {
                checkOutViewModel.OrderItem = JsonConvert.DeserializeObject<OrderItem>(orderItemStr);
            }
            Room room = _appDbContext.Rooms.Include(x=>x.RoomImages).FirstOrDefault(x=>x.Id == checkOutViewModel.OrderItem.RoomId);
            checkOutViewModel.OrderItem.Room = room;
            checkOutViewModel.HouseRules = _appDbContext.HouseRules.ToList();
            checkOutViewModel.Room = checkOutViewModel.OrderItem.Room;
            checkOutViewModel.OrderItem.Id = Guid.NewGuid();
            if (checkOutViewModel.CardNumber is null)
            {
                ModelState.AddModelError("", "Please , write card number");
                return View("bookingsystem", checkOutViewModel);
            }
            if (!(checkOutViewModel.CVV > 99 && checkOutViewModel.CVV < 1000))
            {
                ModelState.AddModelError("cvv", "Please , choice correct cvv");
                return View("bookingsystem", checkOutViewModel);
            }
            int NowDateYear = int.Parse(DateTime.Now.ToString("yy"));
            int NowDateMonth = int.Parse(DateTime.Now.ToString("MM"));
            if ((int.Parse(checkOutViewModel.CardYear.ToString()) <= NowDateYear))
            {
                if (int.Parse(checkOutViewModel.CardMonth.ToString()) < NowDateMonth)
                {
                    ModelState.AddModelError("", "Please , choice correct Month");
                    return View("bookingsystem", checkOutViewModel);
                }
            }
            Order order = new Order
            {
                OrderStatus = Enums.OrderStatus.Pending,
                CreateDate = DateTime.UtcNow.AddHours(4),
                AppUserId = checkOutViewModel.OrderItem?.AppUserId,
                OrderItem = checkOutViewModel.OrderItem,
                RoomId = checkOutViewModel.OrderItem?.RoomId,
                CardMonth = (int)checkOutViewModel.CardMonth,
                CardYear = (int)checkOutViewModel.CardYear,
                CardNumber = (string)checkOutViewModel.CardNumber,
                CVV = (int)checkOutViewModel.CVV,
                IsCancel = false
            };
            OrderItem orderItem = new OrderItem
            {
                Fullname = checkOutViewModel.OrderItem.Fullname,
                Address = checkOutViewModel.OrderItem.Address,
                Country = checkOutViewModel.OrderItem.Country,
                eMail = checkOutViewModel.OrderItem.eMail,
                City = checkOutViewModel.OrderItem.City,
                PhoneNumber = checkOutViewModel.OrderItem.PhoneNumber,
                Note = checkOutViewModel.OrderItem.Note,
                ZipCode = checkOutViewModel.OrderItem.ZipCode,
                CreateDate = DateTime.UtcNow.AddHours(4),
                AppUserId = checkOutViewModel.OrderItem?.AppUserId,
                DayCount = checkOutViewModel.OrderItem.DayCount,
                TotalPrice = checkOutViewModel.OrderItem.TotalPrice,
                Room = room,
                RoomId = checkOutViewModel.OrderItem.RoomId,
                Order = order,
                AdultCount = checkOutViewModel.OrderItem.AdultCount,
                ChildCount = checkOutViewModel.OrderItem.ChildCount,
                Type = checkOutViewModel.OrderItem.Type,
                StartRentDate = checkOutViewModel.OrderItem.StartRentDate,
                EndRentDate = checkOutViewModel.OrderItem.EndRentDate,
            };
            _appDbContext.OrderItems.Add(checkOutViewModel.OrderItem);
            HttpContext.Response.Cookies.Delete("OrderItem");
            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();
            return RedirectToAction("index","home");
        }
    }
}
