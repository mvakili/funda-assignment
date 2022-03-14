using FundaAssignment.WebApp;
using FundaAssignment.WebApp.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace FundaAssignment.WebApp.Services
{
    public class MakelaarsService : IMakelaarsService
    {
        private readonly IObjectsService objectsService;
        public MakelaarsService(IObjectsService objectsService)
        {
            this.objectsService = objectsService;
        }

        public IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsInAmsterdam(int count)
        {
            var objects = objectsService.GetAmsterdamObjects();
            var topMakelaars = TakeTopMakelaarsFromObjects(objects, count);

            return topMakelaars;
        }

        public IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsWithTuinInAmsterdam(int count)
        {
            var objects = objectsService.GetAmsterdamObjectsWithTuin();
            var topMakelaars = TakeTopMakelaarsFromObjects(objects, count);

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
