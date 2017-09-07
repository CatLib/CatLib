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
    public class StringUInt16ConverterTests
    {
        [TestMethod]
        public void TestStringUInt16ConvertTo()
        {
            var converter = new StringUInt16Converter();

            Assert.AreEqual(ushort.Parse(ushort.MinValue.ToString()), converter.ConvertTo(ushort.MinValue.ToString(), typeof(ushort)));
            Assert.AreEqual(ushort.Parse("178"), converter.ConvertTo("178", typeof(ushort)));
            Assert.AreEqual(ushort.Parse(ushort.MaxValue.ToString()), converter.ConvertTo(ushort.MaxValue.ToString(), typeof(ushort)));
        }
    }
}
