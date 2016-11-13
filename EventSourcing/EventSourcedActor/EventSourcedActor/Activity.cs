using System.Collections.Generic;

namespace EventSourcedActor
{
    public class Activity
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public List<string> States { get; set; } = new List<string>();
        public string InitialState { get; set; }
        public string CurrentState { get; set; }
        public List<Transition> Transitions { get; set; } = new List<Transition>();
    }
}
