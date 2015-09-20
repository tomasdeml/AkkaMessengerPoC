using System;
using System.IO;
using Akka.Actor;

namespace AkkaMessenger
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
                Console.WriteLine($"Sending email to {email.ToAddress}"); 
                sentEmails.Value.WriteLine($"TO={email.ToAddress}; SUBJECT={email.Subject}");
            });
        }

        protected override void PostStop()
        {
            base.PostStop();
            sentEmails.Value.Dispose();
        }
    }
}