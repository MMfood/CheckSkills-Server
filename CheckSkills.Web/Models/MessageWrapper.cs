namespace CheckSkills.Web.Models
{
    public class MessageWrapper
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Interval { get; set; }
        public DateTime EndDate { get; set; }
        public int StudentId { get; set; }
        public string StudentEmail { get; set; }
        public Student Student { get; set; }
    }
}
