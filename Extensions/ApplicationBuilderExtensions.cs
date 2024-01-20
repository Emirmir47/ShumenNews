using System.Reflection;
using ShumenNews.Data;
using ShumenNews.Data.Seeding;

namespace ShumenNews.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDatabaseSeeding(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider
                    .GetRequiredService<ShumenNewsDbContext>();

                context.Database.EnsureCreated();

                Assembly.GetAssembly(typeof(ShumenNewsDbContext))
                .GetTypes()
                    .Where(type => typeof(ISeeder).IsAssignableFrom(type))
                .Where(type => type.IsClass)
                    .Select(type => (ISeeder)serviceScope.ServiceProvider.GetRequiredService(type))
                    .ToList()
                    .ForEach(seeder => seeder.Seed().GetAwaiter().GetResult());
            }
        }
    }
}
