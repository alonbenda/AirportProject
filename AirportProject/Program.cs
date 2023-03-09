using AirportProject.BL.Airport;
using AirportProject.BL.Controltower;
using AirportProject.BL.Routes;
using AirportProject.Hubs;

namespace AirportProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IAirportLogic, AirportLogic>();
            builder.Services.AddSingleton<IControlTower, ControlTower>();
            builder.Services.AddSingleton<AllRoutes>();

            builder.Services.AddCors(options => options.AddPolicy(name: "AirportPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AirportPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<AirportHub>("/airportHub");

            app.Run();
        }
    }
}