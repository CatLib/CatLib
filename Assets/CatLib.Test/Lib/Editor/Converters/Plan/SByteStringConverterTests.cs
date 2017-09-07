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
    public class SByteStringConverterTests
    {
        [TestMethod]
        public void TestSByteStringConvertTo()
        {
            var converter = new SByteStringConverter();
            Assert.AreEqual(sbyte.MinValue.ToString(), converter.ConvertTo(sbyte.MinValue, typeof(string)));
            Assert.AreEqual("112", converter.ConvertTo((sbyte)112, typeof(string)));
            Assert.AreEqual(sbyte.MaxValue.ToString(), converter.ConvertTo(sbyte.MaxValue, typeof(string)));
        }
    }
}
