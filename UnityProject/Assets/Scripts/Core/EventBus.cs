using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> subscribers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var key = typeof(T);
            if (!subscribers.TryGetValue(key, out List<Delegate> handlers))
            {
                handlers = new List<Delegate>();
                subscribers.Add(key, handlers);
            }

            if (!handlers.Contains(handler))
            {
                handlers.Add(handler);
            }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null)
            {
                return;
            }

            var key = typeof(T);
            if (!subscribers.TryGetValue(key, out List<Delegate> handlers))
            {
                return;
            }

            handlers.Remove(handler);
            if (handlers.Count == 0)
            {
                subscribers.Remove(key);
            }
        }

        public void Publish<T>(T message)
        {
            if (!subscribers.TryGetValue(typeof(T), out List<Delegate> handlers))
            {
                return;
            }

            for (int i = 0; i < handlers.Count; i++)
            {
                if (handlers[i] is Action<T> action)
                {
                    action.Invoke(message);
                }
            }
        }

        public void Clear()
        {
            subscribers.Clear();
        }
    }
}
