using System;

namespace AnalyticsSystem
{
    public class AnalyticsValue<T> : IAnalyticsValue
    {
        private T _value;
        private readonly object _lock = new object();

        public Type ValueType => typeof(T);

        public AnalyticsValue(T initialValue = default)
        {
            _value = initialValue;
        }

        public object GetValue()
        {
            lock (_lock)
            {
                return _value;
            }
        }

        public void SetValue(object value)
        {
            if (value is T typedValue)
            {
                lock (_lock)
                {
                    _value = typedValue;
                }
            }
            else
            {
                throw new ArgumentException($"Value must be of type {typeof(T)}");
            }
        }

        public void Add(object value)
        {
            if (value is T typedValue)
            {
                lock (_lock)
                {
                    _value = AddInternal(_value, typedValue);
                }
            }
            else
            {
                throw new ArgumentException($"Value must be of type {typeof(T)}");
            }
        }

        public void Subtract(object value)
        {
            if (value is T typedValue)
            {
                lock (_lock)
                {
                    _value = SubtractInternal(_value, typedValue);
                }
            }
            else
            {
                throw new ArgumentException($"Value must be of type {typeof(T)}");
            }
        }

        private T AddInternal(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)((int)(object)a + (int)(object)b);
            if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)a + (float)(object)b);
            if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)a + (double)(object)b);
            if (typeof(T) == typeof(TimeSpan))
                return (T)(object)((TimeSpan)(object)a + (TimeSpan)(object)b);
            throw new NotSupportedException($"Addition not supported for type {typeof(T)}");
        }

        private T SubtractInternal(T a, T b)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)((int)(object)a - (int)(object)b);
            if (typeof(T) == typeof(float))
                return (T)(object)((float)(object)a - (float)(object)b);
            if (typeof(T) == typeof(double))
                return (T)(object)((double)(object)a - (double)(object)b);
            if (typeof(T) == typeof(TimeSpan))
                return (T)(object)((TimeSpan)(object)a - (TimeSpan)(object)b);
            throw new NotSupportedException($"Subtraction not supported for type {typeof(T)}");
        }
    }
}