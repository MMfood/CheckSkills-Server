using CheckSkills.Web.Services.BackgroundServices;
using CheckSkills.Web.Services.Interfaces;

namespace CheckSkills.Web.Services.EmailService
{
    public class BackgroundEmailSenderTask : ScheduledProcessor
    {
        public BackgroundEmailSenderTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }

        protected override string? Schedule { get; set; }

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            IBackgroundEmailSenderService backgroundEmailSenderService = scopeServiceProvider.GetRequiredService<IBackgroundEmailSenderService>();

            if(Schedule != null)
            {
                backgroundEmailSenderService.BackgroundSendEmailAsync();

                Schedule = null;
            }

            while (backgroundEmailSenderService.Schedule == null)
            {
                backgroundEmailSenderService.GetCrontabsAsync();
            }

            Schedule = backgroundEmailSenderService.Schedule;

            return Task.CompletedTask;
        }
    }
}
