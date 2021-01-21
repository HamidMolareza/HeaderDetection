using System;
using System.Collections.Generic;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public static class Exporting
    {
        public static void AddHeader(IStorage storageService, ModelStructure modelStructure, int beginRowZeroBase, int beginColumnZeroBase)
        {
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));

            storageService.InsertText(modelStructure.DisplayName, beginRowZeroBase, beginColumnZeroBase);
            storageService.MergeRow(beginColumnZeroBase, beginRowZeroBase, beginRowZeroBase + modelStructure.NumOfColumns - 1);

            if (modelStructure.InnerProperties is not null)
                AddHeader(storageService, modelStructure.InnerProperties, beginRowZeroBase + 1, beginColumnZeroBase, modelStructure.MaximumInnerDepth);
        }

        private static void AddHeader(IStorage storageService, IEnumerable<ModelStructure> modelStructures, int beginRowZeroBase, int beginColumnZeroBase,
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
                    AddHeader(storageService, modelStructure.InnerProperties, beginRowZeroBase + 1, column, maximumDepth);
                }

                column += modelStructure.NumOfColumns;
            }
        }
    }
}