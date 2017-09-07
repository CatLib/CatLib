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
    public class DoubleStringConverterTests
    {
        [TestMethod]
        public void TestDoubleStringConvertTo()
        {
            var converter = new DoubleStringConverter();
            Assert.AreEqual(double.MinValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(double.MinValue, typeof(string)));
            Assert.AreEqual("89.9864", converter.ConvertTo(89.9864, typeof(string)));
            Assert.AreEqual(double.MaxValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(double.MaxValue, typeof(string)));
        }
    }
}
