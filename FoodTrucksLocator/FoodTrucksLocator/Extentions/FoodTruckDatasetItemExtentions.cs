using FoodTrucksLocator.Datasets;
using FoodTrucksLocator.Utils;

namespace FoodTrucksLocator.Extentions
{
    public static class FoodTruckDatasetItemExtentions
    {
        /// <summary>
        /// Create precomputed coordinates for alghoritm described at https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// </summary>
        /// <param name="item"></param>
        public static void FillMissingData(this FoodTruckDatasetItem item)
        {
            item.FoodItemsParsed = item.Fooditems?.Split(new char[] { ':', ';' }, StringSplitOptions.TrimEntries);
            item.CoordinatesPrecomputed = LocationHelper.CreatePrecomputeCoordinates(item.Latitude, item.Longitude);
        }
    }
}
