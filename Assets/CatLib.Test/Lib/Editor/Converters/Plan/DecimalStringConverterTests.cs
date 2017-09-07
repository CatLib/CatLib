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

using System.Globalization;
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
    public class DecimalStringConverterTests
    {
        [TestMethod]
        public void TestDecimalStringConvertTo()
        {
            var converter = new DecimalStringConverter();
            Assert.AreEqual(decimal.MinValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(decimal.MinValue, typeof(string)));
            Assert.AreEqual("776", converter.ConvertTo((decimal)776, typeof(string)));
            Assert.AreEqual(decimal.MaxValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(decimal.MaxValue, typeof(string)));
        }
    }
}
