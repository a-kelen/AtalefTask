using AtalefTask.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtalefTask.IntegrationTests
{
    public class WebFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        public ApplicationContext DB { get; private set; }
        private CustomWebApplicationFactory factory;
        public WebFixture()
        
        {
            this.factory = new CustomWebApplicationFactory();
            this.Client = this.factory.CreateClient();
            this.DB = this.factory.DB;
        }

        public void Dispose()
        {
            factory.DB.Dispose();
            factory.Dispose();
        }
    }

    class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ApplicationContext DB { get; private set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    this.DB = db;
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    try
                    {
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
        private static void InitializeDbForTests(ApplicationContext db)
        {
            db.SmartMatchResult.AddRange(Data.InitItems);
            db.SaveChanges();
            Data.DBItems = db.SmartMatchResult.ToList();
        }
    }
}
