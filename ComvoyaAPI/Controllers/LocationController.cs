using Comvoya.Application.Common.Pagination;
using Comvoya.Application.Models.Country;
using Comvoya.Application.Models.Location;
using Comvoya.Domain.Helpers;
using ComvoyaAPI.Services.LocationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace ComvoyaAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class LocationController(ILocationService locationService) : ControllerBase
    {
        [HttpGet("countries")]
        public async Task<ActionResult<IReadOnlyList<CountryDTO>>> GetCountries(CancellationToken ct)
            => Ok(await locationService.GetCountriesAsync(ct));

        [HttpGet("countries/{iso2}")]
        public async Task<ActionResult<CountryDetailsDTO>> GetCountry([FromRoute] string iso2, CancellationToken ct)
        {
            var country = await locationService.GetCountryAsync(iso2, ct);
            return country is null ? NotFound() : Ok(country);
        }

        [HttpGet("countries/{iso2}/locations")]
        public async Task<ActionResult<PagedResult<LocationSummaryDTO>>> GetLocationsByCountry(
            [FromRoute] string iso2,
            [FromQuery] LocationQuery query,
            CancellationToken ct)
        {
            var country = await locationService.GetCountryAsync(iso2, ct);
            if (country is null) return NotFound();

            var page = await locationService.GetLocationsByCountryAsync(iso2, query, ct);
            return Ok(page);
        }

        [HttpGet("locations/{id:guid}")]
        public async Task<ActionResult<LocationDetailsDTO>> GetLocation([FromRoute] Guid id, CancellationToken ct)
        {
            var loc = await locationService.GetLocationAsync(id, ct);
            return loc is null ? NotFound() : Ok(loc);
        }

        [HttpGet("locations/search")]
        public async Task<ActionResult<PagedResult<LocationSummaryDTO>>> SearchLocations(
            [FromQuery] string? query,
            [FromQuery] string? country,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var result = await locationService.SearchLocationsAsync(query, country, page, pageSize, ct);
            return Ok(result);
        }
    }
}
