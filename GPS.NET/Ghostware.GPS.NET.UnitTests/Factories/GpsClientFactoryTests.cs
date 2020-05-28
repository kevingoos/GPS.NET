using System;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Exceptions;
using Ghostware.GPS.NET.Factories;
using Ghostware.GPS.NET.GpsClients;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostware.GPS.NET.UnitTests.Factories
{
    [TestClass]
    public class GpsClientFactoryTests
    {
        #region Create by GpsType Tests

        [TestMethod]
        public void CreateComPortGpsTypeTest()
        {
            var result = GpsClientFactory.Create(GpsType.ComPort);
            Assert.IsInstanceOfType(result, typeof(ComPortGpsClient));
        }

        [TestMethod]
        public void CreateGpsdGpsTypeTest()
        {
            var result = GpsClientFactory.Create(GpsType.Gpsd);
            Assert.IsInstanceOfType(result, typeof(GpsdGpsClient));
        }

        [TestMethod]
        public void CreateFileGpsTypeTest()
        {
            var result = GpsClientFactory.Create(GpsType.File);
            Assert.IsInstanceOfType(result, typeof(FileGpsClient));
        }

        #endregion

        #region Create by GpsInfo Tests

        [TestMethod]
        public void CreateComPortGpsInfoTest()
        {
            var result = GpsClientFactory.Create(new ComPortInfo());
            Assert.IsInstanceOfType(result, typeof(ComPortGpsClient));
        }

        [TestMethod]
        public void CreateGpsdGpsInfoTest()
        {
            var result = GpsClientFactory.Create(new GpsdInfo());
            Assert.IsInstanceOfType(result, typeof(GpsdGpsClient));
        }

        [TestMethod]
        public void CreateFileGpsInfoTest()
        {
            var result = GpsClientFactory.Create(new FileGpsInfo());
            Assert.IsInstanceOfType(result, typeof(FileGpsClient));
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownTypeException))]
        public void CreateUnkownGpsInfoTest()
        {
            GpsClientFactory.Create(new UnkownGpsInfo());
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateNullGpsTest()
        {
            GpsClientFactory.Create(null);
        }
    }
}
