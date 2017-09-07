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
using System;

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
    public class IListStringStringConverterTests
    {
        [TestMethod]
        public void TestIListStringStringConvertTo()
        {
            var converter = new IListStringStringConverter();
            var result = converter.ConvertTo(new []
            {
                "hello" , "world" , "same"
            }, typeof(string));

            Assert.AreEqual("hello;world;same", result);
        }

        [TestMethod]
        public void TestIListStringStringConvertToThrowTypeError()
        {
            var converter = new IListStringStringConverter();

            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                converter.ConvertTo("123", typeof(string));
            });   
        }
    }
}
