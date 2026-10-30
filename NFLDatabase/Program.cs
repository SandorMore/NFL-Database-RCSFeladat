using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Import;

namespace NFLDatabase
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<EspnClient>(c =>
                c.BaseAddress = new Uri("https://site.api.espn.com/apis/site/v2/sports/football/nfl/"));

            builder.Services.AddDbContext<NFLDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("NFLDB")));

            builder.Services.AddScoped<DataSeeder>();

            var app = builder.Build();

            // Apply migrations and fill the database from ESPN the first time the app runs
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<NFLDbContext>();
                await db.Database.MigrateAsync();

                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                await seeder.SeedAsync();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Number inputs always post decimals with a dot (e.g. 7.5 sacks), so don't use the PC's culture (hu-HU expects 7,5)
            app.UseRequestLocalization("en-US");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            await app.RunAsync();
        }
    }
}
