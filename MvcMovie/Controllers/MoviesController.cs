using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Interface;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {        
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string? movieGenre, string? searchString)
        {
            IEnumerable<Movie> movies = new List<Movie>();

            if ((movieGenre is not null && searchString is not null) && (!string.IsNullOrEmpty(movieGenre) && !string.IsNullOrEmpty(searchString)))
            {
                movies = await _unitOfWork.MovieRepository.GetAll(m => m.Genre == movieGenre && m.Title.Contains(searchString));
            }

            else if (searchString is not null && !String.IsNullOrEmpty(searchString))
            {
                movies = await _unitOfWork.MovieRepository.GetAll(s => s.Title!.Contains(searchString));
            }

            else if (movieGenre is not null && !String.IsNullOrEmpty(movieGenre))
            {
                movies = await _unitOfWork.MovieRepository.GetAll(m => m.Genre == movieGenre);
            }

            else
            {
                movies = await _unitOfWork.MovieRepository.GetAll();
            }

            MovieGenreViewModel movieGenreViewModel = new MovieGenreViewModel
            {
                Genres = new SelectList(await _unitOfWork.MovieRepository.GetAllGenres()),
                Movies = movies.ToList()
            };



            return View(movieGenreViewModel);
        }


        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Movie movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleasedDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.MovieRepository.Insert(movie);
                await _unitOfWork.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Movie movie = await _unitOfWork.MovieRepository.GetById(id);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleasedDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _unitOfWork.MovieRepository.Update(movie);
                await _unitOfWork.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            Movie movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie is not null)
            {
                await _unitOfWork.MovieRepository.DeleteById(id);
                await _unitOfWork.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
