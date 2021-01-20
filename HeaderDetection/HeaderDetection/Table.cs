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
            throw new Exception();
        }

        public IEnumerable<Item> GetItems(int rowIndex)
        {
            throw new Exception();
        }
    }
}