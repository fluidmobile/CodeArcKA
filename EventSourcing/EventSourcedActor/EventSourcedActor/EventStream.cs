using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EventSourcedActor
{
    public class EventStream
    {
        private readonly List<Event> _events = new List<Event>();

        public IEnumerable<Event> Events
        {
            get
            {
                while (_events.Count > 0)
                {
                    var ev = _events[0];
                    _events.RemoveAt(0);
                    yield return ev;
                }
            }
        }

        public void LoadFromDirectory(string path)
        {
            var eventFiles = Directory
                .EnumerateFiles(path, "event-*.json")
                .OrderBy(ev => ev);

            foreach (var eventFile in eventFiles)
            {
                var json = File.ReadAllText(eventFile);
                var ev = JsonConvert.DeserializeObject<Event>(json);
                if (ev == null)
                {
                    Console.WriteLine($"Invalid event({eventFile}): {json}");
                    continue;
                }
                _events.Add(ev);
            }
        }

        public void InsertNext(Event ev)
        {
            _events.Insert(0, ev);
        }
    }
}
