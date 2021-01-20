using HeaderDetection;
using Xunit;

namespace Test_HeaderDetection
{
    public class TestTable
    {
        [Fact]
        public void Items_SetNull_ReturnEmptyList()
        {
            var obj = new Table<string>(null!);
            Assert.True(obj.Items is not null);
            
            obj.Items = null!;
            Assert.True(obj.Items is not null);
        }
    }
}