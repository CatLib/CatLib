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
    public class StringSingleConverterTests
    {
        [TestMethod]
        public void TestStringSingleConvertTo()
        {
            var converter = new StringSingleConverter();

            Assert.AreEqual(float.Parse(float.MinValue.ToString(CultureInfo.InvariantCulture)), converter.ConvertTo(float.MinValue.ToString(CultureInfo.InvariantCulture), typeof(float)));
            Assert.AreEqual(float.Parse("172.92"), converter.ConvertTo("172.92", typeof(float)));
            Assert.AreEqual(float.Parse(float.MaxValue.ToString(CultureInfo.InvariantCulture)), converter.ConvertTo(float.MaxValue.ToString(CultureInfo.InvariantCulture), typeof(float)));
        }
    }
}
