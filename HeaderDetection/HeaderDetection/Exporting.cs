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

        public void AddHeader(IEnumerable<ModelStructure> modelStructures, int beginColumn, int beginRow,
            int maximumDepth)
        {
            if (modelStructures == null) throw new ArgumentNullException(nameof(modelStructures));
            if (maximumDepth <= 0) throw new ArgumentOutOfRangeException(nameof(maximumDepth));

            var column = beginColumn;

            foreach (var modelStructure in modelStructures)
            {
                var row = beginRow;
                _exportService.InsertText(modelStructure.DisplayName, row, column);

                if (modelStructure.NumOfColumns > 1)
                    _exportService.MergeRow(row, column, modelStructure.NumOfColumns - 1);

                if (modelStructure.InnerProperties is null)
                {
                    var shortage = maximumDepth - row;
                    if (shortage > 0)
                        _exportService.MergeColumn(column, row, row + shortage);
                }
                else
                {
                    AddHeader(modelStructure.InnerProperties, column, beginRow + 1, maximumDepth);
                }

                column += modelStructure.NumOfColumns;
            }
        }
    }
}