namespace LuckiusDev.Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Static class for managing event bindings and raising events.
    /// </summary>
    /// <typeparam name="T">Type of event.</typeparam>
    public static class EventBus<T> where T : IEvent
    {
        /// <summary>
        /// Collection of event bindings.
        /// </summary>
        private static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        /// <summary>
        /// Registers an event binding.
        /// </summary>
        /// <param name="binding">Event binding to register.</param>
        public static void Register(EventBinding<T> binding) => bindings.Add(binding);

        /// <summary>
        /// Deregisters an event binding.
        /// </summary>
        /// <param name="binding">Event binding to deregister.</param>
        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

        /// <summary>
        /// Raises an event, invoking all registered event bindings.
        /// </summary>
        /// <param name="event">Event to raise.</param>
        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        /// <summary>
        /// Clears all event bindings.
        /// </summary>
        private static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }
    }
}