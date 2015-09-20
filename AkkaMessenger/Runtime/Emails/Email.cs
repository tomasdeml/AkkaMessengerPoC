namespace AkkaMessenger.Runtime.Emails
{
    class Email
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }

        public Email(string toAddress, string subject)
        {
            ToAddress = toAddress;
            Subject = subject;
        }
    }
}