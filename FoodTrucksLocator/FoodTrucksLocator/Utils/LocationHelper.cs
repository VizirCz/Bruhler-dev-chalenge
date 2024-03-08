using FoodTrucksLocator.Datasets;
using FoodTrucksLocator.Requests;
using System.Globalization;

namespace FoodTrucksLocator.Utils
{
    public static class LocationHelper
    {
        /// <summary>
        /// Create precomputed coordinates for alghoritm described at https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// </summary>
        /// <param name="item"></param>
        public static CoordinatesPrecomputed CreatePrecomputeCoordinates(string latitude, string longitude)
        {
            return CreatePrecomputeCoordinates(double.Parse(latitude, CultureInfo.InvariantCulture), double.Parse(longitude, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Create precomputed coordinates for alghoritm described at https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// </summary>
        /// <param name="item"></param>
        public static CoordinatesPrecomputed CreatePrecomputeCoordinates(double latitude, double longitude)
        {
            CoordinatesPrecomputed coordinates = new CoordinatesPrecomputed();

            double tmpLatitude = Math.PI * latitude / 180;

            coordinates.Latitude = latitude;
            coordinates.Longitude = longitude;

            coordinates.LatitudeSin = Math.Sin(tmpLatitude);
            coordinates.LatitudeCos = Math.Cos(tmpLatitude);

            return coordinates;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// </summary>
        /// <param name="coordinates1"></param>
        /// <param name="coordinates2"></param>
        /// <returns></returns>
        public static double DistanceInMeters(CoordinatesPrecomputed coordinates1, CoordinatesPrecomputed coordinates2)
        {
            double theta = coordinates1.Longitude - coordinates2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                coordinates1.LatitudeSin * coordinates2.LatitudeSin + coordinates1.LatitudeCos *
                coordinates2.LatitudeCos * Math.Cos(rtheta);
            dist = Math.Acos(dist);

            const double coef = 180.0d / Math.PI * 60 * 1.1515 * 1.609344 * 1000.0;

            return dist * coef;
        }

        public static double DistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1.609344 * 1000.0;
        }
    }
}
