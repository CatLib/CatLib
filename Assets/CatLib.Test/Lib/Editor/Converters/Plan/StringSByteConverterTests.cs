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
    public class StringSByteConverterTests
    {
        [TestMethod]
        public void TestStringSByteConvertTo()
        {
            var converter = new StringSByteConverter();

            Assert.AreEqual(sbyte.Parse(sbyte.MinValue.ToString()), converter.ConvertTo(sbyte.MinValue.ToString(), typeof(sbyte)));
            Assert.AreEqual(sbyte.Parse("125"), converter.ConvertTo("125", typeof(sbyte)));
            Assert.AreEqual(sbyte.Parse(sbyte.MaxValue.ToString()), converter.ConvertTo(sbyte.MaxValue.ToString(), typeof(sbyte)));
        }
    }
}
