using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.Configuration;
using AkkaMessenger.Runtime;

namespace AkkaMessenger
{
    internal static class Program
    { 
        static readonly Config Configuration = ConfigurationFactory.ParseString(
            @"akka {  
                stdout-loglevel = DEBUG
                loglevel = DEBUG
                log-config-on-start = off
                actor {
                            debug {
                                receive = off
                                  autoreceive = off
                                  lifecycle = off
                          event-stream = off
                          unhandled = on
                    }
                }");

        static void Main()
        {
            var system = ActorSystem.Create("messenger-system", Configuration);
            var defaultEventSink = system.ActorOf(Props.Create<DefautEventSinkActor>());
            system.EventStream.Subscribe(defaultEventSink, typeof (object));

            var stopwatch = Stopwatch.StartNew();

            var driver = system.ActorOf(Props.Create<InteractionDriverActor>(), "driver");
            driver.Tell("Do it!");
 
            system.AwaitTermination();
            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadLine();
        }
    }
}