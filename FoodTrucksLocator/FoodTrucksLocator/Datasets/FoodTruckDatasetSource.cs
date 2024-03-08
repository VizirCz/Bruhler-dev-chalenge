using FoodTrucksLocator.Extentions;
using System.Collections.Generic;
using System.Text.Json;

namespace FoodTrucksLocator.Datasets
{
    public class FoodTruckDatasetSource : IFoodTruckDatasetSource
    {
        private List<FoodTruckDatasetItem> items;
        private IEnumerable<string> foodTypes;
        private Dictionary<string, List<FoodTruckDatasetItem>> itemsPerFoodType;

        public Task Initialize()
        {
            return LoadItems();
        }

        public IEnumerable<FoodTruckDatasetItem> GetItems()
        {
            return items;
        }

        public IEnumerable<FoodTruckDatasetItem> GetItems(string requiredfoodType)
        {
            List<FoodTruckDatasetItem> results = new List<FoodTruckDatasetItem>();

            foreach (string foodType in this.foodTypes)
            {
                if (foodType.Contains(requiredfoodType, StringComparison.InvariantCultureIgnoreCase))
                    results.AddRange(this.itemsPerFoodType.GetValueOrDefault(foodType) ?? new List<FoodTruckDatasetItem>());
            }

            return results.DistinctBy(i => i.Objectid);
        }

        public async Task DownloadDataset(IConfiguration configuration)
        {
            string? url = configuration.GetSection("Dataset")?.GetValue<string>("SourceDataUrl");

            string rawData = await DownloadData(url);
            this.items = DeserializeDataset(rawData);

            FillMissingData(items);
            InitializeItemsPerType();

            await SaveDataset(items);
        }

        private static async Task<string> DownloadData(string? url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Dataset.SourceDataUrl");

            string rawData;

            try
            {
                HttpClient client = new HttpClient();
                rawData = await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                throw new Exception($"Downloading dataset from {url} failed", ex);
            }

            return rawData;
        }

        private static List<FoodTruckDatasetItem> DeserializeDataset(string rawData)
        {
            List<FoodTruckDatasetItem> items;
            try
            {
                items = JsonSerializer.Deserialize<List<FoodTruckDatasetItem>>(rawData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Deserialization of dataset failed", ex);
            }

            return items;
        }

        private void FillMissingData(List<FoodTruckDatasetItem> items)
        {
            items.AsParallel().ForAll(item => item.FillMissingData());
        }

        private async Task SaveDataset(List<FoodTruckDatasetItem> items)
        {
            string serializedData = JsonSerializer.Serialize(items);
            await File.WriteAllTextAsync(GetItemsFilePath(), serializedData);
        }

        private async Task LoadItems()
        {
            string rawData = await File.ReadAllTextAsync(GetItemsFilePath());
            this.items = DeserializeDataset(rawData);

            InitializeItemsPerType();
        }

        private void InitializeItemsPerType()
        {
            this.itemsPerFoodType = new Dictionary<string, List<FoodTruckDatasetItem>>();

            foreach (FoodTruckDatasetItem item in this.items)
            {
                if (item.FoodItemsParsed == null)
                    continue;

                foreach (string foodItem in item.FoodItemsParsed)
                {
                    if (!this.itemsPerFoodType.TryGetValue(foodItem, out List<FoodTruckDatasetItem>? itemsForType))
                        this.itemsPerFoodType[foodItem] = itemsForType = new List<FoodTruckDatasetItem> { item };

                    itemsForType.Add(item);
                }
            }

            this.foodTypes = this.itemsPerFoodType.Keys;
        }

        private string GetItemsFilePath()
        {
            return Path.Combine(AppContext.BaseDirectory, "SourceData", "Mobile_Food_Facility_Permit_Dataset.json");
        }
    }
}
