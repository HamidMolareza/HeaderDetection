using System;
using System.Collections.Generic;
using HeaderDetection;
using HeaderDetection.Models;
using Test_HeaderDetection.Models;
using Test_HeaderDetection.Services;
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
        private readonly ModelStructure _displayNameStructure;

        public TestExporting()
        {
            _simpleModelStructure = Detection.DetectHeader(typeof(SimpleModel));
            _complexModelStructure = Detection.DetectHeader(typeof(ComplexModel));
            _displayNameStructure = Detection.DetectHeader(typeof(DisplayNameModel));
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
        public void AddHeader_DisplayNameModel_GetHeader()
        {
            var resultHeader = new[]
            {
                new[] {Empty},
                new[] {Empty},
            };

            var storageService = new StorageService(resultHeader);
            Exporting.AddHeader(storageService, _displayNameStructure, 0, 0);

            var expected = new[]
            {
                new[] {"main"},
                new[] {"Inner Property"}
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
                    nameof(ComplexModel.InnerClassObj), MergeRowStr, MergeRowStr, MergeRowStr
                },
                new[]
                {
                    MergeColumnStr, nameof(ComplexModel.Simple.Integer), nameof(ComplexModel.Simple.Str),
                    nameof(ComplexModel.Simple.Decimal),
                    nameof(ComplexModel.InnerClassObj.Guid), nameof(ComplexModel.InnerClassObj.Simple), MergeRowStr, MergeRowStr
                },
                new[]
                {
                    MergeColumnStr, MergeColumnStr, MergeColumnStr, MergeColumnStr,
                    MergeColumnStr, nameof(ComplexModel.InnerClassObj.Simple.Integer),
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

        [Fact]
        public void AddItem_DisplayNameModel_GetHeader()
        {
            var model = new DisplayNameModel {Property = "value"};
            var result = new[]
            {
                new[] {Empty}
            };
            var storageService = new StorageService(result);

            Exporting.AddItem(model, storageService, _displayNameStructure, 0, 0);

            var expected = new[]
            {
                new[] {"value"},
            };

            Assert.True(IsEqual(result, expected));
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

        [Fact]
        public void AddItems_DisplayNameModel_GetHeader()
        {
            var model = new DisplayNameModel {Property = "value"};
            var list = new List<DisplayNameModel> {model, model};
            var result = new[]
            {
                new[] {Empty},
                new[] {Empty},
            };
            var storageService = new StorageService(result);

            Exporting.AddItems(list, storageService, _displayNameStructure, 0, 0);

            var expected = new[]
            {
                new[] {"value"},
                new[] {"value"},
            };

            Assert.True(IsEqual(result, expected));
        }

        #endregion

        private static bool IsEqual<T>(IReadOnlyList<T[]> array1, IReadOnlyList<T[]> array2)
        {
            if (array1.Count != array2.Count)
                return false;

            for (var r = 0; r < array1.Count; r++)
            {
                if (array1[r].Length != array2[r].Length)
                    return false;

                for (var c = 0; c < array1[r].Length; c++)
                {
                    if (!array1[r][c].Equals(array2[r][c]))
                        return false;
                }
            }

            return true;
        }
    }
}