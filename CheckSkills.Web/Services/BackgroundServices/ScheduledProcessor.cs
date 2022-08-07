using NCrontab;

namespace CheckSkills.Web.Services.BackgroundServices
{
    public abstract class ScheduledProcessor : ScopedProcessor
    {
        private CrontabSchedule? _schedule;
        private DateTime _nextRun;

        protected abstract string? Schedule { get; set; }

        public ScheduledProcessor(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _nextRun = CrontabSchedule.Parse("*/1 * * * *").GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;

                if (now > _nextRun)
                {
                    await Process();

                    _schedule = CrontabSchedule.Parse(Schedule);

                    _nextRun = _schedule.GetNextOccurrence(now);
                }

                await Task.Delay(5000, stoppingToken);

            } while (!stoppingToken.IsCancellationRequested);
        }


    }
}
