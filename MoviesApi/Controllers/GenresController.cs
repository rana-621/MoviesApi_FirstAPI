using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Services;

///Before add folder Service and use it in my code

//namespace MoviesApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GenresController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        public GenresController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllAsync()
//        {
//            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
//            return Ok(genres);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateAsync(GenreDto dto)
//        {
//            var genre = new Genre
//            { Name = dto.Name };
//            // await _context.Genres.AddAsync(genre); //you can remove Genres because we write code in genres class controller
//            await _context.AddAsync(genre);
//            _context.SaveChanges();
//            //return Ok();//can you write any thing of this 2 lines
//            return Ok(genre);
//        }

//        //UPdate
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateAsync(int id ,[FromBody] GenreDto dto)
//        {
//            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
//            if (genre == null)
//                return NotFound($"No genre was found with Id {id}");
//            genre.Name = dto.Name;
//            _context.SaveChanges();
//            return Ok(genre);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAsync(int id)
//        {
//            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
//            if (genre == null)
//                return NotFound($"No genre was found with Id {id}");

//            //Remove from database
//           // _context.Genres.Remove(genre); //it will be understand the Genres from paramater
//            _context.Remove(genre);
//            _context.SaveChanges();
//            // return Ok();
//            return Ok(genre);
//        }

//    }
//}
//-------------------------------------------------------------------------------------------------------------------
////using service and the inplementation of the interfaces  in it 
///

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;
        public GenresController( IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre
            { Name = dto.Name };
            await _genresService.Add(genre);
            return Ok(genre);
        }

        //UPdate
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id ,[FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetById(id);
                if (genre == null)
                return NotFound($"No genre was found with Id {id}");
            genre.Name = dto.Name;
            _genresService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);
            if (genre == null)
            return NotFound($"No genre was found with Id {id}");
            _genresService.Delete(genre);
            return Ok(genre);
        }

    }
}
