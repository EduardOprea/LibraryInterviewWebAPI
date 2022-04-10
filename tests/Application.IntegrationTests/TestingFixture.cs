using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI;
using Xunit;


namespace Application.IntegrationTests
{
    [CollectionDefinition(nameof(TestingFixture))]
    public class TestingCollection : ICollectionFixture<TestingFixture> { }

    public class TestingFixture
    {
        private static IConfigurationRoot _configuration = null;
        private static IServiceScopeFactory _scopeFactory = null;

        public TestingFixture() 
        {
            RunBeforeAnyTests();
        }

        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "WebAPI"));

            services.AddLogging();

            startup.ConfigureServices(services);

            

            _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

    }
}

