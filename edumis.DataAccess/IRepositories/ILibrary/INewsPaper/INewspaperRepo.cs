using edumis.Models.Library.Newspaper;
using edumis.Models.Library.Newspaper.DTO;

namespace edumis.DataAccess.IRepositories.ILibrary.INewsPaper;

public interface INewspaperRepo : IRepository<NewspaperModel>
{
    Task<NewspaperDetailsResponseDTO?> GetDetails(Guid recordId);
    Task<IEnumerable<NewspaperDetailsResponseDTO>?> GetNewspapers(string branchId);
}
