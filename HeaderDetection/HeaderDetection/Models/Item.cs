using System;

namespace HeaderDetection.Models
{
    public class Item
    {
        public Item(string name, object? value, Type? type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
            Type = type;
        }

        public string Name { get; }
        public object? Value { get; }
        public Type? Type { get; }
    }
}