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
using System.Net;
using CatLib.API.Debugger;
using CatLib.API.Json;
using CatLib.Debugger.WebConsole;
using SimpleJson;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Debugger.WebLog.Controller
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void TestGetLog()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            var logger = app.Make<ILogger>();

            logger.Debug("hello world");
            logger.Warning("my name is {0}" , "catlib");

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/log/get-log/1", out ret);
            logger.Warning("my name is {0}", "catlib2");
            string ret2;
            var statu2 = HttpHelper.Get("http://localhost:9478/debug/log/get-log/1", out ret2);
            console.Stop();

            var json = app.Make<IJson>();
            var retJson = json.Decode<JsonObject>(ret)["Response"] as IList<object>;
            var ret2Json = json.Decode<JsonObject>(ret2)["Response"] as IList<object>;

            Assert.AreEqual(HttpStatusCode.OK, statu);
            var id = (retJson[0] as IDictionary<string, object>)["id"];
            Assert.AreEqual((long)id + 1, (retJson[1] as IDictionary<string, object>)["id"]);
            Assert.AreEqual(HttpStatusCode.OK, statu2);
            Assert.AreEqual((long)id + 2, (ret2Json[0] as IDictionary<string, object>)["id"]);
        }
    }
}
#endif