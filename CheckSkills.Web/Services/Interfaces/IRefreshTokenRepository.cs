using CheckSkills.Web.Models;

namespace CheckSkills.Web.Services.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        void CreateRefreshToken(RefreshToken token);
        void UpdateRefreshToken(RefreshToken token);
        void DeleteRefreshToken(RefreshToken token);
    }
}
