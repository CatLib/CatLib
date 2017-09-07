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
    public class UInt16StringConverterTests
    {
        [TestMethod]
        public void TestUInt16StringConvertTo()
        {
            var converter = new UInt16StringConverter();
            Assert.AreEqual(ushort.MinValue.ToString(), converter.ConvertTo(ushort.MinValue, typeof(string)));
            Assert.AreEqual("129", converter.ConvertTo((ushort)129, typeof(string)));
            Assert.AreEqual(ushort.MaxValue.ToString(), converter.ConvertTo(ushort.MaxValue, typeof(string)));
        }
    }
}
