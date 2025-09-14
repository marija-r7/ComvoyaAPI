using Comvoya.Application.Common.Pagination;
using Comvoya.Application.Models.Country;
using Comvoya.Application.Models.Location;
using Comvoya.Domain.Entities;
using Comvoya.Domain.Helpers;

namespace ComvoyaAPI.Services.LocationService
{
    public interface ILocationService
    {
        Task<IReadOnlyList<CountryDTO>> GetCountriesAsync(CancellationToken ct);
        Task<CountryDetailsDTO?> GetCountryAsync(string iso2, CancellationToken ct);

        Task<PagedResult<LocationSummaryDTO>> GetLocationsByCountryAsync(string iso2, LocationQuery q, CancellationToken ct);

        Task<LocationDetailsDTO?> GetLocationAsync(Guid id, CancellationToken ct);

        Task<PagedResult<LocationSummaryDTO>> SearchLocationsAsync(string? query, string? countryIso2, int page, int pageSize, CancellationToken ct);
    }
}