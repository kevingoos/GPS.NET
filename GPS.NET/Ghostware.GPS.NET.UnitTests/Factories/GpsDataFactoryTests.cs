using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Factories;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostware.GPS.NET.UnitTests.Factories
{
    [TestClass]
    public class GpsDataFactoryTests
    {
        #region Create GpsInfo Tests

        [TestMethod]
        public void CreateComPortGpsInfoTest()
        {
            var result = GpsDataFactory.Create(GpsType.ComPort);
            Assert.IsInstanceOfType(result, typeof(ComPortInfo));
        }

        [TestMethod]
        public void CreateGpsdGpsInfoTest()
        {
            var result = GpsDataFactory.Create(GpsType.Gpsd);
            Assert.IsInstanceOfType(result, typeof(GpsdInfo));
        }

        [TestMethod]
        public void CreateFileGpsInfoTest()
        {
            var result = GpsDataFactory.Create(GpsType.File);
            Assert.IsInstanceOfType(result, typeof(FileGpsInfo));
        }

        [TestMethod]
        public void CreateWindowsLocationApiGpsInfoTest()
        {
            var result = GpsDataFactory.Create(GpsType.WindowsLocationApi);
            Assert.IsInstanceOfType(result, typeof(WindowsLocationApiInfo));
        }

        #endregion

        #region GetDataType Tests

        [TestMethod]
        public void CreateComPortGpsInfoTypeTest()
        {
            var result = GpsDataFactory.GetDataType(GpsType.ComPort);
            Assert.AreEqual(result, typeof(ComPortInfo));
        }

        [TestMethod]
        public void CreateGpsdGpsInfoTypeTest()
        {
            var result = GpsDataFactory.GetDataType(GpsType.Gpsd);
            Assert.AreEqual(result, typeof(GpsdInfo));
        }

        [TestMethod]
        public void CreateFileGpsInfoTypeTest()
        {
            var result = GpsDataFactory.GetDataType(GpsType.File);
            Assert.AreEqual(result, typeof(FileGpsInfo));
        }

        [TestMethod]
        public void CreateWindowsLocationApiGpsInfoTypeTest()
        {
            var result = GpsDataFactory.GetDataType(GpsType.WindowsLocationApi);
            Assert.AreEqual(result, typeof(WindowsLocationApiInfo));
        }

        #endregion
    }
}