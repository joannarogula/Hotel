namespace WebApplication1;
using Microsoft.EntityFrameworkCore;
using HotelBookingApp.Infrastructure.Data;
using HotelBookingApp.Domain.Services;
using HotelBookingApp.Domain.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using HotelBookingApp.API.Controllers;
using MySql.Data.MySqlClient;



public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        Console.WriteLine("\n WORKS!!!!! \n");
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IUserService, UserService>();

        services.AddMvc()
            .AddControllersAsServices(); 
        
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Inicjalizacja Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

        // Kontynuacja konfiguracji Middleware
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Mapowanie kontrolerów
        });

    }
}