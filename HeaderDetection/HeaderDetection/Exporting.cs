using System;
using System.Collections.Generic;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;

//TODO: Read
//TODO: Add XML

namespace HeaderDetection
{
    public static class Exporting
    {
        public static void AddHeader(IStorage storageService, ModelStructure modelStructure, int beginRowZeroBase,
            int beginColumnZeroBase)
        {
            if (storageService == null) throw new ArgumentNullException(nameof(storageService));
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));
            if (beginRowZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginRowZeroBase));
            if (beginColumnZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginColumnZeroBase));

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

        public static void AddItem<T>(T model, IStorage storageService, ModelStructure modelStructure,
            int beginRowZeroBase, int beginColumnZeroBase)
        {
            if (storageService == null) throw new ArgumentNullException(nameof(storageService));
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));
            if (beginRowZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginRowZeroBase));
            if (beginColumnZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginColumnZeroBase));

            var column = beginColumnZeroBase;
            foreach (var item in modelStructure.GetItems(model))
            {
                storageService.Insert(item, beginRowZeroBase, column);
                column++;
            }
        }

        public static void AddItems<T>(IEnumerable<T> items, IStorage storageService, ModelStructure modelStructure,
            int beginRowZeroBase, int beginColumnZeroBase)
        {
            if (storageService == null) throw new ArgumentNullException(nameof(storageService));
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));
            if (beginRowZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginRowZeroBase));
            if (beginColumnZeroBase < 0) throw new ArgumentOutOfRangeException(nameof(beginColumnZeroBase));

            var row = beginRowZeroBase;
            foreach (var item in items)
            {
                AddItem(item, storageService, modelStructure, row, beginColumnZeroBase);
                row++;
            }
        }
    }
}