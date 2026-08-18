using edumis.Models.News;

namespace edumis.DataAccess.IRepositories.INews;

public interface INewsRepo : IRepository<NewsModel>
{
    //Task<bool> UpdateNews(NewsModel news);
}
