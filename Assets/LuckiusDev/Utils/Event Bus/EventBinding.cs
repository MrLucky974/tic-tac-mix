namespace LuckiusDev.Utils
{
    using System;

    /// <summary>
    /// Interface for event bindings with and without arguments.
    /// </summary>
    /// <typeparam name="T">Type of event argument.</typeparam>
    public interface IEventBinding<T>
    {
        /// <summary>
        /// Action to be executed when the event occurs with arguments.
        /// </summary>
        public Action<T> OnEvent { get; set; }

        /// <summary>
        /// Action to be executed when the event occurs without arguments.
        /// </summary>
        public Action OnEventNoArgs { get; set; }
    }

    /// <summary>
    /// Implementation of the event binding interface for events with and without arguments.
    /// </summary>
    /// <typeparam name="T">Type of event argument.</typeparam>
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        private Action<T> onEvent = _ => { };
        private Action onEventNoArgs = () => { };

        /// <summary>
        /// Action to be executed when the event occurs with arguments.
        /// </summary>
        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }

        /// <summary>
        /// Action to be executed when the event occurs without arguments.
        /// </summary>
        Action IEventBinding<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }

        /// <summary>
        /// Constructor to initialize the event binding with an action for events with arguments.
        /// </summary>
        /// <param name="onEvent">Action to be executed when the event occurs with arguments.</param>
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

        /// <summary>
        /// Constructor to initialize the event binding with an action for events without arguments.
        /// </summary>
        /// <param name="onEventNoArgs">Action to be executed when the event occurs without arguments.</param>
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        /// <summary>
        /// Adds an action to be executed when the event occurs without arguments.
        /// </summary>
        /// <param name="onEvent">Action to be added.</param>
        public void Add(Action onEvent) => onEventNoArgs += onEvent;

        /// <summary>
        /// Removes an action from the event's execution when it occurs without arguments.
        /// </summary>
        /// <param name="onEvent">Action to be removed.</param>
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

        /// <summary>
        /// Adds an action to be executed when the event occurs with arguments.
        /// </summary>
        /// <param name="onEvent">Action to be added.</param>
        public void Add(Action<T> onEvent) => this.onEvent += onEvent;

        /// <summary>
        /// Removes an action from the event's execution when it occurs with arguments.
        /// </summary>
        /// <param name="onEvent">Action to be removed.</param>
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
    }
}