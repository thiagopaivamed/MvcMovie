namespace MvcMovie.Interface
{
    public interface IUnitOfWork
    {
        public IMovieRepository MovieRepository { get; }

        public Task CommitAsync();
    }
}
