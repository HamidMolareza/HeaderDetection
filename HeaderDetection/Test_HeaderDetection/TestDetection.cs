using System;
using System.Collections.Generic;
using HeaderDetection;
using HeaderDetection.Models;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestDetection
    {
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
                    new(nameof(SimpleModel.Integer), 1, 1, 0, null),
                    new(nameof(SimpleModel.Str), 1, 1, 0, null),
                    new(nameof(SimpleModel.Decimal), 1, 1, 0, null),
                });

            Assert.True(Equals(result, expected));
        }
        
        [Fact]
        public void DetectHeader_ComplexModel_ReturnStructure()
        {
            var result = Detection.DetectHeader(typeof(ComplexModel));
            var expected = new ModelStructure(nameof(ComplexModel), 8, 0, 3,
                new List<ModelStructure>
                {
                    new(nameof(ComplexModel.Guid), 1, 1, 0, null),
                    new(nameof(ComplexModel.Simple), 3, 1, 1,
                        new List<ModelStructure>
                        {
                            new(nameof(SimpleModel.Integer), 1, 2, 0, null),
                            new(nameof(SimpleModel.Str), 1, 2, 0, null),
                            new(nameof(SimpleModel.Decimal), 1, 2, 0, null),
                        }),
                    new(nameof(ComplexModel.InnerClassObj), 4, 1, 2, 
                        new List<ModelStructure>
                        {
                            new(nameof(ComplexModel.Guid), 1, 2, 0, null),
                            new(nameof(ComplexModel.Simple), 3, 2, 1,
                                new List<ModelStructure>
                                {
                                    new(nameof(SimpleModel.Integer), 1, 3, 0, null),
                                    new(nameof(SimpleModel.Str), 1, 3, 0, null),
                                    new(nameof(SimpleModel.Decimal), 1, 3, 0, null),
                                }),
                        }),
                });

            Assert.True(Equals(result, expected));
        }
    }

    public class SimpleModel
    {
        public int Integer { get; set; }
        public string Str { get; set; }
        public double Decimal { get; set; }
    }
    
    public class InvalidSimpleModel
    {
        public int Integer { get; set; }
        public string Str { get; set; }
        public List<double> Decimals { get; set; }
    }

    public class ComplexModel
    {
        public Guid Guid { get; set; }
        public SimpleModel Simple { get; set; }
        public InnerClass InnerClassObj { get; set; }

        public class InnerClass
        {
            public Guid Guid { get; set; }
            public SimpleModel Simple { get; set; }
        }
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