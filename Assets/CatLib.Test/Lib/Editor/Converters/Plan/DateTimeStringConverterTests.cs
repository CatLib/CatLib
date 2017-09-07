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

using System;
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
    public class DateTimeStringConverterTests
    {
        [TestMethod]
        public void TestDateTimeStringConvertTo()
        {
            var converter = new DateTimeStringConverter();

            var time = new DateTime(2020, 12, 13);
            Assert.AreEqual("12/13/2020 00:00:00", converter.ConvertTo(time , typeof(string)));
        }
    }
}
