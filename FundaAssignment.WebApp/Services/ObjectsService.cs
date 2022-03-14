using FundaAssignment.WebApp.Repositories.Contracts;
using FundaAssignment.WebApp.Services.Contracts;

namespace FundaAssignment.WebApp.Services
{
    public class ObjectsService : IObjectsService
    {
        private readonly HttpClient httpClient;
        private readonly IObjectsRepository objectsRepository;

        public ObjectsService(
            IHttpClientFactory httpClientFactory,
            IObjectsRepository objectsRepository)
        {
            httpClient = httpClientFactory.CreateClient("funda-api");
            this.objectsRepository = objectsRepository;
        }

        public IEnumerable<ObjectDto> GetAmsterdamObjects()
        {
            return objectsRepository.GetObjects("amsterdam");
        }

        public IEnumerable<ObjectDto> GetAmsterdamObjectsWithTuin()
        {
            return objectsRepository.GetObjects("amsterdam/tuin");
        }

        public async Task ReloadAmsterdamObjectsAsync()
        {
            var zo = "amsterdam";
            var result = await LoadObjectsFromApiAsync(zo);
            objectsRepository.SetObjects(zo, result);
        }

        public async Task ReloadAmsterdamObjectsWithTuinAsync()
        {
            var zo = "amsterdam/tuin";
            var result = await LoadObjectsFromApiAsync(zo);
            objectsRepository.SetObjects(zo, result);
        }

        private async Task<List<ObjectDto>> LoadObjectsFromApiAsync(string zo)
        {
            var result = new List<ObjectDto>();
            var itemsForSale = await LoadObjectsFromApiByPageAsync("koop", zo, 1);
            result.AddRange(itemsForSale.Objects);

            if (itemsForSale.Paging.AantalPaginas > 1)
            {
                var tasks = new List<Task<PaginatedObjectsDto>>();

                for (int i = 2; i <= itemsForSale.Paging.AantalPaginas; i++)
                {
                    int page = i;
                    tasks.Add(LoadObjectsFromApiByPageAsync("koop", zo, page));
                }

                var results = await Task.WhenAll(tasks.ToArray());
                foreach (var item in results)
                    result.AddRange(item.Objects);
            }

            return result;
        }

        private async Task<PaginatedObjectsDto> LoadObjectsFromApiByPageAsync(string type, string zo, int page, int pageSize = 25)
        {
            PaginatedObjectsDto? objects = await httpClient.GetFromJsonAsync<PaginatedObjectsDto>($"?type={type}&zo=/{zo}&page={page}&pagesize={pageSize}");
            return objects;
        }
    }
}
