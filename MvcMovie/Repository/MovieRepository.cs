
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Interface;
using MvcMovie.Models;

namespace MvcMovie.Repository
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly MvcMovieContext _mvcMovieContext;
        public MovieRepository(MvcMovieContext mvcMovieContext) : base(mvcMovieContext)
        {
            _mvcMovieContext = mvcMovieContext;
        }

        public async Task<IEnumerable<string>> GetAllGenres()
        {
            return await _mvcMovieContext.Movie.Select(m => m.Genre).Distinct().ToListAsync();
        }
    }
}
