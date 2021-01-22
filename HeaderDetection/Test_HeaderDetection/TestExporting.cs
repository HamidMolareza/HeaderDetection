using System;
using System.Collections.Generic;
using HeaderDetection;
using HeaderDetection.Interfaces;
using HeaderDetection.Models;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestExporting
    {
        private const string MergeRowStr = "mergeRow";
        private const string MergeColumnStr = "mergeColumn";
        private const string Empty = "Empty";
        private readonly ModelStructure _simpleModelStructure;
        private readonly ModelStructure _complexModelStructure;

        public TestExporting()
        {
            _simpleModelStructure = Detection.DetectHeader(typeof(SimpleModel));
            _complexModelStructure = Detection.DetectHeader(typeof(ComplexModel));
        }


        #region AddHeader

        [Fact]
        public void AddHeader_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Exporting.AddHeader(
                null!, _simpleModelStructure, 0, 0));

            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentNullException>(() => Exporting.AddHeader(
                storageService, null!, 0, 0));
        }

        [Fact]
        public void AddHeader_OutOfRangeValue_ThrowArgumentOutOfRangeException()
        {
            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddHeader(
                storageService, _simpleModelStructure, -1, 0));

            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddHeader(
                storageService, _simpleModelStructure, 0, -1));
        }

        [Fact]
        public void AddHeader_SimpleModel_GetHeader()
        {
            var resultHeader = new[]
            {
                new[] {Empty, Empty, Empty},
                new[] {Empty, Empty, Empty},
            };

            var storageService = new StorageService(resultHeader);
            Exporting.AddHeader(storageService, _simpleModelStructure, 0, 0);

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

            var storageService = new StorageService(resultHeader);
            Exporting.AddHeader(storageService, _simpleModelStructure, 1, 1);

            var expected = new[]
            {
                new[] {Empty, Empty, Empty, Empty},
                new[] {Empty, nameof(SimpleModel), MergeRowStr, MergeRowStr},
                new[] {Empty, nameof(SimpleModel.Integer), nameof(SimpleModel.Str), nameof(SimpleModel.Decimal)}
            };

            Assert.True(IsEqual(resultHeader, expected));
        }

        [Fact]
        public void AddHeader_ComplexModel_GetHeader()
        {
            var resultHeader = new[]
            {
                new[] {Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty},
                new[] {Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty},
                new[] {Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty},
                new[] {Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty},
            };

            var storageService = new StorageService(resultHeader);
            Exporting.AddHeader(storageService, _complexModelStructure, 0, 0);

            var expected = new[]
            {
                new[]
                {
                    nameof(ComplexModel), MergeRowStr, MergeRowStr, MergeRowStr, MergeRowStr, MergeRowStr, MergeRowStr,
                    MergeRowStr
                },
                new[]
                {
                    nameof(ComplexModel.Guid), nameof(ComplexModel.Simple), MergeRowStr, MergeRowStr,
                    nameof(ComplexModel.InnerClass), MergeRowStr, MergeRowStr, MergeRowStr
                },
                new[]
                {
                    MergeColumnStr, nameof(ComplexModel.Simple.Integer), nameof(ComplexModel.Simple.Str),
                    nameof(ComplexModel.Simple.Decimal),
                    Empty, Empty, Empty, Empty
                },
                new[]
                {
                    MergeColumnStr, MergeColumnStr, MergeColumnStr, MergeColumnStr,
                    nameof(ComplexModel.InnerClassObj.Guid), nameof(ComplexModel.InnerClassObj.Simple.Integer),
                    nameof(ComplexModel.InnerClassObj.Simple.Str),
                    nameof(ComplexModel.InnerClassObj.Simple.Decimal)
                },
            };

            Assert.True(IsEqual(resultHeader, expected));
        }

        #endregion

        #region AddItem

        [Fact]
        public void AddItem_Null_ThrowArgumentNullException()
        {
            var model = new SimpleModel();
            Assert.Throws<ArgumentNullException>(() => Exporting.AddItem(model,
                null!, _simpleModelStructure, 0, 0));

            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentNullException>(() => Exporting.AddItem(model,
                storageService, null!, 0, 0));
        }

        [Fact]
        public void AddItem_OutOfRangeValue_ThrowArgumentOutOfRangeException()
        {
            var model = new SimpleModel();
            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddItem(model,
                storageService, _simpleModelStructure, -1, 0));

            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddItem(model,
                storageService, _simpleModelStructure, 0, -1));
        }

        #endregion

        #region AddItems

        [Fact]
        public void AddItems_Null_ThrowArgumentNullException()
        {
            var model = new List<SimpleModel>();
            Assert.Throws<ArgumentNullException>(() => Exporting.AddItems(model,
                null!, _simpleModelStructure, 0, 0));

            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentNullException>(() => Exporting.AddItems(model,
                storageService, null!, 0, 0));
        }

        [Fact]
        public void AddItems_OutOfRangeValue_ThrowArgumentOutOfRangeException()
        {
            var model = new List<SimpleModel>();
            var storageService = new StorageService(null!);
            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddItems(model,
                storageService, _simpleModelStructure, -1, 0));

            Assert.Throws<ArgumentOutOfRangeException>(() => Exporting.AddItems(model,
                storageService, _simpleModelStructure, 0, -1));
        }

        #endregion

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

    public class StorageService : IStorage
    {
        private readonly string[][] _headerStrings;

        public StorageService(string[][] headerStrings)
        {
            _headerStrings = headerStrings;
        }

        public void InsertText(string text, int rowZeroBase, int columnZeroBase)
        {
            _headerStrings[rowZeroBase][columnZeroBase] = text;
        }

        public void Insert(Item item, int rowZeroBase, int columnZeroBase)
        {
            _headerStrings[rowZeroBase][columnZeroBase] = item.Value?.ToString();
        }

        public void MergeRow(int rowZeroBase, int beginColumnZeroBase, int endColumnZeroBase)
        {
            for (var i = beginColumnZeroBase + 1; i <= endColumnZeroBase; i++)
            {
                _headerStrings[rowZeroBase][i] = "mergeRow";
            }
        }

        public void MergeColumn(int columnZeroBase, int beginRowZeroBase, int endRowZeroBase)
        {
            for (var i = beginRowZeroBase + 1; i <= endRowZeroBase; i++)
            {
                _headerStrings[i][columnZeroBase] = "mergeColumn";
            }
        }
    }
}