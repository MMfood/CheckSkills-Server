using NCrontab;

namespace CheckSkills.Web.Services.Interfaces
{
    public interface IBackgroundEmailSenderService
    {
        public string? Schedule { get; set; }

        Task BackgroundSendEmailAsync();
        Task GetCrontabsAsync();
    }
}
