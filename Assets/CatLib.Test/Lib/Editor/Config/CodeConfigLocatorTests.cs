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

using System;
using CatLib.Config.Locator;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Config
{
    /// <summary>
    /// 代码配置定位器测试
    /// </summary>
    [TestClass]
    public class CodeConfigLocatorTests
    {
        /// <summary>
        /// 设定测试
        /// </summary>
        [TestMethod]
        public void SetTest()
        {
            var locator = new CodeConfigLocator();

            ExceptionAssert.DoesNotThrow(() =>
            {
                locator.Set("name", "name");
                locator.Set("name2", null);
            });
        }

        /// <summary>
        /// 无效的设定测试
        /// </summary>
        [TestMethod]
        public void InvalidSetTest()
        {
            var locator = new CodeConfigLocator();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                locator.Set(null, "name");
            });
        }

        /// <summary>
        /// 获取值测试
        /// </summary>
        [TestMethod]
        public void TryGetValueTest()
        {
            var locator = new CodeConfigLocator();
            locator.Set("hello", "world");

            string str;
            if (!locator.TryGetValue("hello", out str))
            {
                Assert.Fail("can not get [hello]");
            }

            Assert.AreEqual("world", str);

            if (locator.TryGetValue("123", out str))
            {
                Assert.Fail();
            }

            Assert.AreEqual(null, str);
        }

        /// <summary>
        /// 无效的获取值测试
        /// </summary>
        [TestMethod]
        public void InvalidTryGetValueTest()
        {
            var locator = new CodeConfigLocator();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                string str;
                locator.TryGetValue(null, out str);
            });
        }
    }
}
