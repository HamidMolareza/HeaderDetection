using System;
using System.Collections.Generic;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public class Table<T>
    {
        private List<T> _items;

        public List<T> Items
        {
            get => _items;
            set => _items = value ?? new List<T>();
        }

        public Table()
        {
            Items = new List<T>();
        }
        
        public Table(List<T> items)
        {
            Items = items;
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