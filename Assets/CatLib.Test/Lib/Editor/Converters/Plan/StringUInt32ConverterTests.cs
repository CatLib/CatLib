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
    public class StringUInt32ConverterTests
    {
        [TestMethod]
        public void TestStringUInt32ConvertTo()
        {
            var converter = new StringUInt32Converter();

            Assert.AreEqual(uint.Parse(uint.MinValue.ToString()), converter.ConvertTo(uint.MinValue.ToString(), typeof(uint)));
            Assert.AreEqual(uint.Parse("1711318"), converter.ConvertTo("1711318", typeof(uint)));
            Assert.AreEqual(uint.Parse(uint.MaxValue.ToString()), converter.ConvertTo(uint.MaxValue.ToString(), typeof(uint)));
        }
    }
}
