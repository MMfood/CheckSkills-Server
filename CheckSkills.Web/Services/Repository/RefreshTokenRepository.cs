using CheckSkills.Web.Models;
using CheckSkills.Web.Services.Interfaces;

namespace CheckSkills.Web.Services.Repository
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext context) : base(context) { }

        public void CreateRefreshToken(RefreshToken token)
        {
            Create(token);
        }
        public void UpdateRefreshToken(RefreshToken token)
        {
            Update(token);
        }
        public void DeleteRefreshToken(RefreshToken token)
        {
            Delete(token);
        }
    }
}
