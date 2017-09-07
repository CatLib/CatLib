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
using CatLib.Debugger;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebMonitor.Handler;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Debugger.WebMonitor.Controller
{
    [TestClass]
    public class MonitorTests
    {
        [TestMethod]
        public void TestGetMonitor()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();
            var handler = new OnceRecordMonitorHandler("title", "ms", new[] { "tags" }, () => "helloworld");
            monitor.Monitor(handler);

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual("{\"Response\":[{\"name\":\"title\",\"value\":\"helloworld\",\"unit\":\"ms\",\"tags\":[\"tags\"]}]}", ret);
        }

        [TestMethod]
        public void TestGetMonitorIndex()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();

            App.Instance("DebuggerProvider.IndexMonitor", new List<string>
            {
                "title",
            });

            var handler = new OnceRecordMonitorHandler("title", "ms", new[] { "tags" }, () => "helloworld");
            monitor.Monitor(handler);

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors-index", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual(
                "{\"Response\":[{\"name\":\"title\",\"value\":\"helloworld\",\"unit\":\"ms\",\"tags\":[\"tags\"]}]}",
                ret);
        }

        public class ThrowTypeLoadExceptionHandler : IMonitorHandler
        {
            /// <summary>
            /// 监控的名字
            /// </summary>
            public string Name
            {
                get
                {
                    return "test";
                }
            }

            /// <summary>
            /// 标签(第0位：分类)
            /// </summary>
            public string[] Tags
            {
                get
                {
                    return new[]
                    {
                        "tag"
                    };
                }
            }

            /// <summary>
            /// 监控值的单位
            /// </summary>
            public string Unit
            {
                get { throw new TypeLoadException(); }
            }

            /// <summary>
            /// 监控值
            /// </summary>
            public string Value
            {
                get { throw new TypeLoadException(); }
            }
        }

        public class ThrowMissingMethodExceptionHandler : IMonitorHandler
        {
            /// <summary>
            /// 监控的名字
            /// </summary>
            public string Name
            {
                get
                {
                    return "test";
                }
            }

            /// <summary>
            /// 标签(第0位：分类)
            /// </summary>
            public string[] Tags
            {
                get
                {
                    return new[]
                    {
                        "tag"
                    };
                }
            }

            /// <summary>
            /// 监控值的单位
            /// </summary>
            public string Unit
            {
                get { throw new MissingMethodException(); }
            }

            /// <summary>
            /// 监控值
            /// </summary>
            public string Value
            {
                get { throw new MissingMethodException(); }
            }
        }

        [TestMethod]
        public void TestThrowTypeLoadExceptionMonitor()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();

            App.Instance("DebuggerProvider.IndexMonitor", new List<string>
            {
                "test",
            });

            monitor.Monitor(new ThrowTypeLoadExceptionHandler());

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors-index", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual(
                "{\"Response\":[{\"name\":\"test\",\"value\":\"code.notSupport\",\"unit\":\"\",\"tags\":[\"tag\"]}]}",
                ret);
        }

        [TestMethod]
        public void TestMissingMethodExceptionMonitor()
        {
            var app = DebuggerHelper.GetApplication();
            var console = app.Make<HttpDebuggerConsole>();
            var monitor = app.Make<IMonitor>();

            App.Instance("DebuggerProvider.IndexMonitor", new List<string>
            {
                "test",
            });

            monitor.Monitor(new ThrowMissingMethodExceptionHandler());

            string ret;
            var statu = HttpHelper.Get("http://localhost:9478/debug/monitor/get-monitors-index", out ret);

            console.Stop();
            Assert.AreEqual(HttpStatusCode.OK, statu);
            Assert.AreEqual(
                "{\"Response\":[{\"name\":\"test\",\"value\":\"code.notSupport\",\"unit\":\"\",\"tags\":[\"tag\"]}]}",
                ret);
        }
    }
}
#endif