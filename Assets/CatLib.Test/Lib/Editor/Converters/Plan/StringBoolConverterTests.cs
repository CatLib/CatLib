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

using CatLib.API.Converters;
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
    public class StringBoolConverterTests
    {
        [TestMethod]
        public void TestStringBoolConvertTo()
        {
            var converter = new StringBoolConverter();

            Assert.AreEqual(true, converter.ConvertTo("True", typeof(bool)));
            Assert.AreEqual(true, converter.ConvertTo("true", typeof(bool)));
            Assert.AreEqual(true, converter.ConvertTo("on", typeof(bool)));
            Assert.AreEqual(true, converter.ConvertTo("yes", typeof(bool)));
            Assert.AreEqual(true, converter.ConvertTo("y", typeof(bool)));
            Assert.AreEqual(true, converter.ConvertTo("1", typeof(bool)));

            Assert.AreEqual(false, converter.ConvertTo("false", typeof(bool)));
            Assert.AreEqual(false, converter.ConvertTo("faLse", typeof(bool)));
            Assert.AreEqual(false, converter.ConvertTo("off", typeof(bool)));
            Assert.AreEqual(false, converter.ConvertTo("No", typeof(bool)));
            Assert.AreEqual(false, converter.ConvertTo("n", typeof(bool)));
            Assert.AreEqual(false, converter.ConvertTo("0", typeof(bool)));

            ExceptionAssert.Throws<ConverterException>(() =>
            {
                converter.ConvertTo("1111", typeof(bool));
            });
        }
    }
}
