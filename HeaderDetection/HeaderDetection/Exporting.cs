using System;
using System.Collections.Generic;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public static class Exporting
    {
        public static void AddHeader(IStorage storageService, ModelStructure modelStructure, int beginRow, int beginColumn)
        {
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));

            storageService.InsertText(modelStructure.DisplayName, beginRow, beginColumn);
            storageService.MergeRow(beginColumn, beginRow, beginRow + modelStructure.NumOfColumns - 1);

            if (modelStructure.InnerProperties is not null)
                AddHeader(storageService, modelStructure.InnerProperties, beginRow + 1, beginColumn, modelStructure.MaximumInnerDepth);
        }

        private static void AddHeader(IStorage storageService, IEnumerable<ModelStructure> modelStructures, int beginRow, int beginColumn,
            int maximumDepth)
        {
            var column = beginColumn;

            foreach (var modelStructure in modelStructures)
            {
                var row = beginRow;
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
                    AddHeader(storageService, modelStructure.InnerProperties, beginRow + 1, column, maximumDepth);
                }

                column += modelStructure.NumOfColumns;
            }
        }
    }
}