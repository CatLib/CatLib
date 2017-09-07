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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests
{
    [TestClass]
    public class VersionTests
    {
        [TestMethod]
        public void VersionCompareTest()
        {
            var version = new Version("1.0.2-beta.10+29830");

            Assert.AreEqual(-1, version.Compare("1.0.2-beta.10.0+29830"));
            Assert.AreEqual(-1, version.Compare("1.0.2-beta.10.1+29830"));
            Assert.AreEqual(-1, version.Compare("1.0.2-rc.11+29830"));
            Assert.AreEqual(-1, version.Compare("2.0.2-beta.10+29830"));
            Assert.AreEqual(-1, version.Compare("1.0.3-beta.10+29830"));
            Assert.AreEqual(-1, version.Compare("1.1.2-beta.10+29830"));
            Assert.AreEqual(-1, version.Compare("1.0.2-beta.11+29830"));
            Assert.AreEqual(0, version.Compare("1.0.2-beta.10+29829.beta"));
            Assert.AreEqual(0, version.Compare("1.0.2-beta.10+29830"));
            Assert.AreEqual(0, version.Compare("1.0.2-beta.10+29831"));
            Assert.AreEqual(1, version.Compare("1.0.2-beta.9+29830"));
            Assert.AreEqual(1, version.Compare("1.0.2-alhpa.10+29830"));
            Assert.AreEqual(1, version.Compare("1.0.1-beta.10+29830"));
        }

        [TestMethod]
        public void VersionTestMajor()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(-1, version.Compare("3.0.2-beta.10.0+29830"));
            Assert.AreEqual(1, version.Compare("1.0.2-beta.10.0+29830"));
        }

        [TestMethod]
        public void VersionTestMinor()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(-1, version.Compare("2.3.2-beta.10.0+29830"));
            Assert.AreEqual(1, version.Compare("2.1.2-beta.10.0+29830"));
        }

        [TestMethod]
        public void VersionTestRevised()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(-1, version.Compare("2.2.3-beta.10.0+29830"));
            Assert.AreEqual(1, version.Compare("2.2.1-beta.10.0+29830"));
        }

        [TestMethod]
        public void VersionTestPreReleaseNumString()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(1, version.Compare("2.2.2-10.10.0+29830"));
            version = new Version("2.2.2-1.10+29830");
            Assert.AreEqual(-1, version.Compare("2.2.2-rc.10.0+29830"));
            version = new Version("2.2.2+29830");
            Assert.AreEqual(1, version.Compare("2.2.2-rc.10.0+29830"));
            version = new Version("2.2.2-beta+29830");
            Assert.AreEqual(-1, version.Compare("2.2.2+29830"));
        }

        [TestMethod]
        public void VersionTestPreReleaseSame()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(0, version.Compare("2.2.2-beta.10+29830"));
        }

        [TestMethod]
        public void VersionTestBlockIndex()
        {
            var version = new Version("2.2.2-beta.10+29830");
            Assert.AreEqual(-1, version.Compare("2.2.2-beta.10.1+29830"));
            Assert.AreEqual(1, version.Compare("2.2.2-beta+29830"));
        }

        [TestMethod]
        public void VersionTestNormals()
        {
            var version = new Version("2.2.2");
            Assert.AreEqual(0, version.Compare("2.2.2"));
            Assert.AreEqual(-1, version.Compare("2.2.3"));
            Assert.AreEqual(1, version.Compare("2.2.1"));
        }

        [TestMethod]
        public void VersionTestFromRMR()
        {
            var version = new Version(2, 2, 2);
            Assert.AreEqual("2.2.2", version.ToString());
        }

        [TestMethod]
        public void ThrowErrorVersion()
        {
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.01.2-beta.10+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1b.2-beta.10+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1.2/beta.10+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1.2-00.10+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1.2-+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1.2-0.+29830");
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                new Version("1.1..2-0.beta2+29830");
            });
        }
    }
}
