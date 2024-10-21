using System;

namespace LuckiusDev.Utils
{
    public static class EventHelper
    {
        public static bool IsSubscribed<T>(T @event, T @delegate) where T : Delegate
        {
            if (@event == null)
                return false;

            var invocations = @event.GetInvocationList();

            foreach (var entry in invocations)
            {
                if (entry.Equals(@delegate))
                    return true;
            }

            return false;
        }
    }
}