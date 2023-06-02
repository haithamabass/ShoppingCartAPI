namespace APICart2.Helpers.EmailSettings
{
    public class EmailConfiguration
    {
        public string FromEmail { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

    }
}
