using MvcMovie.Data;
using MvcMovie.Interface;

namespace MvcMovie.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MvcMovieContext _mvcMovieContext;
        private MovieRepository? _movieRepository = null;
        private bool _disposed = false;

        public UnitOfWork(MvcMovieContext mvcMovieContext)
        {
            _mvcMovieContext = mvcMovieContext;
        }

        public async Task CommitAsync()
        {
            await _mvcMovieContext.SaveChangesAsync();
        }

        public IMovieRepository MovieRepository
        {
            get
            {
                if (_movieRepository is null)
                    _movieRepository = new MovieRepository(_mvcMovieContext);
                return _movieRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                    _mvcMovieContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
