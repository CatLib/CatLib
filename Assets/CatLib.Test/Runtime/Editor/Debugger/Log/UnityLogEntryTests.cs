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

using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.Log.Handler;
using UnityEngine;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.Log
{
    [TestClass]
    public sealed class UnityLogEntryTests
    {
        [TestMethod]
        public void TestGetCallStack()
        {
            var entry = new UnityLogEntry("helloworld", "hello\nworld", LogType.Log);
            Assert.AreEqual("hello" , entry.GetStackTrace()[0]);
            Assert.AreEqual("world", entry.GetStackTrace()[1]);
        }

        [TestMethod]
        public void TestNullMessage()
        {
            var entry = new UnityLogEntry(null, "hello\nworld", LogType.Log);
            Assert.AreEqual(string.Empty, entry.Message);
        }

        [TestMethod]
        public void TestNullStack()
        {
            var entry = new UnityLogEntry(null, null, LogType.Log);
            Assert.AreEqual(0, entry.GetStackTrace().Length);
        }

        [TestMethod]
        public void TestInvalidLogTypeStack()
        {
            var entry = new UnityLogEntry(null, null, (LogType)100000);
            Assert.AreEqual(LogLevels.Emergency, entry.Level);
            Assert.AreEqual("[LogType Invalid]", entry.Message);
        }

        [TestMethod]
        public void TestIsIgnore()
        {
            var entry = new UnityLogEntry(null, null, (LogType)100000);
            Assert.AreEqual(true, entry.IsIgnore(typeof(UnityConsoleLogHandler)));
            Assert.AreEqual(false, entry.IsIgnore(typeof(StdOutLogHandler)));
        }
    }
}
