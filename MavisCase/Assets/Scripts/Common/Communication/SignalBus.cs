using System;
using System.Collections.Generic;

namespace MavisCase.Common.Communication
{
    public class SignalBus
    {
        private Dictionary<Type, List<Delegate>> _listeners = new();
        
        public void Subscribe<T>(Action<T> listener) where T : struct
        {
            if (listener == null)
            {
                return;
            }
            
            var key = typeof(T);

            if (!_listeners.TryGetValue(key, out var list))
            {
                list = new List<Delegate>();
                _listeners[key] = list;
            }

            if (!list.Contains(listener))
            {
                list.Add(listener);
            }
        }

        public void Unsubscribe<T>(Action<T> listener) where T : struct
        {
            if (listener == null)
            {
                return;
            }
            
            var key = typeof(T);

            if (!_listeners.TryGetValue(key, out var list))
            {
                return;
            }

            list.Remove(listener);
        }

        public void Fire<T>(T payload) where T : struct
        {
            var key = typeof(T);

            if (!_listeners.TryGetValue(key, out var list) || list.Count == 0)
            {
                return;
            }

            var copy = list.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                if (copy[i] is Action<T> action)
                {
                    try
                    {
                        action(payload);
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogException(ex);
                    }
                }
            }
        }
    }
}
