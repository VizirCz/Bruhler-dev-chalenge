using FoodTrucksLocator.Datasets;

namespace FoodTrucksLocator.Requests
{
    public class FoodTruckResultItem
    {
        public string Objectid { get; set; }

        public string Applicant { get; set; }

        public string Address { get; set; }

        public string LocationDescription { get; set; }

        public string[]? FoodItems { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double DistanceInMeters { get; set; }

        public FoodTruckResultItem(FoodTruckDatasetItem datasetItem)
        { 
            this.Objectid = datasetItem.Objectid;
            this.Applicant = datasetItem.Applicant;
            this.Address = datasetItem.Address;
            this.LocationDescription = datasetItem.Locationdescription;
            this.FoodItems = datasetItem.FoodItemsParsed;
            this.Latitude = datasetItem.CoordinatesPrecomputed.Latitude;
            this.Longitude = datasetItem.CoordinatesPrecomputed.Longitude;
        }
    }
}
