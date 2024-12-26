
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;   
        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            return genres;
            //return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
          
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }

        Task<Genre> IGenresService.Add(Genre genre)
        {
            throw new NotImplementedException();
        }

        Genre IGenresService.Delete(Genre genre)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Genre>> IGenresService.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<Genre> IGenresService.GetById(byte id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenresService.IsValidGenre(byte id)
        {
            return _context.Genres.AnyAsync(g => g.Id == id);
        }

        Genre IGenresService.Update(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
