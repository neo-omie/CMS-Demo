using CMS.Application;
using CMS.Application.Models.Identity;
using CMS.Identity;

namespace CMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(); // For Auth
            builder.Services.AddCors(); // For Angular Frontend joining

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()); // For Angular Frontend joining
            app.UseAuthentication(); // For Auth
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
