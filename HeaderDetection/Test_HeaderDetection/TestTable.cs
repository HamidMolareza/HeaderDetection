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
        public void Items_SetNull_ReturnEmptyList()
        {
            Assert.True(_simpleTable.Items is not null);
            
            _simpleTable.Items = null!;
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
    }
}