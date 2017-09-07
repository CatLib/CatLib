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

using System.Net;
using CatLib.Debugger.WebConsole;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Debugger.WebLog.Controller
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        public void TestEcho()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/util/echo/helloworld", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual(string.Empty, ret);
        }
    }
}
#endif