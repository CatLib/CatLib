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
using CatLib.API.Converters;
using CatLib.Converters;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Converters
{
    [TestClass]
    public class ConvertersProviderTests
    {
        /// <summary>
        /// 准备测试环境
        /// </summary>
        /// <returns></returns>
        private Application MakeTestEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestSimpleConvertTo()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            Assert.AreEqual(123, manager.Default.Convert<int>("123"));

            int val;
            if (manager.Default.TryConvert("333", out val))
            {
                Assert.AreEqual(333, val);
            }
        }

        class TestConvertsClass : ITypeConverter
        {
            /// <summary>
            /// 来源类型
            /// </summary>
            public Type From
            {
                get { return typeof(string); }
            }

            /// <summary>
            /// 目标类型
            /// </summary>
            public Type To
            {
                get { return typeof(int); }
            }

            /// <summary>
            /// 源类型转换到目标类型
            /// </summary>
            /// <param name="source">源类型</param>
            /// <param name="to">目标类型</param>
            /// <returns>目标类型</returns>
            public object ConvertTo(object source, Type to)
            {
                return 100;
            }
        }

        [TestMethod]
        public void TestConvertClone()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            var extend = manager.CloneExtend("newExtend");

            extend.AddConverter(new TestConvertsClass());

            Assert.AreEqual(123, manager.Default.Convert<int>("123"));
            Assert.AreEqual(100, extend.Convert<int>("123"));
            Assert.AreEqual(100, manager.Get("newExtend").Convert<int>("123"));
            Assert.AreEqual("123", manager.Get("newExtend").Convert<string>(123));
        }

        [TestMethod]
        public void TestUndefiendConvert()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            ExceptionAssert.Throws<ConverterException>(() =>
            {
                manager.Default.Convert<Application>("123");
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                Application tmp;
                manager.Default.TryConvert("123", out tmp);
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                string tmp;
                manager.Default.TryConvert(app, out tmp);
            });

            ExceptionAssert.Throws<ConverterException>(() =>
            {
                manager.Default.Convert("123", typeof(Application));
            });
        }

        private enum TestEnums
        {
            Hello = 1,
            World = 2,
        }

        [TestMethod]
        public void TestEnumConvert()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            var val = manager.Default.Convert<TestEnums>("1");

            Assert.AreEqual(TestEnums.Hello, val);

            val = manager.Default.Convert<TestEnums>("World");

            Assert.AreEqual(TestEnums.World, val);

            var intVal = manager.Default.Convert<string>(TestEnums.World);

            Assert.AreEqual("World", intVal);
        }

        [TestMethod]
        public void TestRepeatClone()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            manager.Extend(() => null, "newExtend");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                manager.CloneExtend("newExtend");
            });
        }

        [TestMethod]
        public void TestNullConvert()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                manager.Default.Convert<string>(null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                int val2;
                manager.Default.TryConvert(null, out val2);
            });
        }

        class TestThrowErrorConvertsClass : ITypeConverter
        {
            /// <summary>
            /// 来源类型
            /// </summary>
            public Type From
            {
                get { return typeof(string); }
            }

            /// <summary>
            /// 目标类型
            /// </summary>
            public Type To
            {
                get { return typeof(int); }
            }

            /// <summary>
            /// 源类型转换到目标类型
            /// </summary>
            /// <param name="source">源类型</param>
            /// <param name="to">目标类型</param>
            /// <returns>目标类型</returns>
            public object ConvertTo(object source, Type to)
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void TestTryConvertThrowException()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            manager.Default.AddConverter(new TestThrowErrorConvertsClass());

            int val;
            Assert.AreEqual(false, manager.Default.TryConvert("123", out val));
        }

        [TestMethod]
        public void TestGetIConverter()
        {
            var app = MakeTestEnv();
            var manager = app.Make<IConvertersManager>();

            Assert.AreSame(manager.Default, app.Make<IConverters>());
        }
    }
}
