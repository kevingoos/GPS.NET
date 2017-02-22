using Ghostware.GPS.NET.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostware.GPS.NET.UnitTests.Converters
{
    [TestClass]
    public class CoordinateConverterUtilitiesTests
    {
        [TestMethod]
        public void CreateComPortGpsTypeTest()
        {
            double x = 0;
            double y = 0;
            double z = 0;
            var result = CoordinateConverterUtilities.GeoETRS89ToLambert72(50.8991, 4.6157, 0, ref x, ref y, ref z);
            Assert.AreEqual(1, result);
        }
    }
}
