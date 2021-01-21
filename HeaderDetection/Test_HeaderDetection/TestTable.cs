using System;
using System.Linq;
using HeaderDetection;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestTable
    {
        private readonly Table<SimpleModel> _simpleTable = new();
        private readonly Table<ComplexModel> _complexTable = new();

        [Fact]
        public void Items_ItemsIsNotNull_ReturnEmptyList()
        {
            Assert.True(_simpleTable.Items is not null);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void GetHeader_OutOfRange_ThrowArgumentOutOfRange(int rowIndex)
        {
            Assert.Throws<ArgumentException>(() => _simpleTable.GetHeader(rowIndex).ToList());
        }

        [Fact]
        public void GetHeader_Index0_ReturnFirstModel()
        {
            var result = _simpleTable.GetHeader(0).ToList();

            Assert.True(result.Count == 1);
            Assert.True(result.First().CurrentDepth == 0);
        }

        [Fact]
        public void GetHeader_ValidIndex_ReturnModel()
        {
            var result = _complexTable.GetHeader(1).ToList();
            Assert.True(result.Count == 3);

            foreach (var modelStructure in result)
                Assert.True(modelStructure.CurrentDepth == 1);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void GetItems_OutOfRange_ThrowArgumentOutOfRange(int rowIndex)
        {
            _simpleTable.Items.Add(new SimpleModel());
            Assert.Throws<ArgumentException>(() => _simpleTable.GetItems(rowIndex).ToList());
        }

        [Fact]
        public void GetItems_SimpleModel_ReturnItem()
        {
            var simpleModel = new SimpleModel
            {
                Integer = 2,
                Str = "str",
                Decimal = 1.1,
            };
            _simpleTable.Items.Add(simpleModel);

            var result = _simpleTable.GetItems(0).ToList();
            Assert.True(result.Count == _simpleTable.ModelStructure.NumOfColumns);

            Assert.True(result[0].Name == nameof(SimpleModel.Integer) &&
                        result[0].Type == typeof(int) &&
                        (int) result[0].Value! == simpleModel.Integer);
            Assert.True(result[1].Name == nameof(SimpleModel.Str) &&
                        result[1].Type == typeof(string) &&
                        (string) result[1].Value == simpleModel.Str);
            Assert.True(result[2].Name == nameof(SimpleModel.Decimal) &&
                        result[2].Type == typeof(double) &&
                        Math.Abs((double) result[2].Value! - simpleModel.Decimal) < 0.001);
        }

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
            _complexTable.Items.Add(new ComplexModel
            {
                Guid = guid,
                Simple = simpleModel,
                InnerClassObj = new ComplexModel.InnerClass
                {
                    Guid = guid,
                    Simple = simpleModel
                }
            });

            var result = _complexTable.GetItems(0).ToList();
            Assert.True(result.Count == _complexTable.ModelStructure.NumOfColumns);
        }
    }
}