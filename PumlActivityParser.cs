using System;
using System.IO;
using System.Linq;

namespace EventSourcedActor
{
    public class PumlActivityParser
    {
        public static Activity ParseFile(string fileName)
        {
            var activity = new Activity
            {
                Name = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(fileName)),
                Parent = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(Path.GetDirectoryName(fileName))),
            };

            var lines = File.ReadAllLines(fileName);
            var state = string.Empty;
            foreach (var line in lines)
            {
                var parts = line.Split(new [] { "-->" }, StringSplitOptions.None);
                if(parts.Length < 2) continue;

                var from = parts[0].Trim();
                if (!string.IsNullOrEmpty(from) && !activity.States.Contains(from))
                {
                    activity.States.Add(RemoveDecoration(from));
                }
                if (string.IsNullOrEmpty(from))
                {
                    from = state;
                }
                var to = parts[1].Trim();
                string rules = null;
                if (to.StartsWith("["))
                {
                    var end = to.IndexOf("]", StringComparison.Ordinal);
                    rules = to.Substring(1, end - 1);
                    to = to.Substring(end + 1).Trim();
                }
                if (!activity.States.Contains(to))
                {
                    activity.States.Add(RemoveDecoration(to));
                }
                activity.Transitions.Add(new Transition
                {
                    From = RemoveDecoration(from),
                    To = RemoveDecoration(to),
                    Conditions = rules?
                        .Replace("\\n", ";").Split(';')
                        .Select(cond => new Condition { Type = cond.Split('.')[0], Value = cond.Split('.')[1] })
                        .ToList()
                });
                if (from == "(*)")
                {
                    activity.InitialState = to;
                    activity.CurrentState = to;
                }
                state = to;
            }
            return activity;
        }

        private static string RemoveDecoration(string state)
        {
            if (state.StartsWith("\"")) state = state.Substring(1);
            if (state.EndsWith("\"")) state = state.Substring(0, state.Length-1);
            if (state.Contains("\\n"))
            {
                state = state.Split(new[] {"\\n"}, StringSplitOptions.None)[1].Trim();
            }
            return state;
        }

    }
}