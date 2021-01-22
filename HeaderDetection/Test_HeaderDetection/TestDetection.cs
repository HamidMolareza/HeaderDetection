using System;
using System.Collections.Generic;
using HeaderDetection;
using HeaderDetection.Models;
using Test_HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestDetection
    {
        [Fact]
        public void DetectHeader_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Detection.DetectHeader(null!));
        }

        [Fact]
        public void DetectHeader_InvalidInput_ThrowArgumentException()
        {
            var obj1 = new List<string>();
            var obj2 = new string[1];

            Assert.Throws<ArgumentException>(() => Detection.DetectHeader(obj1.GetType()));
            Assert.Throws<ArgumentException>(() => Detection.DetectHeader(obj2.GetType()));
        }

        [Fact]
        public void DetectHeader_InvalidSimpleModel_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Detection.DetectHeader(typeof(InvalidSimpleModel)));
        }

        [Fact]
        public void DetectHeader_InvalidComplexModel_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Detection.DetectHeader(typeof(InvalidComplexModel)));
        }

        [Fact]
        public void DetectHeader_SimpleModel_ReturnStructure()
        {
            var result = Detection.DetectHeader(typeof(SimpleModel));
            var expected = new ModelStructure(nameof(SimpleModel), 3, 0, 1,
                new List<ModelStructure>
                {
                    new(nameof(SimpleModel.Integer), 1, 1, 0, null, typeof(int), nameof(SimpleModel.Integer)),
                    new(nameof(SimpleModel.Str), 1, 1, 0, null, typeof(string), nameof(SimpleModel.Str)),
                    new(nameof(SimpleModel.Decimal), 1, 1, 0, null, typeof(double), nameof(SimpleModel.Decimal)),
                }, typeof(SimpleModel), nameof(SimpleModel));

            Assert.True(expected.Equals(result));
        }

        [Fact]
        public void DetectHeader_ComplexModel_ReturnStructure()
        {
            var result = Detection.DetectHeader(typeof(ComplexModel));
            var expected = new ModelStructure(nameof(ComplexModel), 8, 0, 3,
                new List<ModelStructure>
                {
                    new(nameof(ComplexModel.Guid), 1, 1, 0, null, typeof(Guid), nameof(ComplexModel.Guid)),
                    new(nameof(ComplexModel.Simple), 3, 1, 1,
                        new List<ModelStructure>
                        {
                            new(nameof(SimpleModel.Integer), 1, 2, 0, null, typeof(int), nameof(SimpleModel.Integer)),
                            new(nameof(SimpleModel.Str), 1, 2, 0, null, typeof(string), nameof(SimpleModel.Str)),
                            new(nameof(SimpleModel.Decimal), 1, 2, 0, null, typeof(double),
                                nameof(SimpleModel.Decimal)),
                        }, typeof(SimpleModel), nameof(ComplexModel.Simple)),
                    new(nameof(ComplexModel.InnerClassObj), 4, 1, 2,
                        new List<ModelStructure>
                        {
                            new(nameof(ComplexModel.Guid), 1, 2, 0, null, typeof(Guid), nameof(ComplexModel.Guid)),
                            new(nameof(ComplexModel.Simple), 3, 2, 1,
                                new List<ModelStructure>
                                {
                                    new(nameof(SimpleModel.Integer), 1, 3, 0, null, typeof(int),
                                        nameof(SimpleModel.Integer)),
                                    new(nameof(SimpleModel.Str), 1, 3, 0, null, typeof(string),
                                        nameof(SimpleModel.Str)),
                                    new(nameof(SimpleModel.Decimal), 1, 3, 0, null, typeof(double),
                                        nameof(SimpleModel.Decimal)),
                                }, typeof(SimpleModel), nameof(ComplexModel.Simple)),
                        }, typeof(ComplexModel.InnerClass), nameof(ComplexModel.InnerClassObj))
                }, typeof(ComplexModel), nameof(ComplexModel));

            Assert.True(expected.Equals(result));
        }

        [Fact]
        public void DetectHeader_RecursiveModel_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Detection.DetectHeader(typeof(RecursiveModel)));
        }

        [Fact]
        public void DetectHeader_DisplayNameModel_ReturnDisplayName()
        {
            var result = Detection.DetectHeader(typeof(DisplayNameModel));
            var expected = new ModelStructure("main", 1, 0, 1,
                new List<ModelStructure>
                {
                    new("Inner Property", 1, 1, 0, null, typeof(string), nameof(DisplayNameModel.Property))
                }, typeof(DisplayNameModel), nameof(DisplayNameModel));

            Assert.True(expected.Equals(result));
        }

        [Fact]
        public void IsValidType_InvalidTypes_ReturnFalse()
        {
            var invalidType1 = new string[1]; //Array
            var invalidType2 = new List<int>(); //List

            Assert.False(Detection.IsValidType(invalidType1.GetType()));
            Assert.False(Detection.IsValidType(invalidType2.GetType()));
        }
    }

    public class InvalidSimpleModel
    {
        public int Integer { get; set; }
        public string Str { get; set; }
        public List<double> Decimals { get; set; }
    }

    public class InvalidComplexModel
    {
        public Guid Guid { get; set; }
        public SimpleModel Simple { get; set; }
        public List<InnerClass> InnerClassObj { get; set; }

        public class InnerClass
        {
            public Guid Guid { get; set; }
            public SimpleModel Simple { get; set; }
        }
    }

    public class RecursiveModel
    {
        public string Id { get; set; }
        public RecursiveModel InnerModel { get; set; }
    }
}