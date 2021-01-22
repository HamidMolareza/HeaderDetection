using System;
using System.Collections.Generic;
using System.Linq;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public static class Extensions
    {
        public static IEnumerable<ModelStructure> GetHeader(this ModelStructure modelStructure, int depthIndex)
        {
            if (depthIndex < 0 || depthIndex > modelStructure.MaximumInnerDepth)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {modelStructure.MaximumInnerDepth}. (input: {depthIndex}");

            if (depthIndex == 0)
                yield return modelStructure;

            foreach (var model in GetHeader(depthIndex, modelStructure.InnerProperties!))
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

        public static IEnumerable<Item> GetItems<T>(this ModelStructure modelStructure, T model)
        {
            if (typeof(T) != modelStructure.Type)
            {
                throw new ArgumentException(
                    $"Type of model and structure are not same. ({typeof(T)} != {modelStructure.Type})");
            }

            if (modelStructure.MaximumInnerDepth == 0)
                yield return new Item(modelStructure.DisplayName, model, modelStructure.Type);

            foreach (var item in GetItems(model, modelStructure.InnerProperties!))
                yield return item;
        }

        public static IEnumerable<Item> GetItems<T>(this ModelStructure modelStructure, IEnumerable<T> models) =>
            models.SelectMany(modelStructure.GetItems);

        private static IEnumerable<Item> GetItems(object? value, IEnumerable<ModelStructure> modelStructures)
        {
            foreach (var modelStructure in modelStructures)
            {
                object? propertyValue = null;
                if (value is not null)
                    propertyValue = value.GetType().GetProperty(modelStructure.OriginalName)!.GetValue(value);

                if (modelStructure.InnerProperties is null)
                {
                    yield return new Item(modelStructure.DisplayName, propertyValue, modelStructure.Type);
                }
                else
                {
                    foreach (var item in GetItems(propertyValue, modelStructure.InnerProperties))
                    {
                        yield return item;
                    }
                }
            }
        }
        
    }
}