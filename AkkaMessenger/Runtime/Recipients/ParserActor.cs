using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using AkkaMessenger.Runtime.Recipients.Messages;
using CsvHelper;

namespace AkkaMessenger.Runtime.Recipients
{
    class ParserActor : ReceiveActor
    {
        const int BatchSize = 100;
        const int FailAfter = int.MaxValue;

        readonly CancellationTokenSource cancellationSource;
        ILoggingAdapter logger;

        public ParserActor()
        {
            cancellationSource = new CancellationTokenSource();
            Receive(new Action<StartParsing>(OnStartParsing));
        }

        protected override void PreStart()
        {
            base.PreStart();
            logger = Context.GetLogger();
        }

        protected override void PostStop()
        {
            cancellationSource.Dispose();
            base.PostStop(); 
        }

        void OnStartParsing(StartParsing message)
        {
            logger.Debug("Started parsing...");

            var sender = Sender; 
            Task.Factory.StartNew(() => ParseRecipients(message, sender, cancellationSource.Token),
                cancellationSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .PipeTo(Self, sender);
 
            Become(Parsing);
        }

        static CompleteParsing ParseRecipients(StartParsing message, IActorRef sender, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(message.DataPath))))
            {
                var batch = new List<IDictionary<string, string>>(BatchSize);
                int numberOfParsedRecipients = 0;

                while (csvReader.Read())
                {
                    cancellation.ThrowIfCancellationRequested();

                    var recipientValues =
                        csvReader.FieldHeaders.Zip(csvReader.CurrentRecord,
                            (h, v) => new KeyValuePair<string, string>(h, v)).ToDictionary(p => p.Key, p => p.Value);
                    
                    numberOfParsedRecipients++;
                    batch.Add(recipientValues);

                    if (numberOfParsedRecipients == message.RecipientLimit)
                        break;

                    if (numberOfParsedRecipients >= FailAfter)
                        throw new CsvBadDataException("Simulated failure occured.");

                    if (batch.Count < BatchSize)
                        continue;

                    sender.Tell(new ProcessParsedRecipients(batch.ToArray()));
                    batch.Clear();
                }

                cancellation.ThrowIfCancellationRequested();

                if (batch.Count > 0)
                    sender.Tell(new ProcessParsedRecipients(batch.ToArray()));

                return new CompleteParsing(numberOfParsedRecipients);
            }
        }

        void Parsing()
        {
            Receive(new Action<CancelParsing>(OnCancelParsing));
            Receive(new Action<CompleteParsing>(OnCompleteParsing));
            Receive(new Action<Status.Failure>(OnParsingFailure));
            ReceiveAny(Unhandled);
        }

        void OnCancelParsing(CancelParsing _)
        {
           cancellationSource.Cancel(); 
        }

        void OnCompleteParsing(CompleteParsing message)
        {
            Sender.Tell(message);
        }

        void OnParsingFailure(Status.Failure message)
        {
            throw new ParsingFailedException(
                cancellationSource.IsCancellationRequested ? "Parsing cancelled" : "Parsing failed", message.Cause);
        }
    }
}
