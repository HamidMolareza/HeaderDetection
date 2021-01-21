using System;
using System.Collections.Generic;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public class Table<T>
    {
        private List<T> _items = null!;

        public List<T> Items
        {
            get => _items;
            protected set => _items = value ?? new List<T>();
        }

        public ModelStructure ModelStructure { get; set; }

        public Table()
        {
            Items = new List<T>();
            ModelStructure = Detection.DetectHeader(typeof(T));
        }

        public Table(List<T> items)
        {
            Items = items;
            ModelStructure = Detection.DetectHeader(typeof(T));
        }

        public Table(List<T> items, ModelStructure modelStructure)
        {
            Items = items;
            ModelStructure = modelStructure;
        }

        public T this[int rowIndex]
        {
            get => Items[rowIndex];
            set => Items[rowIndex] = value;
        }

        public IEnumerable<ModelStructure> GetHeader(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > ModelStructure.MaximumInnerDepth)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {ModelStructure.MaximumInnerDepth}. (input: {rowIndex}");

            if (rowIndex == 0)
                yield return ModelStructure;

            foreach (var model in GetHeader(rowIndex, ModelStructure.InnerProperties!))
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

        public IEnumerable<Item> GetItems(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > Items.Count - 1)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {Items.Count - 1}. (input: {rowIndex}");

            var rowItem = Items[rowIndex];
            if (ModelStructure.MaximumInnerDepth == 0)
                yield return new Item(ModelStructure.DisplayName, rowItem, ModelStructure.Type);

            foreach (var item in GetItems(rowItem!, ModelStructure.InnerProperties!))
                yield return item;
        }

        private static IEnumerable<Item> GetItems(object? value, IEnumerable<ModelStructure> modelStructures)
        {
            foreach (var modelStructure in modelStructures)
            {
                object? propertyValue = null;
                if (value is not null)
                    propertyValue = value.GetType().GetProperty(modelStructure.DisplayName)!.GetValue(value);

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