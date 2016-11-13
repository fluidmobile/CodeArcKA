namespace EventSourcedActor
{
    public class Condition
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public bool IsMatch(Activity activity, Event ev)
        {
            if ((Type == "input") && (activity.Name != ev.Context))
                return false;
            return (ev.Type == Type) && (ev.Value == Value);
        }
    }
}