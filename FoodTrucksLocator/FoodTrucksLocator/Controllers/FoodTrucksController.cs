using FoodTrucksLocator.Datasets;
using FoodTrucksLocator.Requests;
using FoodTrucksLocator.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FoodTrucksLocator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodTrucksController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IFoodTruckDatasetSource foodTruckDatasetSource;

        public FoodTrucksController(IConfiguration configuration, IFoodTruckDatasetSource foodTruckDatasetSource)
        {
            this.configuration = configuration;
            this.foodTruckDatasetSource = foodTruckDatasetSource;
        }

        [HttpGet("Search")]
        public ActionResult<SearchResult> Search(double latitude, double longitude, int amount, string? preferredFood)
        {
            if (amount <= 0)
                return BadRequest($"parameter {nameof(amount)} must be higher ther zero");

            if (latitude < -90 || latitude > 90)
                return BadRequest($"parameter {nameof(latitude)} must be in range [-90,90]");

            if (longitude < -180 || longitude > 180)
                return BadRequest($"parameter {nameof(longitude)} must be in range [-180,180]");

            IEnumerable<FoodTruckDatasetItem> allItems = this.foodTruckDatasetSource.GetItems();
            IEnumerable<FoodTruckDatasetItem> preferedItems = !string.IsNullOrEmpty(preferredFood) ? this.foodTruckDatasetSource.GetItems(preferredFood) : new List<FoodTruckDatasetItem>();

            CoordinatesPrecomputed coordinates = LocationHelper.CreatePrecomputeCoordinates(latitude, longitude);

            SearchResult result = new SearchResult();
            result.AllResults = PickClosestResultItems(allItems, coordinates, amount);
            result.PreferredResults = PickClosestResultItems(preferedItems, coordinates, amount);

            return Ok(result);
        }

        private IEnumerable<FoodTruckResultItem> PickClosestResultItems(IEnumerable<FoodTruckDatasetItem> items, CoordinatesPrecomputed coordinates, int amount)
        {
            List<FoodTruckResultItem> resultItems = new List<FoodTruckResultItem>();

            foreach (FoodTruckDatasetItem item in items)
            {
                FoodTruckResultItem resultItem = new FoodTruckResultItem(item);
                resultItem.DistanceInMeters = LocationHelper.DistanceInMeters(item.CoordinatesPrecomputed, coordinates);
                resultItems.Add(resultItem);
            }

            if (resultItems.Count <= amount)
                return resultItems;

            return resultItems.OrderBy(i => i.DistanceInMeters).Take(amount);
        }
    }
}
