using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using AkkaMessenger.Runtime.Jobs.Messages;
using AkkaMessenger.Runtime.Jobs.SystemEvents;
using AkkaMessenger.Runtime.Recipients;
using AkkaMessenger.Runtime.Recipients.Messages;

namespace AkkaMessenger.Runtime.Jobs
{
    class JobActor : ReceiveActor
    {
        readonly JobId jobId;
        ILoggingAdapter logger;
        IActorRef recipientParser;
        List<JobRecipient> recipients;
        IActorRef recipientValidator;

        public JobActor(JobId jobId)
        {
            this.jobId = jobId;
            Become(AwaitingInitialization);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(e => e is ParsingFailedException ? Directive.Stop : Directive.Restart);
        }

        protected override void PreStart()
        {
            base.PreStart(); 
            logger = Context.GetLogger();
        }
 
        void AwaitingInitialization()
        { 
            Receive(new Action<InitializeRecipients>(OnInitializeRecipients)); 
        }

        void OnInitializeRecipients(InitializeRecipients message)
        {
            logger.Debug("Initializing recipients...");
            recipients = new List<JobRecipient>();

            recipientParser = Context.ActorOf(Props.Create<ParserActor>(), "recipient-parser");
            Context.Watch(recipientParser);
            recipientParser.Tell(new StartParsing(message.RecipientsDataPath, message.RecipientLimit));

            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(5), recipientParser, new CancelParsing(), Self); 
            Become(InitializingRecipents);
        }

        void InitializingRecipents()
        {
            Receive(new Action<Terminated>(OnChildTerminated));
            Receive(new Action<ProcessParsedRecipients>(OnProcessParsedRecipients));
            Receive(new Action<CompleteParsing>(OnCompleteRecipientParsing));
            ReceiveAny(Unhandled);
        }

        void OnChildTerminated(Terminated message)
        {
            if (!message.ActorRef.Equals(recipientParser))
            {
                Unhandled(message);
                return;
            }

            Context.PublishEvent(new RecipientsInitializationFailed(jobId, "Parsing failed"));
            Become(AwaitingInitialization);
        }

        void OnProcessParsedRecipients(ProcessParsedRecipients message)
        {
            logger.Debug($"Storing {message.RecipientValues.Length} parsed recipients");
            recipients.AddRange(message.RecipientValues.Select(v => new JobRecipient(v)));
        }

        void OnCompleteRecipientParsing(CompleteParsing message)
        {
            logger.Debug($"Completing parsing of recipients, total parsed = {message.NumberOfParsedRecipients}");

            Context.Unwatch(recipientParser);
            Context.Stop(recipientParser);
            recipientParser = null;

            Context.PublishEvent(new RecipientsInitialized(jobId));
            Become(Initialized);
        }

        void Initialized()
        {
            Receive(new Action<ValidateRecipients>(OnValidateRecipients));
            ReceiveAny(Unhandled);
        }

        void OnValidateRecipients(ValidateRecipients message)
        {
            recipientValidator = Context.ActorOf(Props.Create<ValidatorActor>().WithRouter(new RoundRobinPool(4)));

            foreach (var recipient in recipients)
                recipientValidator.Tell(recipient.Values);

            // TODO batch recipients
        }
    }
}
