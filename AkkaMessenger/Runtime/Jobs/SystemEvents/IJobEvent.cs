namespace AkkaMessenger.Runtime.Jobs.SystemEvents
{
    interface IJobEvent
    {
        JobId JobId { get; }
    }
}