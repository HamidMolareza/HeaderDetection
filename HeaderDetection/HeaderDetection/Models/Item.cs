using System;

namespace HeaderDetection.Models
{
    public class Item
    {
        public Item(object? value, Type? type)
        {
            Value = value;
            Type = type;
        }

        public object? Value { get; }
        public Type? Type { get; }
    }
}