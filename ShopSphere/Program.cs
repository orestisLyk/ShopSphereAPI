
using CloudinaryDotNet;
using dotenv.net;
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

            

            //Cloudinary configuration
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            // Get the connection string from environment variables
            var connString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");


            // Add services to the container.

            builder.Services.AddDbContext<ShopSphereContext>(options =>
                options.UseNpgsql(connString));

            builder.Services.AddSingleton<IEncryptionUtil, EncryptionUtil>();

            builder.Services.AddControllers();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
