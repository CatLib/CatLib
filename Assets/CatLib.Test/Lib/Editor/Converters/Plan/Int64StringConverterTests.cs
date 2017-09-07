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
    public class Int64StringConverterTests
    {
        [TestMethod]
        public void TestInt64StringConvertTo()
        {
            var converter = new Int64StringConverter();
            Assert.AreEqual(long.MinValue.ToString(), converter.ConvertTo(long.MinValue, typeof(string)));
            Assert.AreEqual("19283718291", converter.ConvertTo(19283718291, typeof(string)));
            Assert.AreEqual(long.MaxValue.ToString(), converter.ConvertTo(long.MaxValue, typeof(string)));
        }
    }
}
