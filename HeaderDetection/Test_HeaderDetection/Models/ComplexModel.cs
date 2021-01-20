using System;

namespace Test_HeaderDetection.Models
{
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
}