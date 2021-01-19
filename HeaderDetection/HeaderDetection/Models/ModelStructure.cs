using System.Collections.Generic;

namespace HeaderDetection.Models
{
    public class ModelStructure
    {
        public ModelStructure(string displayName, int numOfColumns, int currentDepth, int maximumInnerDepth,
            List<ModelStructure>? innerProperties)
        {
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