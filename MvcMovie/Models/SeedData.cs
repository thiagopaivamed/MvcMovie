﻿using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (MvcMovieContext context = new MvcMovieContext(serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
            {
                if(context.Movie.Any())
                {
                    return;
                }
                context.Movie.AddRange(
                   new Movie
                   {
                       Title = "When Harry Met Sally",
                       ReleasedDate = DateTime.Parse("1989-2-12"),
                       Genre = "Romantic Comedy",
                       Price = 7.99M,
                       Rating = "A"
                   },

                   new Movie
                   {
                       Title = "Ghostbusters ",
                       ReleasedDate = DateTime.Parse("1984-3-13"),
                       Genre = "Comedy",
                       Price = 8.99M,
                       Rating = "B"
                   },

                   new Movie
                   {
                       Title = "Ghostbusters 2",
                       ReleasedDate = DateTime.Parse("1986-2-23"),
                       Genre = "Comedy",
                       Price = 9.99M,
                       Rating = "C"
                   },

                   new Movie
                   {
                       Title = "Rio Bravo",
                       ReleasedDate  = DateTime.Parse("1959-4-15"),
                       Genre = "Western",
                       Price = 3.99M,
                       Rating = "D"
                       
                   }
               );
                context.SaveChanges();
            }
        }
    }
}
