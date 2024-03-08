namespace FoodTrucksLocator.Datasets
{
    public interface IFoodTruckDatasetSource
    {
        public Task Initialize();

        public IEnumerable<FoodTruckDatasetItem> GetItems();

        public IEnumerable<FoodTruckDatasetItem> GetItems(string foodType);

        public Task DownloadDataset(IConfiguration configuration);
    }
}
