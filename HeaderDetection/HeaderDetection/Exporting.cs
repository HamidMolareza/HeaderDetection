using System;
using System.Collections.Generic;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public class Exporting
    {
        private readonly IExport _exportService;

        public Exporting(IExport exportService)
        {
            _exportService = exportService;
        }

        public void AddHeader(ModelStructure modelStructure, int beginRow, int beginColumn)
        {
            if (modelStructure is null) throw new ArgumentNullException(nameof(modelStructure));

            _exportService.InsertText(modelStructure.DisplayName, beginRow, beginColumn);
            _exportService.MergeRow(beginColumn, beginRow, beginRow + modelStructure.NumOfColumns - 1);

            if (modelStructure.InnerProperties is not null)
            {
                AddHeader(modelStructure.InnerProperties, beginRow + 1, beginColumn, modelStructure.MaximumInnerDepth);
            }
        }

        private void AddHeader(IEnumerable<ModelStructure> modelStructures, int beginRow, int beginColumn,
            int maximumDepth)
        {
            var column = beginColumn;

            foreach (var modelStructure in modelStructures)
            {
                var row = beginRow;
                _exportService.InsertText(modelStructure.DisplayName, row, column);

                if (modelStructure.NumOfColumns > 1)
                    _exportService.MergeRow(row, column, column + modelStructure.NumOfColumns - 1);

                if (modelStructure.InnerProperties is null)
                {
                    var shortage = maximumDepth - row;
                    if (shortage > 0)
                        _exportService.MergeColumn(column, row, row + shortage);
                }
                else
                {
                    AddHeader(modelStructure.InnerProperties, beginRow + 1, column, maximumDepth);
                }

                column += modelStructure.NumOfColumns;
            }
        }
    }
}