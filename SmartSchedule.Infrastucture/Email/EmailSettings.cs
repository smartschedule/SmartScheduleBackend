namespace SmartSchedule.Infrastucture.Email
{
    public sealed class EmailSettings
    {
        public string SmtpClient { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
