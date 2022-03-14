using FundaAssignment.WebApp;

namespace FundaAssignment.WebApp.Services.Contracts
{
    public interface IMakelaarsService
    {
        IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsInAmsterdam(int count);
        IEnumerable<MakelaarDto> GetMakelaarsWithMostObjectsWithTuinInAmsterdam(int count);
    }
}