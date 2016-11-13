using System.Collections.Generic;

namespace EventSourcedActor
{
    public class Transition
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<Condition> Conditions { get; set; } = new List<Condition>();

        public override string ToString()
        {
            return $"{From} --> {To}";
        }
    }
}