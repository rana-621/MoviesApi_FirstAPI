using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Services;
using System.Diagnostics.CodeAnalysis;

//namespace MoviesApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MoviesController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
//        private long _maxAllowedPosterSize = 1048576;

//        public MoviesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllAsync()
//        {
//            var movies = await _context.Movies
//                .OrderByDescending(x=>x.Rate)
//                .Include(m => m.Genre)
//                .Select(m => new MovieDetailsDto
//                {
//                    Id = m.Id,
//                    GenreId = m.GenreId,
//                    GenreName = m.Genre.Name,
//                    Poster = m.Poster,
//                    Rate = m.Rate,
//                    Storeline = m.Storeline,
//                    Title = m.Title,
//                    Year  = m.Year,
//                })
//                .ToListAsync();
//            return Ok(movies);
//        }

//        [HttpGet("{id}")]
//       public async Task<IActionResult> GetByIdAsync(int id)
//        {
//            //var movie = await _context.Movies.FindAsync(id);
//            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
//            if (movie == null)
//                return NotFound(movie);

//            var dto = new MovieDetailsDto
//            {
//                Id = movie.Id,
//                GenreId = movie.GenreId,
//                //GenreName = movie.Genre?.Name,
//                GenreName = movie.Genre.Name,
//                Poster = movie.Poster,
//                Rate = movie.Rate,
//                Storeline = movie.Storeline,
//                Title = movie.Title,
//                Year = movie.Year,
//            };


//            return Ok(dto);
//        }


//        [HttpGet("GetByGenreId")]
//        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
//        {
//            var movies = _context.Movies
//                .Where(m => m.GenreId == genreId)
//                .OrderByDescending(x => x.Rate)
//                .Include(m => m.Genre)
//                .Select(m => new MovieDetailsDto
//                {
//                    Id = m.Id,
//                    GenreId = m.GenreId,
//                    GenreName = m.Genre.Name,
//                    Poster = m.Poster,
//                    Rate = m.Rate,
//                    Storeline = m.Storeline,
//                    Title = m.Title,
//                    Year = m.Year,
//                })
//                .ToList();
//            return Ok(movies);
//        }


//        [HttpPost]
//        public async Task<IActionResult> CreateAsysc([FromForm]MovieDto dto)
//        {

//            if (dto.Poster == null)
//                return BadRequest("Poster is required!");

//            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
//                return BadRequest("only .png and .jpg images are allowed");

//            if(dto.Poster.Length> _maxAllowedPosterSize)
//                return BadRequest("Max allowed size for poster is 1MB");

//            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
//            if (!isValidGenre)
//                return BadRequest("Invalid genre Id");


//            using var dataStream = new MemoryStream();

//            await dto.Poster.CopyToAsync(dataStream);

//            var movie = new Movie
//            {
//                GenreId = dto.GenreId,
//                Title = dto.Title,
//                Poster = dataStream.ToArray(),
//                Rate = dto.Rate,
//                Storeline = dto.Storeline,
//                Year = dto.Year,
//            };
//            await _context.AddAsync(movie);
//            _context.SaveChangesAsync();

//            return Ok(movie);

//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
//        {
//            var movie = await _context.Movies.FindAsync(id);
//            if(movie == null)
//                return NotFound($"No movie was found with Id {id}");

//            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

//            if (!isValidGenre)
//                return BadRequest("Invalid genre id");

//            if(dto.Poster != null)
//            {
//                if(!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
//                    return BadRequest("only .png and .jpg images are allowed");

//                if (dto.Poster.Length > _maxAllowedPosterSize)
//                    return BadRequest("Max allowed size for poster is 1MB");

//                using var dataStream = new MemoryStream();
//                await dto.Poster.CopyToAsync(dataStream);

//                movie.Poster = dataStream.ToArray();

//            }
//            movie.Title = dto.Title;
//            movie.GenreId = dto.GenreId;
//            movie.Year = dto.Year;
//            movie.Storeline = dto.Storeline;
//            movie.Rate = dto.Rate;


//            _context.SaveChanges();

//            return Ok(movie);

//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult>  DeleteAsync(int id)
//        {
//          //  var movie = await _context.Movies.SingleOrDefaultAsync(s => s.Id == id);
//            var movie = await _context.Movies.FindAsync(id);

//            if (movie == null)
//                return NotFound($"No movie was found with Id {id}");
//            //_context.Movies.Remove(movie);
//            _context.Remove(movie);
//            _context.SaveChanges();

//            return Ok(movie);


//        }
//    }
//}
////-------------------------------------------------------------------
namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMoviesService moviesService, IGenresService genresService)
        {
            _moviesService = moviesService;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
                //ToDo : map movies to DTO
            return Ok(movies);
        }

        [HttpGet("{id}")]
       public async Task<IActionResult> GetByIdAsync(int id)
        {
            //var movie = await _context.Movies.FindAsync(id);
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound(movie);

            var dto = new MovieDetailsDto
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                //GenreName = movie.Genre?.Name,
                GenreName = movie.Genre.Name,
                Poster = movie.Poster,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                Title = movie.Title,
                Year = movie.Year,
            };


            return Ok(dto);
        }


        [HttpGet("GetByGenreId")]
        //public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        //{
        //    var movies = _context.Movies
        //        .Where(m => m.GenreId == genreId)
        //        .OrderByDescending(x => x.Rate)
        //        .Include(m => m.Genre)
        //        .Select(m => new MovieDetailsDto
        //        {
        //            Id = m.Id,
        //            GenreId = m.GenreId,
        //            GenreName = m.Genre.Name,
        //            Poster = m.Poster,
        //            Rate = m.Rate,
        //            Storeline = m.Storeline,
        //            Title = m.Title,
        //            Year = m.Year,
        //        })
        //        .ToList();
        //    return Ok(movies);
        //}


        [HttpPost]
        public async Task<IActionResult> CreateAsysc([FromForm]MovieDto dto)
        {

            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only .png and .jpg images are allowed");

            if(dto.Poster.Length> _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId); 
            if (!isValidGenre)
                return BadRequest("Invalid genre Id");


            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Year = dto.Year,
            };
            _moviesService.Add(movie);

            return Ok(movie);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if(movie == null)
                return NotFound($"No movie was found with Id {id}");

            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);   
            if (!isValidGenre)
                return BadRequest("Invalid genre id");

            if(dto.Poster != null)
            {
                if(!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("only .png and .jpg images are allowed");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();

            }
            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeline;
            movie.Rate = dto.Rate;


            _moviesService.Update(movie);   

            return Ok(movie);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>  DeleteAsync(int id)
        {
          //  var movie = await _context.Movies.SingleOrDefaultAsync(s => s.Id == id);
            var movie = await _moviesService.GetById(id);   
            if (movie == null)
                return NotFound($"No movie was found with Id {id}");
            //_context.Movies.Remove(movie);
            
            _moviesService.Delete(movie);

            return Ok(movie);


        }
    }
}
