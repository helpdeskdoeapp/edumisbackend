using edumis.DataAccess.IRepositories.INews;
using edumis.Models.News;

namespace edumis.DataAccess.Repositories.News;

internal class NewsRepo : Repository<NewsModel>, INewsRepo
{
    private readonly ApplicationDBContext dBContext;
    public NewsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    //public async Task<bool> UpdateNews(NewsModel news)
    //{
    //    var newsId = new NpgsqlParameter("p_id", NpgsqlDbType.Bigint) { Value = news.RowId };
    //    var newsTitle = new NpgsqlParameter("p_title", NpgsqlDbType.Varchar) { Value = news.Title };
    //    var newsDescription = new NpgsqlParameter("p_description", NpgsqlDbType.Varchar) { Value = news.Description };
    //    var newsPhoto = new NpgsqlParameter("p_photo", NpgsqlDbType.Varchar) { Value = news.Photo };
    //    var newsVideoLink = new NpgsqlParameter("p_video_link", NpgsqlDbType.Varchar) { Value = news.VideoLink };
    //    var newsExpiryDate = new NpgsqlParameter("p_expirydate", NpgsqlDbType.Varchar) { Value = news.NewsDate };
    //    var newsExternalLink = new NpgsqlParameter("p_external_link", NpgsqlDbType.Varchar) { Value = news.ExternalLink };
    //    var newsModifiedBy = new NpgsqlParameter("p_modifiedby", NpgsqlDbType.Varchar) { Value = news.ModifiedBy };

    //    await dBContext.Database.ExecuteSqlRawAsync(
    //        "CALL spms_newsupdate(@p_id,@p_title, @p_description, @p_photo, @p_video_link, @p_external_link, @p_expirydate, @p_modifiedby)",
    //        newsId, newsTitle, newsDescription, newsPhoto, newsVideoLink, newsExternalLink, newsExpiryDate, newsModifiedBy
    //    );

    //    return true;
    //}

}
