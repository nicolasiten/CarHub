using CarHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Tests
{
    public abstract class DbContextTestBase
    {
        protected readonly ApplicationDbContext applicationDbContext;

        public DbContextTestBase()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            var applicationDbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            applicationDbContext = new ApplicationDbContext(applicationDbContextOptions);
        }
    }
}
