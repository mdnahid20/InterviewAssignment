using Microsoft.AspNetCore.Mvc;
using MovieStory.Models;
using Microsoft.AspNetCore.Session;

namespace MovieStory.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegisterController(ILogger<RegisterController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("{Controller}/PostEmail")]
        public IActionResult Register(string email)
        {
             if(email == null) 
             {
                return Ok(new { success = false });
             }

            _httpContextAccessor.HttpContext.Session.SetString("Email", email);
            _httpContextAccessor.HttpContext.Session.Remove("FavouriteMovies");
            return Ok(new { success = true });
        }
    }
}
