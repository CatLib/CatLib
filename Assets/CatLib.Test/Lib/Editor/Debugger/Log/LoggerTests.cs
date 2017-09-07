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
using System.Reflection;
using CatLib.API.Debugger;
using CatLib.Debugger.Log;
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
    public class LoggerTests
    {
        private static string logLevel;

        public class TestHandler : ILogHandler
        {
            /// <summary>
            /// 日志处理器
            /// </summary>
            /// <param name="log"></param>
            public void Handler(ILogEntry log)
            {
                logLevel = log.Level + log.Message;
            }
        }

        [TestMethod]
        public void TestDebugLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Debug(LogLevels.Debug.ToString() + "{0}", "helloworld");
                return LogLevels.Debug.ToString();
            });
        }

        [TestMethod]
        public void TestAlertLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Alert(LogLevels.Alert.ToString() + "{0}", "helloworld");
                return LogLevels.Alert.ToString();
            });
        }

        [TestMethod]
        public void TestCriticalLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Critical(LogLevels.Critical.ToString() + "{0}", "helloworld");
                return LogLevels.Critical.ToString();
            });
        }

        [TestMethod]
        public void TestEmergencyLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Emergency(LogLevels.Emergency.ToString() + "{0}", "helloworld");
                return LogLevels.Emergency.ToString();
            });
        }

        [TestMethod]
        public void TestErrorLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Error(LogLevels.Error.ToString() + "{0}", "helloworld");
                return LogLevels.Error.ToString();
            });
        }

        [TestMethod]
        public void TestInfoLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Info(LogLevels.Info.ToString() + "{0}", "helloworld");
                return LogLevels.Info.ToString();
            });
        }

        [TestMethod]
        public void TestNoticeLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Notice(LogLevels.Notice.ToString() + "{0}", "helloworld");
                return LogLevels.Notice.ToString();
            });
        }

        [TestMethod]
        public void TestWarningLogLevel()
        {
            AssertLog((logger) =>
            {
                logger.Warning(LogLevels.Warning.ToString() + "{0}", "helloworld");
                return LogLevels.Warning.ToString();
            });
        }

        [TestMethod]
        public void TestLog()
        {
            AssertLog((logger) =>
            {
                logger.Log(LogLevels.Warning, LogLevels.Warning.ToString() + "{0}", "helloworld");
                return LogLevels.Warning.ToString();
            });
        }

        [TestMethod]
        public void TestSetSkip()
        {
            logLevel = string.Empty;
            var app = DebuggerHelper.GetApplication(false);
            var logger = app.Make<ILogger>();
            ExceptionAssert.DoesNotThrow(() =>
            {
                (logger as Logger).SetSkip(0);
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                (logger as Logger).SetSkip(10, () =>
                {
                    var flag = BindingFlags.Instance | BindingFlags.NonPublic;
                    var type = logger.GetType();
                    var field = type.GetField("skipFrames", flag);
                    Assert.AreEqual(10, (int)field.GetValue(logger));
                });
            });
        }

        [TestMethod]
        public void TestSetSkipRecursiveCall()
        {
            logLevel = string.Empty;
            var app = DebuggerHelper.GetApplication(false);
            var logger = app.Make<ILogger>();
            ExceptionAssert.DoesNotThrow(() =>
            {
                (logger as Logger).SetSkip(0);
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                var flag = BindingFlags.Instance | BindingFlags.NonPublic;
                var type = logger.GetType();
                var field = type.GetField("skipFrames", flag);
                (logger as Logger).SetSkip(10, () =>
                {
                    (logger as Logger).SetSkip(20, () =>
                    {
                        Assert.AreEqual(10, (int)field.GetValue(logger));
                    });
                });
                Assert.AreEqual(0, (int)field.GetValue(logger));
            });
        }

        public void AssertLog(Func<ILogger, string> doLogger)
        {
            logLevel = string.Empty;
            var app = DebuggerHelper.GetApplication(false);
            var logger = app.Make<ILogger>();

            (logger as Logger).AddLogHandler(new TestHandler());

            var result = doLogger(logger);

            Assert.AreEqual(result + result + "helloworld", logLevel);
        }
    }
}
