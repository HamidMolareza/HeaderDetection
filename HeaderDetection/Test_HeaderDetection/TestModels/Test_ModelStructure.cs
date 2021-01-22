using System;
using System.Collections.Generic;
using HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection.TestModels
{
    public class TestModelStructure
    {
        private const int ValidColumns = 1;
        private const int ValidCurrentDepth = 0;
        private const string ValidName = "valid";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidDisplayName_ThrowArgumentException(string displayName)
        {
            Assert.Throws<ArgumentException>(
                () => new ModelStructure(displayName, ValidColumns, ValidCurrentDepth,
                    0, null, typeof(int), ValidName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_InvalidOriginalName_ThrowArgumentException(string originalName)
        {
            Assert.Throws<ArgumentException>(
                () => new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth,
                    0, null, typeof(int), originalName));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 0, -1)]
        public void Constructor_OutOfRangeInteger_ThrowOutOfRangeException(int numOfColumns, int currentDepth,
            int maximumInnerDepth)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ModelStructure(ValidName, numOfColumns, currentDepth,
                    maximumInnerDepth, null, typeof(int), ValidName));
        }

        [Fact]
        public void Constructor_InnerPropertiesAndOutOfRange_ThrowOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 0,
                    new List<ModelStructure>
                    {
                        new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName)
                    }, typeof(int), ValidName));
        }

        [Fact]
        public void Constructor_EmptyInnerProperties_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 1,
                    new List<ModelStructure>(), typeof(int), ValidName));
        }

        [Fact]
        public void Constructor_ValidInputs_CreateObject()
        {
            var obj = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName);
            Assert.IsType<ModelStructure>(obj);
        }

        [Fact]
        public void Equals_SimpleEqualObjects_ReturnTrue()
        {
            var obj1 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName);
            var obj2 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName);
            Assert.True(Equals(obj1, obj2));
        }

        [Fact]
        public void Equals_SimpleNotEqualObjects_ReturnTrue()
        {
            var obj1 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName);
            var obj2 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 1, null, typeof(int), ValidName);
            var obj3 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth + 1, 0, null, typeof(int),
                ValidName);
            var obj4 = new ModelStructure(ValidName, ValidColumns + 1, ValidCurrentDepth, 0, null, typeof(int),
                ValidName);
            var obj5 = new ModelStructure(ValidName + "a", ValidColumns, ValidCurrentDepth, 0, null, typeof(int),
                ValidName);
            var obj6 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 1,
                new List<ModelStructure> {obj1}, typeof(int), ValidName);

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

        private static ModelStructure GenerateComplexModel() =>
            new(ValidName, ValidColumns, ValidCurrentDepth, 2,
                new List<ModelStructure>
                {
                    new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                    new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                    new(ValidName, ValidColumns, ValidCurrentDepth, 1,
                        new List<ModelStructure>
                        {
                            new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                            new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                        }, typeof(int), ValidName),
                }, typeof(int), ValidName);

        [Fact]
        public void Equals_ComplexNotEqualObjects_ReturnTrue()
        {
            var obj1 = GenerateComplexModel();
            var obj2 = new ModelStructure(ValidName, ValidColumns, ValidCurrentDepth, 2,
                new List<ModelStructure>
                {
                    new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                    new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                    new(ValidName, ValidColumns, ValidCurrentDepth, 1,
                        new List<ModelStructure>
                        {
                            new(ValidName, ValidColumns, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                            new(ValidName, ValidColumns + 5, ValidCurrentDepth, 0, null, typeof(int), ValidName),
                        }, typeof(int), ValidName),
                }, typeof(int), ValidName);

            Assert.False(Equals(obj1, obj2));
        }
    }
}