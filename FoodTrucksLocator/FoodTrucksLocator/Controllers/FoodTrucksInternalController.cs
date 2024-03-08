using FoodTrucksLocator.Datasets;
using Microsoft.AspNetCore.Mvc;

namespace FoodTrucksLocator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodTrucksInternalController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IFoodTruckDatasetSource foodTruckDatasetSource;

        public FoodTrucksInternalController(IConfiguration configuration, IFoodTruckDatasetSource foodTruckDatasetSource)
        {
            this.configuration = configuration;
            this.foodTruckDatasetSource = foodTruckDatasetSource;
        }

        [HttpPost("UpdateData")]
        public async Task<string> UpdateData()
        {
            await this.foodTruckDatasetSource.DownloadDataset(this.configuration);

            return "OK";
        }
    }
}
