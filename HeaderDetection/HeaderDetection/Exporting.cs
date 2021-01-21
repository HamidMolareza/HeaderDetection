using System;
using System.Collections.Generic;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;

//TODO: Read

namespace HeaderDetection
{
    public static class Exporting
    {
        public static void AddHeader(IStorage storageService, ModelStructure modelStructure, int beginRowZeroBase,
            int beginColumnZeroBase)
        {
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));

            storageService.InsertText(modelStructure.DisplayName, beginRowZeroBase, beginColumnZeroBase);
            storageService.MergeRow(beginColumnZeroBase, beginRowZeroBase,
                beginRowZeroBase + modelStructure.NumOfColumns - 1);

            if (modelStructure.InnerProperties is not null)
                AddHeader(storageService, modelStructure.InnerProperties, beginRowZeroBase + 1, beginColumnZeroBase,
                    modelStructure.MaximumInnerDepth);
        }

        private static void AddHeader(IStorage storageService, IEnumerable<ModelStructure> modelStructures,
            int beginRowZeroBase, int beginColumnZeroBase,
            int maximumDepth)
        {
            var column = beginColumnZeroBase;

            foreach (var modelStructure in modelStructures)
            {
                var row = beginRowZeroBase;
                storageService.InsertText(modelStructure.DisplayName, row, column);

                if (modelStructure.NumOfColumns > 1)
                    storageService.MergeRow(row, column, column + modelStructure.NumOfColumns - 1);

                if (modelStructure.InnerProperties is null)
                {
                    var shortage = maximumDepth - row;
                    if (shortage > 0)
                        storageService.MergeColumn(column, row, row + shortage);
                }
                else
                {
                    AddHeader(storageService, modelStructure.InnerProperties, beginRowZeroBase + 1, column,
                        maximumDepth);
                }

                column += modelStructure.NumOfColumns;
            }
        }

        public static void AddItem<T>(T model, ModelStructure modelStructure, IStorage storageService,
            int beginRowZeroBase, int beginColumnZeroBase)
        {
            var column = beginColumnZeroBase;
            foreach (var item in GetItems(model, modelStructure))
            {
                storageService.Insert(item, beginRowZeroBase, column);
                column++;
            }
        }

        public static void AddItems<T>(IEnumerable<T> items, ModelStructure modelStructure, IStorage storageService,
            int beginRowZeroBase, int beginColumnZeroBase)
        {
            var row = beginRowZeroBase;
            foreach (var item in items)
            {
                AddItem(item, modelStructure, storageService, row, beginColumnZeroBase);
                row++;
            }
        }

        public static IEnumerable<Item> GetItems<T>(T model, ModelStructure modelStructure)
        {
            if (modelStructure.MaximumInnerDepth == 0)
                yield return new Item(modelStructure.DisplayName, model, modelStructure.Type);

            foreach (var item in GetItems(model, modelStructure.InnerProperties!))
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