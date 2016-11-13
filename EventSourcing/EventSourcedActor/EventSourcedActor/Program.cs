using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventSourcedActor
{
    class Program
    {
        public static List<Activity> Activities = new List<Activity>();
        public static EventStream Events = new EventStream();

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            //var path = Path.GetDirectoryName(typeof(Activity).Assembly.Location) ?? ".";
            var path = "C:\\Users\\Frank\\Dropbox\\CodeArcKA\\EventSourcing\\Gebäude";
            var files = Directory.EnumerateFiles(path, "*.puml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var activity = PumlActivityParser.ParseFile(file);
                Activities.Add(activity);
                Console.WriteLine($"Activity {activity.Name} (*) -> {activity.CurrentState}");
            }
            Console.WriteLine();

            path = "C:\\Users\\Frank\\Dropbox\\CodeArcKA\\EventSourcing\\Events";
            Events.LoadFromDirectory(path);

            foreach (var ev in Events.Events)
            {
                HandleEvent(ev);
                Console.WriteLine();
            }

            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void HandleEvent(Event ev)
        {
            Console.WriteLine($"Event {ev}");
            foreach (var act in Activities)
            {
                HandleEvent(act, ev);
            }
        }

        private static void HandleEvent(Activity activity, Event ev)
        {
            var possibleTransitions = activity.Transitions
                .Where(t => t.From == activity.CurrentState).ToList();
            foreach (var transition in possibleTransitions)
            {
                if (transition.Conditions != null)
                {
                    var allTrue = transition.Conditions
                        .All(cond => cond.IsMatch(activity, ev));
                    if (!allTrue) continue;
                }
                Console.WriteLine($"Activity {activity.Name}: {activity.CurrentState} -> {transition.To}");
                activity.CurrentState = transition.To;
                Events.InsertNext(new Event()
                {
                    Context = activity.Name,
                    Type = "event",
                    Value = transition.To
                });
            }
        }
    }
}
