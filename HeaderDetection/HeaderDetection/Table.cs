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
            set => _items = value ?? new List<T>();
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

        public IEnumerable<ModelStructure> GetHeader(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > ModelStructure.MaximumInnerDepth)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {ModelStructure.MaximumInnerDepth}. (input: {rowIndex}");

            throw new Exception();
        }

        public IEnumerable<Item> GetItems(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > Items.Count - 1)
                throw new ArgumentException(
                    $"Value can not less than 0 or more than {Items.Count - 1}. (input: {rowIndex}");

            throw new Exception();
        }
    }
}