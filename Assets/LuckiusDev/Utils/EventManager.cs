using System;
using System.Collections.Generic;

namespace LuckiusDev.Utils
{
    public static class EventManager<TEventArgs>
    {
        private static readonly Dictionary<string, Action<TEventArgs>> Events = new Dictionary<string, Action<TEventArgs>>();

        public static void Register(string name, Action<TEventArgs> handler)
        {
            if (!Events.ContainsKey(name))
            {
                Events[name] = handler;
            }
            else
            {
                Events[name] += handler;
            }
        }

        public static void Unregister(string name, Action<TEventArgs> handler)
        {
            if (Events.ContainsKey(name))
            {
                Events[name] -= handler;
            }
        }

        public static void Trigger(string name, TEventArgs args)
        {
            if (Events.ContainsKey(name))
            {
                Events[name]?.Invoke(args);
            }
        }
    }
}
