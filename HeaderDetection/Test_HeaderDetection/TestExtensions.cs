using System;
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
        
        public TestExtensions()
        {
            _simpleModel = Detection.DetectHeader(typeof(SimpleModel));
            _complexModel = Detection.DetectHeader(typeof(ComplexModel));
        }

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
    }
}