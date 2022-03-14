using FundaAssignment.WebApp;
using FundaAssignment.WebApp.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace FundaAssignment.WebApp.Services
{
    public class MakelaarsService : IMakelaarsService
    {
        private readonly IObjectsService objectsService;
        private readonly IMemoryCache memoryCache;
        private const string MAKELAARS_WITH_MOST_OBJECTS_KEY = "MAKELAARS_WITH_MOST_OBJECTS";
        private const string MAKELAARS_WITH_MOST_OBJECTS_WITH_TUIN_KEY = "MAKELAARS_WITH_MOST_OBJECTS_WITH_TUIN";
        public MakelaarsService(
            IObjectsService objectsService,
            IMemoryCache memoryCache)
        {
            this.objectsService = objectsService;
            this.memoryCache = memoryCache;
        }

        public IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsInAmsterdam(int count)
        {
            var topMakelaars = memoryCache.GetOrCreate(MAKELAARS_WITH_MOST_OBJECTS_KEY + $"_{count}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                var objects = objectsService.GetAmsterdamObjects();
                return TakeTopMakelaarsFromObjects(objects, count);
            });

            return topMakelaars;
        }

        public IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsWithTuinInAmsterdam(int count)
        {
            var topMakelaars = memoryCache.GetOrCreate(MAKELAARS_WITH_MOST_OBJECTS_WITH_TUIN_KEY + $"_{count}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                var objects = objectsService.GetAmsterdamObjectsWithTuin();
                return TakeTopMakelaarsFromObjects(objects, count);
            });

            return topMakelaars;
        }

        private static IEnumerable<MakelaarDto> TakeTopMakelaarsFromObjects(IEnumerable<ObjectDto> objects, int count = 10)
        {
            return objects
                    .GroupBy(o => o.MakelaarId)
                    .OrderByDescending(o => o.Count())
                    .Take(count)
                    .Select(o => new MakelaarDto(o.Key, o.First().MakelaarNaam, o.Count()));
        }
    }
}
