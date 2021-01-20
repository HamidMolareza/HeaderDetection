using System;
using HeaderDetection;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestTable
    {
        private readonly Table<SimpleModel> _simpleTable = new();

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
            Assert.Throws<ArgumentException>(() => _simpleTable.GetHeader(rowIndex));
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void GetItems_OutOfRange_ThrowArgumentOutOfRange(int rowIndex)
        {
            _simpleTable.Items.Add(new SimpleModel());
            Assert.Throws<ArgumentException>(() => _simpleTable.GetItems(rowIndex));
        }
    }
}