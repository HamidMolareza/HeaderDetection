using System.Collections.Generic;
using HeaderDetection;
using HeaderDetection.Interfaces;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestExporting
    {
        private const string MergeRowStr = "mergeRow";
        private const string Empty = "Empty";

        [Fact]
        public void AddHeader_SimpleModel_GetHeader()
        {
            var resultHeader = new[]
            {
                new[] {Empty, Empty, Empty},
                new[] {Empty, Empty, Empty},
            };
            var exportingService = new ExportService(resultHeader);
            var exporting = new Exporting(exportingService);

            exporting.AddHeader(Detection.DetectHeader(typeof(SimpleModel)), 0, 0);

            var expected = new[]
            {
                new[] {nameof(SimpleModel), MergeRowStr, MergeRowStr},
                new[] {nameof(SimpleModel.Integer), nameof(SimpleModel.Str), nameof(SimpleModel.Decimal)}
            };

            Assert.True(IsEqual(resultHeader, expected));
        }

        [Fact]
        public void AddHeader_SimpleModelWidthShift_GetHeader()
        {
            var resultHeader = new[]
            {
                new[] {Empty, Empty, Empty, Empty},
                new[] {Empty, Empty, Empty, Empty},
                new[] {Empty, Empty, Empty, Empty},
            };
            var exportingService = new ExportService(resultHeader);
            var exporting = new Exporting(exportingService);

            exporting.AddHeader(Detection.DetectHeader(typeof(SimpleModel)), 1, 1);

            var expected = new[]
            {
                new[] {Empty, Empty, Empty, Empty},
                new[] {Empty, nameof(SimpleModel), MergeRowStr, MergeRowStr},
                new[] {Empty,nameof(SimpleModel.Integer), nameof(SimpleModel.Str), nameof(SimpleModel.Decimal)}
            };

            Assert.True(IsEqual(resultHeader, expected));
        }

        private static bool IsEqual<T>(IReadOnlyList<T[]> array1, IReadOnlyList<T[]> array2)
        {
            if (array1.Count != array2.Count)
                return false;

            for (var i = 0; i < array1.Count; i++)
            {
                if (array1[i].Length != array2[i].Length)
                    return false;
                for (var j = 0; j < array1.Count; j++)
                {
                    if (!array1[i][j].Equals(array2[i][j]))
                        return false;
                }
            }

            return true;
        }
    }

    public class ExportService : IExport
    {
        private readonly string[][] _headerStrings;

        public ExportService(string[][] headerStrings)
        {
            _headerStrings = headerStrings;
        }

        public void InsertText(string text, int row, int column)
        {
            _headerStrings[row][column] = text;
        }

        public void MergeRow(int row, int beginColumn, int endColumn)
        {
            for (var i = beginColumn + 1; i <= endColumn; i++)
            {
                _headerStrings[row][i] = "mergeRow";
            }
        }

        public void MergeColumn(int column, int beginRow, int endRow)
        {
            for (var i = beginRow + 1; i <= endRow; i++)
            {
                _headerStrings[i][column] = "mergeColumn";
            }
        }
    }
}