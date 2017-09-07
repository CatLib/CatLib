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
    public class StringInt64ConverterTests
    {
        [TestMethod]
        public void TestStringInt64ConvertTo()
        {
            var converter = new StringInt64Converter();

            Assert.AreEqual(long.Parse(long.MinValue.ToString()), converter.ConvertTo(long.MinValue.ToString(), typeof(long)));
            Assert.AreEqual(long.Parse("1718218271818"), converter.ConvertTo("1718218271818", typeof(long)));
            Assert.AreEqual(long.Parse(long.MaxValue.ToString()), converter.ConvertTo(long.MaxValue.ToString(), typeof(long)));
        }
    }
}
