/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.Debugger.WebMonitor.Handler;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.WebMonitor.Handler
{
    [TestClass]
    public class SizeMonitorHandlerTests
    {
        [TestMethod]
        public void TestSizeBound()
        {
            var monitor = new SizeMonitorHandler("test", new[] { "test" }, () =>
               {
                   return (long)1024;
               });

            Assert.AreEqual("1.00", monitor.Value);
            Assert.AreEqual("unit.size.kb", monitor.Unit);
        }

        [TestMethod]
        public void TestSizeBoundSmall()
        {
            var monitor = new SizeMonitorHandler("test", new[] { "test" }, () =>
            {
                return 512;
            });

            Assert.AreEqual("512.00", monitor.Value);
            Assert.AreEqual("unit.size.b", monitor.Unit);
        }

        [TestMethod]
        public void TestSizeBoundLarge()
        {
            var monitor = new SizeMonitorHandler("test", new[] { "test" }, () =>
            {
                return long.MaxValue;
            });

            Assert.AreEqual("1024.00", monitor.Value);
            Assert.AreEqual("unit.size.pb", monitor.Unit);
        }

        [TestMethod]
        public void TestGetTagName()
        {
            var monitor = new SizeMonitorHandler("test", new[] { "tag" }, () =>
            {
                return 512;
            });

            Assert.AreEqual("test", monitor.Name);
            Assert.AreEqual("tag", monitor.Tags[0]);
        }
    }
}
