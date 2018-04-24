using Ghostware.GPS.NET.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostware.GPS.NET.UnitTests.Converters
{
    [TestClass]
    public class CoordinateConverterUtilitiesTests
    {
        [TestMethod]
        public void GeoEtrs89ToLambert72Tests()
        {
            double x = 0;
            double y = 0;
            double z = 0;
            var result = CoordinateConverterUtilities.GeoETRS89ToLambert72(50.8991, 4.6157, 0, ref x, ref y, ref z);
            Assert.AreEqual(1, result);
            Assert.AreEqual(167371.95591558915, x);
            Assert.AreEqual(176557.58040618268, y);
            Assert.AreEqual(-42.959737811999808, z);
        }

        [TestMethod]
        public void Lambert72ToGeoEtrs89Tests()
        {
            double x = 0;
            double y = 0;
            double z = 0;
            CoordinateConverterUtilities.Lambert72ToGeoETRS89(167371.95591558915, 176557.58040618268, -42.959737811999808, ref x, ref y, ref z);
            Assert.AreEqual(50.8991, x, 0.0001);
            Assert.AreEqual(4.6157, y, 0.0001);
            Assert.AreEqual(0, z, 0.0001);
        }
    }
}
