using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Geocoding;
using Geocoding.Microsoft;


namespace Verein.Maps
{
    public class GeoMapService : IGeoService
    {
        private readonly GeoMapOptions _options;

        private readonly ILogger _logger;

        public GeoMapService(IOptions<GeoMapOptions> optionsAccessor, ILogger<GeoMapService> logger)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }

        private async Task<Location> AddressToLocation(string address)
        {
            IGeocoder geocoder = new BingMapsGeocoder(_options.BingMapsKey);
            IEnumerable<Address> addresses = await geocoder.GeocodeAsync(address).ConfigureAwait(false);

            _logger.LogInformation($"Found {addresses.Count()} addresses for {address}");

            if (!addresses.Any())
            {
                return null;
            }
            return addresses.First().Coordinates;
        }


        public async Task<double> GetKmDistance(string currentAddress, string destinationAddress)
        {
            var currentLocation = await AddressToLocation(currentAddress).ConfigureAwait(false);
            var destinationLocation = await AddressToLocation(destinationAddress).ConfigureAwait(false);

            return currentLocation.DistanceBetween(destinationLocation, DistanceUnits.Kilometers);
        }

        public async Task<double> GetKmDistance(Position currentLocation, string destinationAddress)
        {
            var location = new Location(currentLocation.Latitude, currentLocation.Longitude);
            var destinationLocation = await AddressToLocation(destinationAddress).ConfigureAwait(false);

            return location.DistanceBetween(destinationLocation, DistanceUnits.Kilometers);
        }
    }
}

