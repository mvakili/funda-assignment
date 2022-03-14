using FundaAssignment.WebApp.Repositories.Contracts;

namespace FundaAssignment.WebApp.Repositories;

public class ObjectsRepository : IObjectsRepository
{
    private Dictionary<string, IEnumerable<ObjectDto>> Objects { get; set; } = new();

    public void SetObjects(string key, IEnumerable<ObjectDto> objects)
    {
        Objects[key] = objects;
    }
    public IEnumerable<ObjectDto> GetObjects(string key)
    {
        return Objects[key];
    }

}
