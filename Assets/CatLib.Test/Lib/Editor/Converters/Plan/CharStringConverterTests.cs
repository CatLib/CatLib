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
    public class CharStringConverterTests
    {
        [TestMethod]
        public void TestCharStringConvertTo()
        {
            var converter = new CharStringConverter();
            Assert.AreEqual(char.MinValue.ToString(), converter.ConvertTo(char.MinValue, typeof(string)));
            Assert.AreEqual("h", converter.ConvertTo('h', typeof(string)));
            Assert.AreEqual(char.MaxValue.ToString(), converter.ConvertTo(char.MaxValue, typeof(string)));
        }
    }
}
