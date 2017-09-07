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
    public class Int16StringConverterTests
    {
        [TestMethod]
        public void TestInt16StringConvertTo()
        {
            var converter = new Int16StringConverter();
            Assert.AreEqual(short.MinValue.ToString(), converter.ConvertTo(short.MinValue, typeof(string)));
            Assert.AreEqual("129", converter.ConvertTo((short)129, typeof(string)));
            Assert.AreEqual(short.MaxValue.ToString(), converter.ConvertTo(short.MaxValue, typeof(string)));
        }
    }
}
