using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Services
{
    public class UserSettingService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSettingService(UserManager<AppUser> userManager, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppUser> GetUser()
        {
            string name = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (name is not null)
            {
                AppUser appUser = await _userManager.FindByNameAsync(name);
                return appUser;
            }
            return null;
        }
        public async Task<List<Setting>> GetSettingsAsnyc()
        {
            return await _appDbContext.Settings.ToListAsync();
        }
        public async Task<List<ContactMessage>> GetContactMessages()
        {
            return await _appDbContext.ContactMessages.OrderByDescending(x => x.MessageTime).Take(3).ToListAsync();
        }
    }
}
