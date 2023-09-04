using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.EventSystem
{
    public class EventManager : Singleton<EventManager>
    {

        private Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

        public void AddListener<T>(Action<T> callback) where T : Event
        {
            if (!typeof(T).IsSubclassOf(typeof(Event))) return;

            Type eventType = typeof(T);

            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Delegate>();
            }
            eventListeners[eventType].Add(callback);
        }

        public void RemoveListener<T>(Action<T> callback) where T : Event
        {
            Type eventType = typeof(T);
            if (!eventListeners.ContainsKey(eventType)) return;
            eventListeners[eventType].Remove(callback);
        }

        public void Raise(Event e)
        {
            Type eventType = e.GetType();
            if (!eventListeners.ContainsKey(eventType)) return;

            List<Delegate> delegates = eventListeners[eventType];
            for (int i = 0; i < delegates.Count; i++)
            {
                Delegate callback = delegates[i];
                if (callback.Method.GetParameters().Length == 1 && callback.Method.GetParameters()[0].ParameterType == eventType)
                {
                    callback.DynamicInvoke(e);
                }
            }
        }
    }
}
