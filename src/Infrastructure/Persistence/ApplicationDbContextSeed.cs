using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.Books.Any())
            {
                context.Books.AddRange(new Book
                {
                    Title = "Cei trei muschetari",
                    Author = "Alexandre Dumas",
                    ISBN = "1224424121",
                    BaseRentPrice = 2
                }, 
                new Book
                {
                    Title = "Dupa 20 de ani",
                    Author = "Alexandre Dumas",
                    ISBN = "1224424123",
                    BaseRentPrice = 3
                });
            }

            if (!context.LibraryBadges.Any())
            {
                context.LibraryBadges.AddRange(new LibraryBadge
                {
                    Created = DateTime.Now,
                    Expired = DateTime.Now.Add(TimeSpan.FromDays(365)),
                    OwnerName = "Eduard Oprea"
                }, new LibraryBadge
                {
                    Created = DateTime.Now,
                    Expired = DateTime.Now.Add(TimeSpan.FromDays(365)),
                    OwnerName = "Bruce Wayne"
                });
            }


            await context.SaveChangesAsync(CancellationToken.None);

        }
    }
}
