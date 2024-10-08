using System;

namespace AnalyticsSystem
{
    public interface IAnalyticsValue
    {
        Type ValueType { get; }
        object GetValue();
        void SetValue(object value);
        void Add(object value);
        void Subtract(object value);
    }
}