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
using System.Collections.Generic;
using System.Net;
using CatLib.API.Routing;
using CatLib.Debugger.WebConsole;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Debugger.WebConsole
{
    [Routed]
    [TestClass]
    public class HttpDebuggerConsoleTests
    {
        private class SimpleResponse : IWebConsoleResponse
        {
            /// <summary>
            /// 响应
            /// </summary>
            public object Response { get; private set; }

            public SimpleResponse(object data)
            {
                Response = data;
            }
        }

        [Routed]
        public void SimpleCall(IResponse response)
        {
            response.SetContext(new SimpleResponse(new Dictionary<string, string> { { "hello", "world" } }));
        }

        [Routed]
        public void ThrowError()
        {
            throw new Exception();
        }

        [TestMethod]
        public void TestDebuggerConsole()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/catlib/http-debugger-console-tests/simple-call", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":{\"hello\":\"world\"}}", ret);
        }

        [TestMethod]
        public void TestNotFound()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            bool isThrow = false;
            try
            {
                string ret;
                var statu = HttpHelper.Get("http://localhost:9478/notfound/notfound/notfound", out ret);
            }
            catch (WebException ex)
            {
                Assert.AreEqual(WebExceptionStatus.ProtocolError, ex.Status);
                isThrow = true;
            }
            finally
            {
                console.Stop();
            }
            Assert.AreEqual(true, isThrow);
        }

        [TestMethod]
        public void TestThrowError()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            bool isThrow = false;
            try
            {
                string ret;
                var statu = HttpHelper.Get("http://localhost:9478/catlib/http-debugger-console-tests/throw-error",
                    out ret);
            }
            catch (WebException ex)
            {
                Assert.AreEqual(WebExceptionStatus.ProtocolError, ex.Status);
                isThrow = true;
            }
            finally
            {
                console.Stop();
            }

            Assert.AreEqual(true, isThrow);
        }

        [TestMethod]
        public void TestNoDispatcher()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
        }

        [TestMethod]
        public void TestGetGuid()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/http-debugger-console/get-guid", out ret);
            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
        }

        [TestMethod]
        public void OnDestroyTest()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            app.Release<HttpDebuggerConsole>();
            string ret;
            ExceptionAssert.Throws<WebException>(() =>
            {
                var statu = HttpHelper.Get("http://localhost:9478/", out ret);
                Console.WriteLine(statu);
            });
        }

        [TestMethod]
        public void RepeatStartTest()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();

            console.Start();

            string ret;
            HttpStatusCode statu;
            try
            {
                statu = HttpHelper.Get("http://localhost:9478/", out ret);
            }
            finally
            {
                console.Stop();
            }

            Assert.AreEqual(HttpStatusCode.OK, statu);
        }
    }
}
#endif