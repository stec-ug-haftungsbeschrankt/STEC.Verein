using System;
using System.Threading.Tasks;

namespace Verein.Maps
{
    public interface IGeoService
    {
        Task<double> GetKmDistance(string currentAddress, string destinationAddress);

        Task<double> GetKmDistance(Position currentLocation, string destinationAddress);
    }
}