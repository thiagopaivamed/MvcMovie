using MvcMovie.Models;

namespace MvcMovie.Interface
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        public Task<IEnumerable<string>> GetAllGenres();
    }
}
