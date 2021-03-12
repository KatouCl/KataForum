using System.Linq;
using System.Threading.Tasks;
using KataForum.Data;
using KataForum.Data.Models;
using KataForum.WebApp.Models.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KataForum.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IApplicationUser userService,
            IUpload uploadService)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
        }

        // GET
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                MemberSince = user.MemberSince,
                UserRating = user.Rating.ToString(),
                ProfileImageUrl = user.ProfileImageUrl,
                IsAdmin = userRoles.Contains("Admin")
            };
            
            return View(model);
        }

        /*[HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            
            //Connect to an Azure Storage Account Container 
            //Get Blog Container 
            
            //
        }
        */

        public IActionResult Index()
        {
            var profile = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Email = u.Email,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince,
                });

            var model = new ProfileListingViewModel
            {
                Profiles = profile
            };
            
            return View(model);
        }
    }
}