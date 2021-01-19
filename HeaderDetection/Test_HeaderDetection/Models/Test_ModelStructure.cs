using System;
using System.Collections.Generic;
using HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection.Models
{
    public class TestModelStructure
    {
        private const int ValidColumns = 1;
        private const int ValidCurrentDepth = 0;
        private const string ValidDisplayName = "valid";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidDisplayName_ThrowArgumentException(string displayName)
        {
            Assert.Throws<ArgumentException>(
                () => new ModelStructure(displayName, ValidColumns, ValidCurrentDepth,
                    0, null));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 0, -1)]
        public void Constructor_OutOfRangeInteger_ThrowOutOfRangeException(int numOfColumns, int currentDepth,
            int maximumInnerDepth)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ModelStructure(ValidDisplayName, numOfColumns, currentDepth, maximumInnerDepth, null));
        }

        [Fact]
        public void Constructor_InnerPropertiesAndOutOfRange_ThrowOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ModelStructure("valid name", ValidColumns, ValidCurrentDepth, 0,
                    new List<ModelStructure>
                        {new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null)}));
        }
        
        [Fact]
        public void Constructor_EmptyInnerProperties_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 1,
                    new List<ModelStructure>()));
        }
        
        [Fact]
        public void Constructor_ValidInputs_CreateObject()
        {
            var obj = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null);
            Assert.IsType<ModelStructure>(obj);
        }
    }
}