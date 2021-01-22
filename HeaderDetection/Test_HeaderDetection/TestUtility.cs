using System;
using HeaderDetection;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestUtility
    {
        #region ConvertIndexToName

        [Fact]
        public void ConvertIndexToName_OutOfRangeIndex_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.ConvertIndexToName(-1));
        }

        [Theory]
        [InlineData("@")]
        [InlineData("[")]
        [InlineData("A2")]
        [InlineData("Z%Z")]
        public void ConvertNameToIndex_OutOfRangeIndex_ThrowArgumentOutOfRangeException(string name)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.ConvertNameToIndex(name));
        }

        [Fact]
        public void ConvertNameToIndex_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.ConvertNameToIndex(null!));
        }

        [Theory]
        [InlineData("a")]
        public void ConvertNameToIndex_ValidLowercaseName_ReturnValidIndex(string name)
        {
            var result = Utility.ConvertNameToIndex(name);
            var expected = 1;

            Assert.Equal(expected, result);
        }

        #endregion

        #region IsNameValid

        [Fact]
        public void IsNameValid_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.IsValidName(null!));
        }

        [Fact]
        public void IsNameValid_ValidLowercaseName_ReturnTrue()
        {
            var isValid = Utility.IsValidName("a");
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("A2")]
        [InlineData("Z%Z")]
        public void IsNameValid_InvalidName_ReturnFalse(string name)
        {
            Assert.False(Utility.IsValidName(name));
        }

        #endregion

        #region GetNextName single parameter

        [Fact]
        public void GetNextName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.GetNextName(null!));
        }

        [Fact]
        public void GetNextName_ValidLowercaseName_ReturnNextName()
        {
            var result = Utility.GetNextName("a");
            const string expected = "B";

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A", "B")]
        [InlineData("ABG", "ABH")]
        public void GetNextName_ValidName_ReturnNextName(string name, string expected)
        {
            var result = Utility.GetNextName(name);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A2")]
        [InlineData("Z%Z")]
        public void GetNextName_InvalidName_ThrowArgumentOutOfRangeException(string name)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.GetNextName(name));
        }

        #endregion

        #region GetNextName 2 parameters

        [Fact]
        public void GetNextNameWith2Params_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.GetNextName(null!, 1));
        }

        [Fact]
        public void GetNextNameWith2Params_InvalidCountNumber_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.GetNextName("A", -1));
        }

        [Fact]
        public void GetNextNameWith2Params_ValidLowercaseName_ReturnNextName()
        {
            var result = Utility.GetNextName("a", 0);
            const string expected = "A";

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A2")]
        [InlineData("Z%Z")]
        public void GetNextNameWith2Params_InvalidName_ThrowArgumentOutOfRangeException(string name)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.GetNextName(name, 1));
        }
        
        [Theory]
        [InlineData("A", "C")]
        [InlineData("ABG", "ABI")]
        public void GetNextNameWith2Params_ValidName_ReturnNextName(string name, string expected)
        {
            var result = Utility.GetNextName(name,2);

            Assert.Equal(expected, result);
        }

        #endregion

        #region GetNextNames

        [Fact]
        public void GetNexNames_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Utility.GetNexNames(null!, 1));
        }

        [Fact]
        public void GetNexNames_InvalidCountNumber_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.GetNexNames("A", -1));
        }

        [Fact]
        public void GetNexNames_ValidLowercaseName_ReturnNames()
        {
            var result = Utility.GetNexNames("a", 0);

            Assert.True(result.Length == 0);
        }

        [Theory]
        [InlineData("A2")]
        [InlineData("Z%Z")]
        public void GetNexNames_InvalidName_ThrowArgumentOutOfRangeException(string name)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Utility.GetNexNames(name, 1));
        }
        
        [Fact]
        public void GetNexNames_ValidName_ReturnNextName()
        {
            var result = Utility.GetNexNames("ABC",2);
            var expected = new[] {"ABD", "ABE"};

            Assert.Equal(expected, result);
        }

        #endregion
    }
}