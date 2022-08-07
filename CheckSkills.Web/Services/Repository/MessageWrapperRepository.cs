using CheckSkills.Web.Models;
using CheckSkills.Web.Services.Interfaces;

namespace CheckSkills.Web.Services.Repository
{
    public class MessageWrapperRepository : RepositoryBase<MessageWrapper>, IMessageWrapperRepository
    {
        public MessageWrapperRepository(DataContext context) : base(context) { }

        public async Task<IEnumerable<MessageWrapper>> GetAllMessageWrappersAsync()
        {
            return await FindAll()
               .ToListAsync();
        }
        public async Task<MessageWrapper> GetMessageWrapperByIdAsync(int Id)
        {
            return await FindByCondition(mw => mw.Id.Equals(Id))
                .FirstOrDefaultAsync();
        }

        public async Task<MessageWrapper> GetMessageWrapperByEmailAsync(string email)
        {
            return await FindByCondition(mw => mw.StudentEmail.Equals(email))
                    .FirstOrDefaultAsync();
        }

        public void CreateMessageWrapper(MessageWrapper messageWrapper)
        {
            Create(messageWrapper);
        }
        public void UpdateMessageWrapper(MessageWrapper messageWrapper)
        {
            Update(messageWrapper);
        }
        public void DeleteMessageWrapper(MessageWrapper messageWrapper)
        {
            Delete(messageWrapper);
        }
    }
}
