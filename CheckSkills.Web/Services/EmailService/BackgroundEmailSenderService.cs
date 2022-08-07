using CheckSkills.Web.Services.Interfaces;
using CheckSkills.Web.Models;
using NCrontab;

namespace CheckSkills.Web.Services.EmailService
{
    public class BackgroundEmailSenderService : IBackgroundEmailSenderService
    {
        private readonly IStudentService _studentService;
        private readonly IMessageWrapperRepository _messageWrapperRepository;
        public DateTime currentTime = DateTime.Now;

        public static DateTime oldTime { get; set; }
        public string? Schedule { get; set; }

        public BackgroundEmailSenderService(IStudentService studentService, IMessageWrapperRepository messageWrapperRepository)
        {
            _studentService = studentService;
            _messageWrapperRepository = messageWrapperRepository;
        }

        public async Task BackgroundSendEmailAsync()
        {
            //test

            //await Task.Delay(3000);

            //var poolMessages = new List<string>();
            //poolMessages.Add("15 16 * * *");
            //poolMessages.Add("17 16 * * *");
            //poolMessages.Add("15 16 * * *");
            //poolMessages.Add("16 16 * * *");
            //poolMessages.Add("18 16 * * *");
            //poolMessages.Add("17 16 * * *");

            //var sortPoolMessages = new List<string>();

            var time = oldTime;

            var poolMessages = await _messageWrapperRepository.GetAllMessageWrappersAsync();

            var sortPoolMessages = new List<MessageWrapper>();

            foreach (var message in poolMessages)
            {
                var test = CrontabSchedule.Parse(message.Interval).GetNextOccurrence(time);

                if (test.Minute == currentTime.Minute && test.Hour == currentTime.Hour && test.Day == currentTime.Day && test.Month == currentTime.Month && test.Year == test.Year)
                {
                    sortPoolMessages.Add(message);
                }
            }

            if (sortPoolMessages.Any())
            {
                foreach (var message in sortPoolMessages)
                {
                     await _studentService.SendEmailAsync(
                        "testemailservice@email.com",
                        message + $" old time: {time}, current time: {currentTime}",
                        "notify"
                        );
                }
            }
        }

        public async Task GetCrontabsAsync()
        {
            //test

            //await Task.Delay(3000);

            //var poolMessages = new List<string>();

            //poolMessages.Add("11 16 * * *");
            //poolMessages.Add("15 16 * * *");
            //poolMessages.Add("17 16 * * *");
            //poolMessages.Add("15 16 * * *");
            //poolMessages.Add("16 16 * * *");
            //poolMessages.Add("18 16 * * *");
            //poolMessages.Add("17 16 * * *");

            oldTime = currentTime;

            var poolMessages = await _messageWrapperRepository.GetAllMessageWrappersAsync();

            var poolScheldule = poolMessages
                .OrderBy(m => CrontabSchedule.Parse(m.Interval).GetNextOccurrence(currentTime))
                .ToList();

            Schedule = poolScheldule.First().Interval;
        }
    }
}
