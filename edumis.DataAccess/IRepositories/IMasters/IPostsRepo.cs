using edumis.Models.Masters;
using edumis.Models.Masters.DTO;

namespace edumis.DataAccess.IRepositories.IMasters
{
    public interface IPostsRepo : IRepository<PostsModel>
    {
        Task<bool> Update(PostsDTO PostDetails);
    }
}
