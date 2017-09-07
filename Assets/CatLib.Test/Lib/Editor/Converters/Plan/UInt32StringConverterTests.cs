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
    public class UInt32StringConverterTests
    {
        [TestMethod]
        public void TestUInt32StringConvertTo()
        {
            var converter = new UInt32StringConverter();
            Assert.AreEqual(uint.MinValue.ToString(), converter.ConvertTo(uint.MinValue, typeof(string)));
            Assert.AreEqual("12129", converter.ConvertTo((uint)12129, typeof(string)));
            Assert.AreEqual(uint.MaxValue.ToString(), converter.ConvertTo(uint.MaxValue, typeof(string)));
        }
    }
}
