
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Security;

namespace ShopSphere
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DevConnection");


            // Add services to the container.

            builder.Services.AddDbContext<ShopSphereContext>(options =>
                options.UseNpgsql(connString));

            builder.Services.AddSingleton<IEncryptionUtil, EncryptionUtil>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
