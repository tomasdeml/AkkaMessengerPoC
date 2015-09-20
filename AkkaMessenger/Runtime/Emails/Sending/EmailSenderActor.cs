using System;
using System.IO;
using Akka.Actor;

namespace AkkaMessenger.Runtime.Emails.Sending
{
    class EmailSenderActor : ReceiveActor
    {
        private readonly Lazy<StreamWriter> sentEmails;
        private readonly string fileName;

        public EmailSenderActor()
        {
            fileName = $"__EMAILS_{Self.Path.Name.Replace('$', 'x')}.log";
            sentEmails =
                new Lazy<StreamWriter>(
                    () =>
                        new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)));

            Receive<Email>(email =>
            {
                //sentEmails.Value.WriteLine($"TO={email.ToAddress}; SUBJECT={email.Subject}");
            });
        }

        protected override void PostStop()
        {
            base.PostStop();

            if (!sentEmails.IsValueCreated) return;
            Console.WriteLine($"Stopping email sender {Self.Path.Name}");
            sentEmails.Value.Dispose();
        }
    }
}