using System.Collections.Generic;

namespace Events
{
    public static class EventManager
    {
        public delegate void EventReciever(params object[] parameter);

        static Dictionary<string, EventReciever> _events = new Dictionary<string, EventReciever>();

        public static void Subscribe(string eventType, EventReciever method)
        {
            if (!_events.ContainsKey(eventType))
                _events.Add(eventType, method);
            else
                _events[eventType] += method;
        }

        public static void UnSubscribe(string eventType, EventReciever method)
        {
            if (_events.ContainsKey(eventType))
            {
                _events[eventType] -= method;

                if (_events[eventType] == null)
                    _events.Remove(eventType);
            }
        }

        public static void Trigger(string eventType, params object[] parameters)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType](parameters);
        }
    }
}
