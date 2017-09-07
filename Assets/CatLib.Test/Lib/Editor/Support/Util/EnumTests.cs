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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Support.Util
{
    [TestClass]
    public class EnumTests
    {
        public class TestEnums : Enum
        {
            public static readonly TestEnums Hello = new TestEnums("hello");

            public static readonly TestEnums World = new TestEnums("world");

            /// <summary>
            /// 构造一个枚举
            /// </summary>
            protected TestEnums(string name) : base(name)
            {

            }
        }

        public class TestEnums2 : Enum
        {
            public static readonly TestEnums2 Hello = new TestEnums2("hello");

            public static readonly TestEnums2 World = new TestEnums2("world");

            /// <summary>
            /// 构造一个枚举
            /// </summary>
            protected TestEnums2(string name) : base(name)
            {

            }
        }

        [TestMethod]
        public void TestEnumEqualsNull()
        {
            Assert.AreEqual(false ,TestEnums.Hello.Equals(null));
        }

        [TestMethod]
        public void TestEnumEqualsNotEqual()
        {
            Assert.AreNotEqual(TestEnums.Hello, TestEnums2.Hello);
        }

        [TestMethod]
        public void TestEnumSame()
        {
            Assert.AreEqual(false, TestEnums.Hello == TestEnums2.Hello);
            Assert.AreEqual(false, TestEnums.Hello == TestEnums.World);
            Assert.AreEqual(false, TestEnums.Hello == null);
            Assert.AreEqual(false, null == TestEnums.Hello);
        }

        [TestMethod]
        public void TestEnumNotSame()
        {
            Assert.AreEqual(true, TestEnums.Hello != TestEnums2.Hello);
            Assert.AreEqual(true, TestEnums.Hello != TestEnums.World);
            Assert.AreEqual(true, TestEnums.Hello != null);
            Assert.AreEqual(true, null != TestEnums.Hello);
        }
    }
}
