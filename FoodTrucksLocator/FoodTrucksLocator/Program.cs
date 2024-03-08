
using FoodTrucksLocator.Datasets;

namespace FoodTrucksLocator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IFoodTruckDatasetSource>(new FoodTruckDatasetSource());

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            await app.Services.GetService<IFoodTruckDatasetSource>().Initialize();

            app.Run();
        }
    }
}
