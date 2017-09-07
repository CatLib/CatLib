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

using CatLib.Converters.Plan;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Converters.Plan
{
    [TestClass]
    public class StringEnumConverterTests
    {
        private enum TestEnums
        {
            Hello = 1,
            World = 2,
        }

        [TestMethod]
        public void TestStringEnumConvertTo()
        {
            var converter = new StringEnumConverter();

            Assert.AreEqual(TestEnums.Hello, converter.ConvertTo("TestEnums.Hello", typeof(TestEnums)));
            Assert.AreEqual(TestEnums.World, converter.ConvertTo("World", typeof(TestEnums)));
            Assert.AreEqual(TestEnums.Hello, converter.ConvertTo("1", typeof(TestEnums)));
        }
    }
}
