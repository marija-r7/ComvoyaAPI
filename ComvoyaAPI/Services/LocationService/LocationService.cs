using Comvoya.Application.Common.Pagination;
using Comvoya.Application.Models.Country;
using Comvoya.Application.Models.Location;
using Comvoya.Domain.Helpers;
using ComvoyaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComvoyaAPI.Services.LocationService
{
    public class LocationService(AppDbContext context) : ILocationService
    {
        const int MaxPageSize = 100;

        public async Task<IReadOnlyList<CountryDTO>> GetCountriesAsync(CancellationToken ct)
        {
            return await context.Countries
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CountryDTO(c.Iso2, c.Iso3, c.Name, c.Emoji))
                .ToListAsync(ct);
        }

        public async Task<CountryDetailsDTO?> GetCountryAsync(string iso2, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(iso2)) return null;
            iso2 = iso2.Trim().ToUpperInvariant();

            var q = from c in context.Countries.AsNoTracking()
                    where c.Iso2 == iso2
                    select new CountryDetailsDTO(
                        c.Iso2, c.Iso3, c.Name, c.Emoji,
                        context.Locations.Count(l => l.CountryIso2 == c.Iso2)
                    );

            return await q.FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<LocationSummaryDTO>> GetLocationsByCountryAsync(string iso2, LocationQuery q, CancellationToken ct)
        {
            iso2 = iso2.Trim().ToUpperInvariant();
            var page = Math.Max(1, q.Page);
            var pageSize = Math.Clamp(q.PageSize, 1, MaxPageSize);

            var baseQuery = context.Locations.AsNoTracking()
                .Where(l => l.CountryIso2 == iso2);

            if (!string.IsNullOrWhiteSpace(q.Query))
            {
                var term = q.Query.Trim();
                baseQuery = baseQuery.Where(l =>
                    l.Name.Contains(term) ||
                    (l.Admin1 != null && l.Admin1.Contains(term)) ||
                    (l.Admin2 != null && l.Admin2.Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(q.Admin1))
            {
                var a1 = q.Admin1.Trim();
                baseQuery = baseQuery.Where(l => l.Admin1 != null && l.Admin1.Contains(a1));
            }

            var total = await baseQuery.CountAsync(ct);

            var items = await baseQuery
                .OrderBy(l => l.Name)
                .ThenBy(l => l.Admin1)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LocationSummaryDTO(l.Id, l.Name, l.Admin1, l.Admin2, l.CountryIso2))
                .ToListAsync(ct);

            return new PagedResult<LocationSummaryDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<LocationDetailsDTO?> GetLocationAsync(Guid id, CancellationToken ct)
        {
            return await context.Locations
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(l => new LocationDetailsDTO(
                    l.Id, l.Name, l.Admin1, l.Admin2, l.CountryIso2,
                    l.Latitude, l.Longitude, l.Timezone, l.Provider, l.ProviderPlaceId))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<LocationSummaryDTO>> SearchLocationsAsync(string? query, string? countryIso2, int page, int pageSize, CancellationToken ct)
        {
            var q = context.Locations.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryIso2))
            {
                var iso = countryIso2.Trim().ToUpperInvariant();
                q = q.Where(l => l.CountryIso2 == iso);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(l =>
                    l.Name.Contains(term) ||
                    (l.Admin1 != null && l.Admin1.Contains(term)) ||
                    (l.Admin2 != null && l.Admin2.Contains(term)));
            }

            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize <= 0 ? 20 : pageSize, 1, MaxPageSize);

            var total = await q.CountAsync(ct);

            var items = await q
                .OrderBy(l => l.Name).ThenBy(l => l.Admin1)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LocationSummaryDTO(l.Id, l.Name, l.Admin1, l.Admin2, l.CountryIso2))
                .ToListAsync(ct);

            return new PagedResult<LocationSummaryDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }
    }
}
