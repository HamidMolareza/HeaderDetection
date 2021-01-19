using System;
using System.Collections.Generic;
using System.Linq;

namespace HeaderDetection.Models
{
    public class ModelStructure
    {
        public ModelStructure(string displayName, int numOfColumns, int currentDepth, int maximumInnerDepth,
            List<ModelStructure>? innerProperties)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayName));
            if (numOfColumns < 1)
                throw new ArgumentOutOfRangeException($"Value can not be less than 1. {nameof(numOfColumns)}");
            if (currentDepth < 0)
                throw new ArgumentOutOfRangeException($"Value can not be less than 0. {nameof(currentDepth)}");
            if (maximumInnerDepth < 0)
                throw new ArgumentOutOfRangeException($"Value can not be less than 0. {nameof(maximumInnerDepth)}");
            if (innerProperties is not null && !innerProperties.Any())
                throw new ArgumentException($"{nameof(innerProperties)} can not be empty.");
            if (innerProperties is not null && maximumInnerDepth < 1)
                throw new ArgumentOutOfRangeException(
                    $"{innerProperties} is not null, so value can not be less than 1. {nameof(maximumInnerDepth)}");

            DisplayName = displayName;
            NumOfColumns = numOfColumns;
            CurrentDepth = currentDepth;
            MaximumInnerDepth = maximumInnerDepth;
            InnerProperties = innerProperties;
        }

        public string DisplayName { get; }
        public int NumOfColumns { get; }
        public int CurrentDepth { get; }
        public int MaximumInnerDepth { get; }
        public List<ModelStructure>? InnerProperties { get; }
    }
}