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
    public class StringByteConverterTests
    {
        [TestMethod]
        public void TestStringByteConvertTo()
        {
            var converter = new StringByteConverter();

            Assert.AreEqual(byte.Parse(byte.MinValue.ToString()), converter.ConvertTo(byte.MinValue.ToString(), typeof(byte)));
            Assert.AreEqual(byte.Parse("132"), converter.ConvertTo("132", typeof(byte)));
            Assert.AreEqual(byte.Parse(byte.MaxValue.ToString()), converter.ConvertTo(byte.MaxValue.ToString(), typeof(byte)));
        }
    }
}
