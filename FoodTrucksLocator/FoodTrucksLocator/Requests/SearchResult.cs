namespace FoodTrucksLocator.Requests
{
    public class SearchResult
    {
        public IEnumerable<FoodTruckResultItem> PreferredResults { get; set; }

        public IEnumerable<FoodTruckResultItem> AllResults { get; set; }
    }
}
