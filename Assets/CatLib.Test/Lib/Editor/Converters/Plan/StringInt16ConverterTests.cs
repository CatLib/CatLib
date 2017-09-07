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
    public class StringInt16ConverterTests
    {
        [TestMethod]
        public void TestStringInt16ConvertTo()
        {
            var converter = new StringInt16Converter();

            Assert.AreEqual(short.Parse(short.MinValue.ToString()), converter.ConvertTo(short.MinValue.ToString(), typeof(short)));
            Assert.AreEqual(short.Parse("178"), converter.ConvertTo("178", typeof(short)));
            Assert.AreEqual(short.Parse(short.MaxValue.ToString()), converter.ConvertTo(short.MaxValue.ToString(), typeof(short)));
        }
    }
}
