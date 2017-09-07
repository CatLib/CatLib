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

using System.Collections;
using CatLib.API.MonoDriver;
using CatLib.Events;
using CatLib.MonoDriver;
using MDriver = global::CatLib.MonoDriver.MonoDriver;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif


namespace CatLib.Tests.MonoDriver
{
    [TestClass]
    public class DriverTests
    {
        private static string updateResult;
        private static string lateUpdateResult;
        private static bool lateUpdateIsAfter;
        private static string onDestroyResult;

        public class TestStaticClass : IUpdate, ILateUpdate , IOnDestroy
        {
            public void Update()
            {
                if (updateResult == "TestStaticClassUpdate")
                {
                    updateResult = "TestStaticClassUpdate1";
                }
                else
                {
                    updateResult = "TestStaticClassUpdate";
                }
            }

            public void LateUpdate()
            {
                if (updateResult == "TestStaticClassUpdate")
                {
                    lateUpdateIsAfter = true;
                }
                else
                {
                    lateUpdateIsAfter = false;
                }
                lateUpdateResult = "TestStaticClassLateUpdate";

            }

            public void OnDestroy()
            {
                onDestroyResult = "TestStaticClassDestroy";
            }
        }

        /// <summary>
        /// 静态对象解决时自动载入
        /// </summary>
        [TestMethod]
        public void OnResolvingLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();

            var d = c.Make<IMonoDriver>() as MDriver;
            d.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
        }

        /// <summary>
        /// 释放时自动移除
        /// </summary>
        [TestMethod]
        public void OnReleaseUnLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();

            c.Release<TestStaticClass>();

            var d = c.Make<IMonoDriver>() as MDriver;
            d.Update();
            Assert.AreEqual(string.Empty, updateResult);
        }

        /// <summary>
        /// 替换实例的Unload场景
        /// </summary>
        [TestMethod]
        public void ReplaceInstanceToUnload()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();
      
            c.Instance<TestStaticClass>(null);

            var d = c.Make<IMonoDriver>() as MDriver;
            d.Update();
            Assert.AreEqual(string.Empty, updateResult);

            ExceptionAssert.DoesNotThrow(() =>
            {
                c.Release<TestStaticClass>();
            });
        }

        /// <summary>
        /// 载入和卸载
        /// </summary>
        [TestMethod]
        public void OnLoadUnLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            var cls = c.Make<TestStaticClass>();

            var d = c.Make<IMonoDriver>() as MDriver;
            d.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            d.Detach(cls);
            d.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
        }

        /// <summary>
        /// 测试驱动和行为
        /// </summary>
        [TestMethod]
        public void TestDriver()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            var cls = c.Make<TestStaticClass>();

            var d = c.Make<IMonoDriver>() as MDriver;
            d.Update();
            d.LateUpdate();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            d.Detach(cls);
            d.Update();
            d.LateUpdate();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            Assert.AreEqual(true, lateUpdateIsAfter);
            Assert.AreEqual("TestStaticClassDestroy", onDestroyResult);
            Assert.AreEqual("TestStaticClassLateUpdate", lateUpdateResult);
        }

        /// <summary>
        /// 主线程调用
        /// </summary>
        [TestMethod]
        public void MainThreadCall()
        {
            var c = MakeDriver();
            var d = c.Make<IMonoDriver>() as MDriver;
            var isCall = false;
            d.MainThread(() =>
            {
                isCall = true;
            });

            Assert.AreEqual(true, isCall);
        }

        private bool isRunCoroutine;

        /// <summary>
        /// 在主线程调用通过协程
        /// </summary>
        [TestMethod]
        public void MainThreadCallWithCoroutine()
        {
            var c = MakeDriver();

            isRunCoroutine = false;
            var d = c.Make<IMonoDriver>() as MDriver;
            d.MainThread(Coroutine());

            Assert.AreEqual(true, isRunCoroutine);
        }

        /// <summary>
        /// 开始和停止协同
        /// </summary>
        [TestMethod]
        public void StartAndStopCoroutine()
        {
            var c = MakeDriver();
            isRunCoroutine = false;
            var d = c.Make<IMonoDriver>() as MDriver;
            d.StartCoroutine(Coroutine());
            Assert.AreEqual(true, isRunCoroutine);
            d.StopCoroutine(Coroutine());
        }

        /// <summary>
        /// 重复的载入测试
        /// </summary>
        [TestMethod]
        public void RepeatLoadTest()
        {
            var c = MakeDriver();
            var d = c.Make<IMonoDriver>() as MDriver;
            var obj = new TestStaticClass();

            d.Attach(obj);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                d.Attach(obj);
            });
        }

        /// <summary>
        /// 释放测试
        /// </summary>
        [TestMethod]
        public void OnDestroyTest()
        {
            var c = MakeDriver();
            var d = c.Make<IMonoDriver>() as MDriver;
            var obj = new TestStaticClass();

            d.Attach(obj);

            updateResult = string.Empty;
            d.OnDestroy();
            d.Update();

            Assert.AreEqual(string.Empty, updateResult);
        }

        /// <summary>
        /// 卸载没有被加载的对象
        /// </summary>
        [TestMethod]
        public void UnloadNotLoadObj()
        {
            onDestroyResult = string.Empty;
            var c = MakeDriver();
            var d = c.Make<IMonoDriver>() as MDriver;
            var obj = new TestStaticClass();
            d.Detach(obj);
            Assert.AreEqual(string.Empty, onDestroyResult);
        }

        /// <summary>
        /// 测试全局On
        /// </summary>
        [TestMethod]
        public void GloablEventOn()
        {
            var app = MakeDriver();
            var isCall = false;
            app.On("GloablEventOn", (payload) =>
            {
                isCall = true;
            });

            app.Trigger("GloablEventOn");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 测试全局On
        /// </summary>
        [TestMethod]
        public void GloablEventOne()
        {
            var app = MakeDriver();
            var isCall = false;
            app.On("GloablEventOne", (payload) =>
            {
                isCall = !isCall;
            },1);
            app.Trigger("GloablEventOne");
            app.Trigger("GloablEventOne");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 根据名称发送者触发事件
        /// </summary>
        [TestMethod]
        public void AppTriggerEventWithSender()
        {
            var app = MakeDriver();
            var isCall = false;
            var sender = new object();
            app.On("AppTriggerEventWithSender", (payload) =>
            {
                if (payload == sender)
                {
                    isCall = !isCall;
                }
            },1);

            app.Trigger("AppTriggerEventWithSender", sender);
            Assert.AreEqual(true, isCall);
        }

        private static int destroyNum_DoubleDestroyClass;

        public class DoubleOnDestroyClass : IOnDestroy
        {
            /// <summary>
            /// 当释放时
            /// </summary>
            public void OnDestroy()
            {
                destroyNum_DoubleDestroyClass++;
            }
        }
        
        /// <summary>
        /// 防止双重释放
        /// </summary>
        [TestMethod]
        public void TestDoubleDestroyClass()
        {
            destroyNum_DoubleDestroyClass = 0;
            var app = MakeDriver();
            var onRelease = false;
            app.Singleton<DoubleOnDestroyClass>().OnRelease((bind, obj) =>
            {
                onRelease = true;
            });

            app.Make<DoubleOnDestroyClass>();
            var d = app.Make<IMonoDriver>() as MDriver;
            d.OnDestroy();

            Assert.AreEqual(true, onRelease);
            Assert.AreEqual(1, destroyNum_DoubleDestroyClass);
        }

        private static string fixedUpdateValue = string.Empty;
        public class TestFixedUpdateClass : IFixedUpdate
        {
            /// <summary>
            /// FixedUpdate时调用
            /// </summary>
            public void FixedUpdate()
            {
                fixedUpdateValue = "helloworld";
            }
        }

        [TestMethod]
        public void TestFixedUpdate()
        {
            fixedUpdateValue = string.Empty;
            var app = MakeDriver();
            app.Singleton<TestFixedUpdateClass>();

            app.Make<TestFixedUpdateClass>();
            var d = app.Make<IMonoDriver>() as MDriver;
            d.FixedUpdate();

            Assert.AreEqual("helloworld", fixedUpdateValue);
        }

        private static string onGUIValue = string.Empty;
        public class TestOnGUIValue : IOnGUI
        {
            /// <summary>
            /// OnGUI时调用
            /// </summary>
            public void OnGUI()
            {
                onGUIValue = "hellogui";
            }
        }

        [TestMethod]
        public void TestOnGUI()
        {
            fixedUpdateValue = string.Empty;
            var app = MakeDriver();
            app.Singleton<TestOnGUIValue>();

            app.Make<TestOnGUIValue>();
            var d = app.Make<IMonoDriver>() as MDriver;
            d.OnGUI();

            Assert.AreEqual("hellogui", onGUIValue);
        }

        private IEnumerator Coroutine()
        {
            yield return null;
            isRunCoroutine = true;
        }

        public IApplication MakeDriver()
        {
            var driver = new Application();
            driver.Bootstrap();
            driver.Register(new EventsProvider());
            driver.Register(new MonoDriverProvider());
            driver.Init();
            return driver;
        }
    }
}
