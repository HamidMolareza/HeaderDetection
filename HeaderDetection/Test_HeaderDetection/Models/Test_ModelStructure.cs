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

        [Fact]
        public void Equals_SimpleEqualObjects_ReturnTrue()
        {
            var obj1 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null);
            var obj2 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null);
            Assert.True(Equals(obj1, obj2));
        }

        [Fact]
        public void Equals_SimpleNotEqualObjects_ReturnTrue()
        {
            var obj1 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null);
            var obj2 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 1, null);
            var obj3 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth + 1, 0, null);
            var obj4 = new ModelStructure(ValidDisplayName, ValidColumns + 1, ValidCurrentDepth, 0, null);
            var obj5 = new ModelStructure(ValidDisplayName + "a", ValidColumns, ValidCurrentDepth, 0, null);
            var obj6 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 1,
                new List<ModelStructure> {obj1});
            
            Assert.False(Equals(obj1, obj2));
            Assert.False(Equals(obj1, obj3));
            Assert.False(Equals(obj1, obj4));
            Assert.False(Equals(obj1, obj5));
            Assert.False(Equals(obj1, obj6));
        }

        [Fact]
        public void Equals_ComplexEqualObjects_ReturnTrue()
        {
            var obj1 = GenerateComplexModel();
            var obj2 = GenerateComplexModel();
            
            Assert.True(Equals(obj1, obj2));
        }
        
        private static ModelStructure GenerateComplexModel()=>
            new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 2, 
                new List<ModelStructure>
                {
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 1, 
                        new List<ModelStructure>
                        {
                            new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                            new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                        }),
                });
        
        [Fact]
        public void Equals_ComplexNotEqualObjects_ReturnTrue()
        {
            var obj1 = GenerateComplexModel();
            var obj2 = new ModelStructure(ValidDisplayName, ValidColumns, ValidCurrentDepth, 2, 
                new List<ModelStructure>
                {
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                    new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 1, 
                        new List<ModelStructure>
                        {
                            new(ValidDisplayName, ValidColumns, ValidCurrentDepth, 0, null),
                            new(ValidDisplayName, ValidColumns + 5, ValidCurrentDepth, 0, null),
                        }),
                });
            
            Assert.False(Equals(obj1, obj2));
        }
    }
}