namespace FundaAssignment.WebApp;

public record ObjectDto(Guid Id, int MakelaarId, string MakelaarNaam);
public record PagingDto(int AantalPaginas, int HuidigePagina);
public record PaginatedObjectsDto(IEnumerable<ObjectDto> Objects, PagingDto Paging);