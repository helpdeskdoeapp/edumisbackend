using edumis.DataAccess.IRepositories.IMISC;
using edumis.Models.MISC;

namespace edumis.DataAccess.Repositories.MISC;

internal class SwachhBharatImagesRepo(ApplicationDBContext dBContext) : 
    Repository<SwachhBharatImagesModel>(dBContext), ISwachhBharatImagesRepo
{
}
