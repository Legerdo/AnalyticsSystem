using System;
using System.Collections.Generic;

namespace AnalyticsSystem
{
    public static class AnalyticsManager
    {
        private static readonly Dictionary<string, IAnalyticsValue> _values = new Dictionary<string, IAnalyticsValue>();
        private static readonly object _dictionaryLock = new object();

        private static void EnsureValueExists<T>(string key, T initialValue = default)
        {
            lock (_dictionaryLock)
            {
                if (!_values.ContainsKey(key))
                {
                    _values[key] = new AnalyticsValue<T>(initialValue);
                }
            }
        }

        public static T GetValue<T>(string key)
        {
            lock (_dictionaryLock)
            {
                if (_values.TryGetValue(key, out var value) && value is AnalyticsValue<T> typedValue)
                {
                    return (T)typedValue.GetValue();
                }
                throw new KeyNotFoundException($"Key '{key}' not found or type mismatch");
            }
        }

        public static void SetValue<T>(string key, T value)
        {
            EnsureValueExists(key, value);
            lock (_dictionaryLock)
            {
                if (_values.TryGetValue(key, out var analyticsValue))
                {
                    analyticsValue.SetValue(value);
                }
            }
        }

        public static void AddValue<T>(string key, T value)
        {
            EnsureValueExists(key, value);
            lock (_dictionaryLock)
            {
                if (_values.TryGetValue(key, out var analyticsValue))
                {
                    analyticsValue.Add(value);
                }
            }
        }

        public static void SubtractValue<T>(string key, T value)
        {
            EnsureValueExists(key, value);
            lock (_dictionaryLock)
            {
                if (_values.TryGetValue(key, out var analyticsValue))
                {
                    analyticsValue.Subtract(value);
                }
            }
        }

        public static Dictionary<string, object> GetAllValues()
        {
            var result = new Dictionary<string, object>();
            lock (_dictionaryLock)
            {
                foreach (var kvp in _values)
                {
                    result[kvp.Key] = kvp.Value.GetValue();
                }
            }
            return result;
        }
    }
}