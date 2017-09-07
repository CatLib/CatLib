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

using System.Collections.Generic;
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
    public class StringIListStringConverterTests
    {
        [TestMethod]
        public void TestStringIListStringConvertTo()
        {
            var converter = new StringIListStringConverter();
            var result = converter.ConvertTo("hello;world;same", typeof(IList<string>));
            Assert.AreEqual(3,(result as IList<string>).Count);
            Assert.AreEqual("hello", (result as IList<string>)[0]);
            Assert.AreEqual("world", (result as IList<string>)[1]);
            Assert.AreEqual("same", (result as IList<string>)[2]);
        }
    }
}
