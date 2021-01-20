using System;

namespace Test_HeaderDetection.Models
{
    public class SimpleModel
    {
        public int Integer { get; set; }
        public string Str { get; set; }
        public double Decimal { get; set; }

        public override bool Equals(object? obj) =>
            obj is SimpleModel other && Equals(other);

        private bool Equals(SimpleModel other)
        {
            return Integer == other.Integer && Str == other.Str && Decimal.Equals(other.Decimal);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Integer, Str, Decimal);
        }
    }
}