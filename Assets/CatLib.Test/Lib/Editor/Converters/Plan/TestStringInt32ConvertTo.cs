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
    public class StringInt32ConverterTests
    {
        [TestMethod]
        public void TestStringInt32ConvertTo()
        {
            var converter = new StringInt32Converter();

            Assert.AreEqual(int.Parse(int.MinValue.ToString()), converter.ConvertTo(int.MinValue.ToString(), typeof(int)));
            Assert.AreEqual(int.Parse("1718218"), converter.ConvertTo("1718218", typeof(int)));
            Assert.AreEqual(int.Parse(int.MaxValue.ToString()), converter.ConvertTo(int.MaxValue.ToString(), typeof(int)));
        }
    }
}
