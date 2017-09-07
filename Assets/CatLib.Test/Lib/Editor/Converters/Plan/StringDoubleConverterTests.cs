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
    public class StringDoubleConverterTests
    {
        [TestMethod]
        public void TestStringDoubleConvertTo()
        {
            var converter = new StringDoubleConverter();

            ExceptionAssert.Throws<OverflowException>(() =>
            {
                Assert.AreEqual(double.Parse((double.MinValue).ToString(CultureInfo.InvariantCulture)),
                    converter.ConvertTo((double.MinValue).ToString(CultureInfo.InvariantCulture), typeof(double)));
            });

            Assert.AreEqual(192.129,
                converter.ConvertTo(192.129.ToString(CultureInfo.InvariantCulture), typeof(double)));

            ExceptionAssert.Throws<OverflowException>(() =>
            {
                Assert.AreEqual(double.Parse((double.MaxValue).ToString(CultureInfo.InvariantCulture)),
                    converter.ConvertTo((double.MaxValue).ToString(CultureInfo.InvariantCulture), typeof(double)));
            });
        }
    }
}
