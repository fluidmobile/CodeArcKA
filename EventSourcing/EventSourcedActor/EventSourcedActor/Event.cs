namespace EventSourcedActor
{
    public class Event
    {
        public string Context { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"{Type}.{Value}";
        }
    }
}
