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
    public class Int32StringConverterTests
    {
        [TestMethod]
        public void TestInt32StringConvertTo()
        {
            var converter = new Int32StringConverter();
            Assert.AreEqual(int.MinValue.ToString(), converter.ConvertTo(int.MinValue, typeof(string)));
            Assert.AreEqual("1299102", converter.ConvertTo(1299102, typeof(string)));
            Assert.AreEqual(int.MaxValue.ToString(), converter.ConvertTo(int.MaxValue, typeof(string)));
        }
    }
}
