using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcMovie.Controllers;
using MvcMovie.Data;
using MvcMovie.Interface;
using MvcMovie.Models;
using MvcMovie.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Tests
{
    [TestClass]
    public class MoviesTests
    {
        private Mock<IUnitOfWork>? mockUnitOfWork = null;
        private Movie? movie = null;
        private IEnumerable<Movie>? listMovies = null;

        public MoviesTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

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

        }

        [TestInitialize]
        public void Initiate()
        {
            Assert.IsNotNull(mockUnitOfWork);
            Assert.IsNotNull(movie);
            Assert.IsNotNull(listMovies);
        }        

        [TestMethod]
        public async Task Should_Return_Movie_By_its_Id()
        {
            mockUnitOfWork.Setup(m => m.MovieRepository.GetById(It.IsAny<int>())).ReturnsAsync(movie);

            Movie movieToBeTested = await mockUnitOfWork.Object.MovieRepository.GetById(It.IsAny<int>());

            mockUnitOfWork.Verify(m => m.MovieRepository.GetById(It.IsAny<int>()), Times.Exactly(1));

            Assert.AreEqual(movieToBeTested, movie);
            Assert.IsInstanceOfType(movieToBeTested, typeof(Movie));
            Assert.IsNotNull(movieToBeTested);

            mockUnitOfWork.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Should_Return_ListOfMovies()
        {
            mockUnitOfWork.Setup(m => m.MovieRepository.GetAll(null)).ReturnsAsync(listMovies);
            IEnumerable<Movie> listOfMoviesToBeTested = await mockUnitOfWork.Object.MovieRepository.GetAll(null);

            mockUnitOfWork.Verify(m => m.MovieRepository.GetAll(null), Times.Exactly(1));

            Assert.IsNotNull(listOfMoviesToBeTested);
            Assert.AreEqual(listMovies, listOfMoviesToBeTested);
            Assert.IsTrue(listOfMoviesToBeTested.Any());
            Assert.IsTrue(listOfMoviesToBeTested.Count() == 3);

            mockUnitOfWork.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Should_Create_New_Movie()
        {
            mockUnitOfWork.Setup(x => x.MovieRepository.Insert(It.IsAny<Movie>()));
            await mockUnitOfWork.Object.MovieRepository.Insert(movie);
            mockUnitOfWork.Verify(x => x.MovieRepository.Insert(It.IsAny<Movie>()), Times.Once);

            mockUnitOfWork.Setup(x => x.CommitAsync());
            await mockUnitOfWork.Object.CommitAsync();
            mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);

            mockUnitOfWork.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Should_Update_Movie()
        {      
            mockUnitOfWork.Setup(x => x.MovieRepository.Update(It.IsAny<Movie>()));

            foreach (Movie movie in listMovies)
            {
                mockUnitOfWork.Object.MovieRepository.Update(movie);
            }            

            mockUnitOfWork.Verify(x => x.MovieRepository.Update(It.IsAny<Movie>()), Times.Exactly(listMovies.Count()));

            mockUnitOfWork.Setup(x => x.CommitAsync());
            await mockUnitOfWork.Object.CommitAsync();
            mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once());

            mockUnitOfWork.VerifyNoOtherCalls();
        }        

        [TestMethod]
        public async Task Should_Delete_Movie()
        {
            mockUnitOfWork.Setup(x => x.MovieRepository.DeleteById(It.IsAny<int>()));

            foreach (Movie movie in listMovies)
            {
                await mockUnitOfWork.Object.MovieRepository.DeleteById(movie.Id);
            }
            mockUnitOfWork.Verify(x => x.MovieRepository.DeleteById(It.IsAny<int>()), Times.Exactly(listMovies.Count()));

            mockUnitOfWork.Setup(x => x.CommitAsync());
            await mockUnitOfWork.Object.CommitAsync();
            mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once());

            mockUnitOfWork.VerifyNoOtherCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            mockUnitOfWork = null;
            movie = null;
            listMovies = null;

            Assert.IsNull(mockUnitOfWork);
            Assert.IsNull(movie);
            Assert.IsNull(listMovies);
        }
    }
}
