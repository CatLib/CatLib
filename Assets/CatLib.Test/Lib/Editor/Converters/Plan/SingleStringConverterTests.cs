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
    public class SingleStringConverterTests
    {
        [TestMethod]
        public void TestSingleStringConvertTo()
        {
            var converter = new SingleStringConverter();
            Assert.AreEqual(float.MinValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(float.MinValue, typeof(string)));
            Assert.AreEqual("112.2", converter.ConvertTo(112.2f, typeof(string)));
            Assert.AreEqual(float.MaxValue.ToString(CultureInfo.InvariantCulture), converter.ConvertTo(float.MaxValue, typeof(string)));
        }
    }
}
