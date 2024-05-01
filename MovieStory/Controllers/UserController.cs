using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MovieStory.Models;
using Newtonsoft.Json;

namespace MovieStory.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string SearchOption { get; set; } = "Title";
        private static string SearchValue { get; set; }
        private static string Email { get; set; }
        private static List<int> FavouriteMovie { get; set; } 

        private static readonly List<Movie> Movie = new List<Movie>()
        {
          new Movie { Id = 1, Title = "The Shawshank Redemption", Cast = "Tim Robbins, Morgan Freeman", Category = "Drama", Budget = "$25,000,000", ReleaseDate = "01/06/1994" },
          new Movie { Id = 2, Title = "The Godfather", Cast = "Marlon Brando, Al Pacino", Category = "Crime", Budget = "$6,000,000", ReleaseDate = "24/03/1972" },
          new Movie { Id = 3, Title = "The Dark Knight", Cast = "Christian Bale, Heath Ledger", Category = "Action", Budget = "$185,000,000", ReleaseDate = "18/07/2008" },
          new Movie { Id = 4, Title = "The Lord of the Rings: The Fellowship of the Ring", Cast = "Elijah Wood, Ian McKellen", Category = "Fantasy", Budget = "$93,000,000", ReleaseDate = "19/12/2001" },
          new Movie { Id = 5, Title = "Pulp Fiction", Cast = "John Travolta, Uma Thurman", Category = "Crime", Budget = "$8,000,000", ReleaseDate = "14/09/1994" },
          new Movie { Id = 6, Title = "Forrest Gump", Cast = "Tom Hanks, Robin Wright", Category = "Drama", Budget = "$55,000,000", ReleaseDate = "06/07/1994" },
          new Movie { Id = 7, Title = "Inception", Cast = "Leonardo DiCaprio, Elliot Page", Category = "Sci-Fi", Budget = "$160,000,000", ReleaseDate = "16/07/2010" },
          new Movie { Id = 8, Title = "The Matrix", Cast = "Keanu Reeves, Laurence Fishburne", Category = "Sci-Fi", Budget = "$60,000,000", ReleaseDate = "31/03/1999" },
          new Movie { Id = 9, Title = "The Good, the Bad and the Ugly", Cast = "Clint Eastwood, Eli Wallach", Category = "Western", Budget = "$12,000,000", ReleaseDate = "12/12/1966" },
          new Movie { Id = 10, Title = "Fight Club", Cast = "Brad Pitt, Edward Norton", Category = "Drama", Budget = "$63,000,000", ReleaseDate = "15/10/1999" }
        };

        public UserController(ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Index()
        {
            Email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            FavouriteMovie = GetList();

            if (FavouriteMovie == null)
                FavouriteMovie = new List<int>();

            if (Email == null)
            {
               return RedirectToAction("Index", "Register");
            }

            return View();
        }

        [HttpGet("{controller}/GetEmail")]
        public IActionResult GetEmail()
        {
            return Ok(new { email = Email });
        }

        [HttpGet("{controller}/GetSearchValue")]
        public IActionResult SearchMovie() 
        {
            return Ok(new { option = SearchOption , value = SearchValue});
        }

        [HttpPost("{controller}/PostSearchValue")]
        public IActionResult SearchMovie(string option, string value)
        {
            SearchOption = option;
            SearchValue = value;

            return Ok(new { success = true });
        }

        [HttpGet("{controller}/GetMovie")]
        public IActionResult GetMovieList()
        {
            List<Movie> movies = new List<Movie>();

            if (FavouriteMovie != null)
            {
                foreach (var item in FavouriteMovie)
                {
                    var movie = Movie.FirstOrDefault(m => m.Id == item);
                    if (movie != null)
                        movies.Add(movie);
                }
            }

            if (SearchValue != null)
            {
                movies = movies.OrderBy(movie => movie.Title).ToList();

                if (SearchOption == "Title")
                {
                    movies = movies.Where(movie => movie.Title.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else if (SearchOption == "Cast")
                {
                    movies = movies.Where(movie => movie.Cast.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
                else
                {
                    movies = movies.Where(movie => movie.Category.ToLower().Contains(SearchValue.ToLower())).ToList();
                }
            }
            return Ok(movies);
        }
        

        [HttpPost("{controller}/ChangeFavourite")]
        public IActionResult FavoriteMovie(int id)
        {
            if (Email == null)
                return Ok(new { success = false });
            else
            {
                FavouriteMovie.Remove(id);
                SetList(FavouriteMovie);

                return Ok(new { success = true });
            }
        }

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Movie ID is required.");
            }

            var movie = Movie.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public List<int> GetList()
        {
            string serializedList = _httpContextAccessor.HttpContext.Session.GetString("FavouriteMovies");
            if (serializedList != null)
            {
                List<int> retrievedList = JsonConvert.DeserializeObject<List<int>>(serializedList);
                return retrievedList;
            }
            else
            {
                return null;
            }
        }

        public void SetList(List<int> favourite)
        {
            string serializedList = JsonConvert.SerializeObject(favourite);
            _httpContextAccessor.HttpContext.Session.SetString("FavouriteMovies", serializedList);
        }
    }
}
