using System;
using System.Collections.Generic;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public static class Extensions
    {
        public static IEnumerable<ModelStructure> GetHeader(this ModelStructure modelStructure, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > modelStructure.MaximumInnerDepth)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {modelStructure.MaximumInnerDepth}. (input: {rowIndex}");

            if (rowIndex == 0)
                yield return modelStructure;

            foreach (var model in GetHeader(rowIndex, modelStructure.InnerProperties!))
                yield return model;
        }

        private static IEnumerable<ModelStructure> GetHeader(int rowIndex, IEnumerable<ModelStructure> modelStructures)
        {
            foreach (var modelStructure in modelStructures)
            {
                if (modelStructure.CurrentDepth == rowIndex)
                    yield return modelStructure;
                else
                {
                    if (modelStructure.CurrentDepth > rowIndex)
                        continue;

                    if (modelStructure.InnerProperties is null)
                        continue;

                    foreach (var model in GetHeader(rowIndex, modelStructure.InnerProperties))
                        yield return model;
                }
            }
        }
    }
}