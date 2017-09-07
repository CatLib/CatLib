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
using CatLib.API.Config;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void RepeatInitTest()
        {
            var app = MakeApplication();

            app.Init();
        }

        /// <summary>
        /// 未经引导的初始化
        /// </summary>
        [TestMethod]
        public void NoBootstrapInit()
        {
            var app = new Application();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Init();
            });
        }

        /// <summary>
        /// 获取版本号测试
        /// </summary>
        [TestMethod]
        public void GetVersionTest()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(string.Empty, app.Version);
        }

        /// <summary>
        /// 获取当前启动流程
        /// </summary>
        [TestMethod]
        public void GetCurrentProcess()
        {
            var app = MakeApplication();
            Assert.AreEqual(Application.StartProcess.Inited, app.Process);
        }

        /// <summary>
        /// 重复的引导测试
        /// </summary>
        [TestMethod]
        public void RepeatBootstrap()
        {
            var app = new Application();
            app.Bootstrap();
            App.Register(new EventsProvider());
            app.Init();
            app.Bootstrap();
            Assert.AreEqual(Application.StartProcess.Inited, app.Process);
        }

        /// <summary>
        /// 注册非法类型测试
        /// </summary>
        [TestMethod]
        public void RegisteredIllegalType()
        {
            var app = new Application();
            app.Bootstrap();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                app.Register(null);
            });
        }

        /// <summary>
        /// 重复的注册
        /// </summary>
        [TestMethod]
        public void RepeatRegister()
        {
            var app = MakeApplication();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(new ConfigProvider());
            });
        }

        /// <summary>
        /// 获取运行时唯一Id
        /// </summary>
        [TestMethod]
        public void GetRuntimeId()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(app.GetRuntimeId(), app.GetRuntimeId());
        }

        private static bool prioritiesTest;

        private class ProviderTest1 : IServiceProvider
        {
            [Priority(10)]
            public void Init()
            {
                prioritiesTest = true;
            }

            public void Register()
            {

            }
        }

        [Priority(5)]
        private class ProviderTest2 : IServiceProvider
        {
            public void Init()
            {
                prioritiesTest = false;
            }

            public void Register()
            {

            }
        }

        /// <summary>
        /// 优先级测试
        /// </summary>
        [TestMethod]
        public void ProvidersPrioritiesTest()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Register(new ProviderTest1());
            App.Register(new ProviderTest2());
            App.Register(new EventsProvider());
            app.Init();
            Assert.AreEqual(true, prioritiesTest);
        }

        /// <summary>
        /// 无效的引导
        /// </summary>
        [TestMethod]
        public void IllegalBootstrap()
        {
            var app = new Application();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                app.Bootstrap(null);
                app.Init();
            });
        }

        /// <summary>
        /// 初始化后再注册
        /// </summary>
        [TestMethod]
        public void InitedAfterRegister()
        {
            prioritiesTest = true;
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Register(new ProviderTest1());
            App.Register(new EventsProvider());
            app.Init();

            App.Register(new ProviderTest2());
            Assert.AreEqual(false, prioritiesTest);
        }

        [TestMethod]
        public void TestRepeatRegister()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });

            app.Bootstrap();
            app.Register(new ProviderTest1());

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(new ProviderTest1());
            });
        }

        [TestMethod]
        public void TestOnDispatcher()
        {
            var app = MakeApplication();

            app.Listen("testevent", (payload) =>
            {
                Assert.AreEqual("abc", payload);
                return 123;
            });

            var result = app.TriggerHalt("testevent", "abc");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TestIsMainThread()
        {
            var app = MakeApplication();
            Assert.AreEqual(true, app.IsMainThread);
        }

        public class TestInjectConfigClass
        {
            [Config("helloworld")]
            public string MyConfig { get; set; }

            [Config("helloworld2")]
            public string MyConfig2 { get; set; }

            public string Name = "123";
        }

        private Application MakeApplication()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap(new BootstrapClass());
            app.Init();
            return app;
        }

        private class BootstrapClass : IBootstrap
        {
            public void Bootstrap()
            {
                App.Register(new ConfigProvider());
                App.Register(new ConvertersProvider());
                App.Register(new EventsProvider());
            }
        }
    }
}
