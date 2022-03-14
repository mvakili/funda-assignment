using FundaAssignment.WebApp;

namespace FundaAssignment.WebApp.Services.Contracts
{
    public interface IObjectsService
    {
        Task ReloadAmsterdamObjectsAsync();
        Task ReloadAmsterdamObjectsWithTuinAsync();
        IEnumerable<ObjectDto> GetAmsterdamObjects();
        IEnumerable<ObjectDto> GetAmsterdamObjectsWithTuin();

    }
}
