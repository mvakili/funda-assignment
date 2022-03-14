namespace FundaAssignment.WebApp.Repositories.Contracts;

public interface IObjectsRepository
{
    IEnumerable<ObjectDto> GetObjects(string key);
    void SetObjects(string key, IEnumerable<ObjectDto> objects);
}
