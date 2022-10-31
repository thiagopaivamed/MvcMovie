using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMovie.Data;
using MvcMovie.Interface;
using MvcMovie.Models;
using MvcMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMovie.Tests
{
    [TestClass]
    public class MoviesIntegrationTests
    {
        private Movie? movie = null;
        private IEnumerable<Movie>? listMovies = null;
        private IUnitOfWork? unitOfWork = null;

        public MoviesIntegrationTests()
        {
            movie = new Movie
            {
                Title = "When Harry Met Sally",
                Genre = "Romantic Comedy",
                Price = 9.99M,
                Rating = "A",
                ReleasedDate = new DateTime(1989, 02, 12, 0, 0, 0)
            };

            listMovies = new List<Movie>()
            {
                new Movie
                {
                    Title = "When Harry Met Sally",
                    Genre = "Romantic Comedy",
                    Price = 9.99M,
                    Rating = "A",
                    ReleasedDate = new DateTime(1989, 02, 12, 0, 0, 0)
                },

                new Movie
                {
                    Title = "Stealth",
                    Genre = "Scientific Fiction",
                    Price = 19.99M,
                    Rating = "S",
                    ReleasedDate = new DateTime(2002, 02, 09, 0, 0, 0)
                },

                new Movie
                {
                    Title = "Avengers",
                    Genre = "Adventure",
                    Price = 149.99M,
                    Rating = "B",
                    ReleasedDate = new DateTime(2010, 05, 07, 0, 0, 0)
                }
            };

            var services = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddDbContext<MvcMovieContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MvcMovieContext")));

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();
            unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        }

        [TestInitialize]
        public void Iniatiate()
        {
            Assert.IsNotNull(movie);
            Assert.IsNotNull(listMovies);
            Assert.IsNotNull(unitOfWork);
        }

        [TestMethod]
        public async Task Should_Get_All_Movies()
        {
            IEnumerable<Movie> movies = await unitOfWork.MovieRepository.GetAll();
            Assert.IsNotNull(movies);
        }

        [TestMethod]
        public async Task Return_Movie_By_Its_Id()
        {
            Movie movieToBeTested = await unitOfWork.MovieRepository.GetById(1);
            Assert.IsNotNull(movieToBeTested);
            Assert.IsTrue(movieToBeTested.Id > 0);
        }

        [TestMethod]
        public async Task Create_New_Movie()
        {
            await unitOfWork.MovieRepository.Insert(movie);
            await unitOfWork.CommitAsync();

            Assert.IsTrue(movie.Id > 0);
        }

        [TestMethod]
        public async Task Update_A_Movie()
        {
            Movie? movieToBeUpdated = null;
            int? movieId = null;
            string movieTitle = "Transformers";
            decimal? moviePrice = Math.Round((decimal)Random.Shared.NextDouble() * 100, 2);

            while (movieToBeUpdated == null)
            {
                movieId = Random.Shared.Next(1, 10);
                movieToBeUpdated = await unitOfWork.MovieRepository.GetById(movieId);
            }

            Assert.IsNotNull(movieTitle);
            Assert.IsNotNull(moviePrice);
            Assert.IsNotNull(movieToBeUpdated);
            Assert.AreNotEqual(movieToBeUpdated.Title, movieTitle);
            Assert.AreNotEqual(movieToBeUpdated.Price, moviePrice);

            movieToBeUpdated.Title = movieTitle;
            movieToBeUpdated.Price = (decimal)moviePrice;

            unitOfWork.MovieRepository.Update(movieToBeUpdated);
            await unitOfWork.CommitAsync();

            Assert.AreEqual(movieToBeUpdated.Title, movieTitle);
            Assert.AreEqual(movieToBeUpdated.Price, moviePrice);

            Movie? movieUpdated = await unitOfWork.MovieRepository.GetById(movieToBeUpdated.Id);
            Assert.IsNotNull(movieUpdated);
            Assert.AreEqual(movieUpdated.Id, movieToBeUpdated.Id);
            Assert.AreEqual(movieUpdated.Title, movieToBeUpdated.Title);
            Assert.AreEqual(movieUpdated.Genre, movieToBeUpdated.Genre);
            Assert.AreEqual(movieUpdated.ReleasedDate, movieToBeUpdated.ReleasedDate);
            Assert.AreEqual(movieUpdated.Price, movieToBeUpdated.Price);
        }

        [TestMethod]
        public async Task Delete_A_Movie()
        {
            Movie? movieToBeDeleted = null;
            int? movieId = null;           
            
            while (movieToBeDeleted == null)
            {
                movieId = Random.Shared.Next(1, 10);
                movieToBeDeleted = await unitOfWork.MovieRepository.GetById(movieId);
            }

            Assert.IsNotNull(movieToBeDeleted);            
            
            await unitOfWork.MovieRepository.DeleteById(movieToBeDeleted.Id);
            await unitOfWork.CommitAsync();
            
            Movie? movieDeleted = await unitOfWork.MovieRepository.GetById(movieToBeDeleted.Id);
            Assert.IsNull(movieDeleted);            
        }

        [TestCleanup]
        public void CleanUp()
        {
            movie = null;
            listMovies = null;    
            unitOfWork = null;

            Assert.IsNull(movie);
            Assert.IsNull(listMovies);   
            Assert.IsNull(unitOfWork);
        }
    }
}
