using CheckSkills.Web.Models;

namespace CheckSkills.Web.Services.Interfaces
{
    public interface IMessageWrapperRepository : IRepositoryBase<MessageWrapper>
    {
        Task<IEnumerable<MessageWrapper>> GetAllMessageWrappersAsync();
        Task<MessageWrapper> GetMessageWrapperByIdAsync(int id);
        Task<MessageWrapper> GetMessageWrapperByEmailAsync(string email);
        void CreateMessageWrapper(MessageWrapper messageWrapper);
        void UpdateMessageWrapper(MessageWrapper messageWrapper);
        void DeleteMessageWrapper(MessageWrapper messageWrapper);
    }
}
