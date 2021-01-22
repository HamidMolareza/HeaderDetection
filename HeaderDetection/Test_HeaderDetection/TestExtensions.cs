using System;
using System.Collections.Generic;
using System.Linq;
using HeaderDetection;
using HeaderDetection.Models;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestExtensions
    {
        private readonly ModelStructure _simpleModel;
        private readonly ModelStructure _complexModel;
        private readonly ModelStructure _displayNameModel;

        public TestExtensions()
        {
            _simpleModel = Detection.DetectHeader(typeof(SimpleModel));
            _complexModel = Detection.DetectHeader(typeof(ComplexModel));
            _displayNameModel = Detection.DetectHeader(typeof(DisplayNameModel));
        }

        #region GetHeader

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void GetHeader_OutOfRange_ThrowArgumentOutOfRange(int rowIndex)
        {
            Assert.Throws<ArgumentException>(() => _simpleModel.GetHeader(rowIndex).ToList());
        }

        [Fact]
        public void GetHeader_Index0_ReturnFirstModel()
        {
            var result = _simpleModel.GetHeader(0).ToList();

            Assert.True(result.Count == 1);
            Assert.True(result.First().CurrentDepth == 0);
        }

        [Fact]
        public void GetHeader_ValidIndex_ReturnModel()
        {
            var result = _complexModel.GetHeader(1).ToList();
            Assert.True(result.Count == 3);

            foreach (var modelStructure in result)
                Assert.True(modelStructure.CurrentDepth == 1);
        }

        [Fact]
        public void GetHeader_ValidIndexDisplayName_ReturnModel()
        {
            var result = _displayNameModel.GetHeader(0).ToList();
            Assert.True(result.Count == 1);

            foreach (var modelStructure in result)
                Assert.True(modelStructure.CurrentDepth == 0);
        }

        #endregion

        #region GetItems

        [Fact]
        public void GetItems_MismatchType_ThrowArgumentException()
        {
            var model = new ComplexModel();
            Assert.Throws<ArgumentException>(() => _simpleModel.GetItems(model).ToList());
        }

        [Fact]
        public void GetItems_SimpleModel_ReturnItems()
        {
            var simpleModel = new SimpleModel
            {
                Integer = 2,
                Str = "str",
                Decimal = 1.1,
            };

            var result = _simpleModel.GetItems(simpleModel).ToList();
            Assert.True(IsSimpleModelValid(simpleModel, result));
        }

        [Fact]
        public void GetItems_DisplayNameModel_ReturnItems()
        {
            var simpleModel = new DisplayNameModel {Property = "value"};

            var result = _displayNameModel.GetItems(simpleModel).ToList();
            Assert.True(result.Count == _displayNameModel.NumOfColumns &&
                        result[0].Name == "Inner Property" && result[0].Type == typeof(string) &&
                        (string) result[0].Value == "value");
        }

        [Fact]
        public void GetItems_ListOfSimpleModel_ReturnItems()
        {
            var simpleModel = new SimpleModel
            {
                Integer = 2,
                Str = "str",
                Decimal = 1.1,
            };
            var list = new List<SimpleModel> {simpleModel, simpleModel};

            var result = _simpleModel.GetItems<SimpleModel>(list).ToList();
            Assert.True(IsSimpleModelValid(simpleModel, result.GetRange(0, _simpleModel.NumOfColumns)));
            Assert.True(IsSimpleModelValid(simpleModel,
                result.GetRange(_simpleModel.NumOfColumns, result.Count - _simpleModel.NumOfColumns)));
        }

        private bool IsSimpleModelValid(SimpleModel simpleModel, IReadOnlyList<Item> items) =>
            items.Count == _simpleModel.NumOfColumns && items[0].Name == nameof(SimpleModel.Integer) &&
            items[0].Type == typeof(int) && (int) items[0].Value! == simpleModel.Integer &&
            items[1].Name == nameof(SimpleModel.Str) && items[1].Type == typeof(string) &&
            (string) items[1].Value == simpleModel.Str && items[2].Name == nameof(SimpleModel.Decimal) &&
            items[2].Type == typeof(double) && Math.Abs((double) items[2].Value! - simpleModel.Decimal) < 0.001;

        [Fact]
        public void GetItems_ComplexModel_ReturnItem()
        {
            var guid = Guid.NewGuid();
            var simpleModel = new SimpleModel
            {
                Decimal = 1.1,
                Integer = 2,
                Str = "str"
            };
            var complexModel = new ComplexModel
            {
                Guid = guid,
                Simple = simpleModel,
                InnerClassObj = new ComplexModel.InnerClass
                {
                    Guid = guid,
                    Simple = simpleModel
                }
            };

            var result = _complexModel.GetItems(complexModel).ToList();
            Assert.True(result.Count == _complexModel.NumOfColumns);
        }

        #endregion
    }
}