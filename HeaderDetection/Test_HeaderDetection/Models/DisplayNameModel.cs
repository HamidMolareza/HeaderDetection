using System.ComponentModel;

namespace Test_HeaderDetection.Models
{
    [DisplayName("main")]
    public class DisplayNameModel
    {
        [DisplayName("Inner Property")] public string Property { get; set; }
    }
}