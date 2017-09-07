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
using CatLib.API.Routing;
using CatLib.Events;
using CatLib.Routing;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Routing
{
    /// <summary>
    /// 路由异常冒泡测试
    /// </summary>
    [TestClass]
    public class RouterExceptionTests
    {
        internal Router MakeRouter()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Init();
            var router = new Router(app, app);

            router.SetDefaultScheme("catlib");
            return router;
        }

        /// <summary>
        /// 简单的无Scheme测试
        /// </summary>
        [TestMethod]
        public void TestSimpleNoSchemeException()
        {
            var router = MakeRouter();

            var isThrowOnError = false;
            router.OnError((req, re, ex, next) =>
            {
                isThrowOnError = true;
            });

            var isThrowOnNotFound = false;
            router.OnNotFound((req, next) =>
            {
                isThrowOnNotFound = true;
            });

            router.Dispatch("catlib://helloworld/call");
            Assert.AreEqual(false, isThrowOnError);
            Assert.AreEqual(true, isThrowOnNotFound);
        }

        /// <summary>
        /// 简单的无Scheme异常测试，在没有拦截的情况下抛出异常
        /// </summary>
        [TestMethod]
        public void TestSimpleNoSchemeOnThrowOut()
        {
            var router = MakeRouter();

            router.OnNotFound((req, next) =>
            {
                next(req);
            });

            ExceptionAssert.Throws<NotFoundRouteException>(() =>
            {
                router.Dispatch("catlib://helloworld/call");
            });
        }

        /// <summary>
        /// 检查嵌套的无scheme异常
        /// </summary>
        [TestMethod]
        public void TestNestedNoSchemeException()
        {
            var router = MakeRouter();

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                router.Dispatch("catlib://helloworld/call");
            });

            var throwNotFound = 0;
            router.OnNotFound((req, next) =>
            {
                throwNotFound++;
            });

            router.Dispatch("ui://helloworld/call");

            Assert.AreEqual(1, throwNotFound);
        }

        /// <summary>
        /// 检查嵌套的无scheme异常，并抛出
        /// </summary>
        [TestMethod]
        public void TestNestedNoSchemeThrowException()
        {
            var router = MakeRouter();

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                router.Dispatch("catlib://helloworld/call");
            });

            var throwNotFound = 0;
            router.OnNotFound((req, next) =>
            {
                throwNotFound++;
                next(req);
            });

            ExceptionAssert.Throws<NotFoundRouteException>(() =>
            {
                router.Dispatch("ui://helloworld/call");
            });

            Assert.AreEqual(1, throwNotFound);
        }

        [TestMethod]
        public void TestSimpleException()
        {
            var router = MakeRouter();

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                throw new ArgumentNullException("hello");
            });

            var throwNotFound = 0;
            router.OnNotFound((req, next) =>
            {
                throwNotFound++;
                next(req);
            });

            var throwError = 0;
            router.OnError((req, res, ex, next) =>
            {
                throwError++;
                next(req, res, ex);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                router.Dispatch("ui://helloworld/call");
            });

            Assert.AreEqual(1, throwError);
            Assert.AreEqual(0, throwNotFound);
        }


        [TestMethod]
        public void TestNestedException()
        {
            var router = MakeRouter();
            var throwNotFound = 0; var throwError = 0;

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call2");
            });

            router.Reg("ui://helloworld/call2", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call3");
            }).OnError((req, res, ex, next) =>
            {
                if (throwError == 1)
                {
                    throwError = 10;
                }
                next(req, res, ex);
            });

            router.Reg("ui://helloworld/call3", (req, res) =>
            {
                throw new ArgumentNullException("hello");
            });

            router.OnNotFound((req, next) =>
            {
                throwNotFound++;
                next(req);
            });


            router.OnError((req, res, ex, next) =>
            {
                throwError++;
                next(req, res, ex);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                router.Dispatch("ui://helloworld/call");
            });

            Assert.AreEqual(12, throwError);
            Assert.AreEqual(0, throwNotFound);
        }

        [TestMethod]
        public void TestNestedExceptionInterception()
        {
            var router = MakeRouter();
            var throwNotFound = 0; var throwError = 0;

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call2");
            });

            router.Reg("ui://helloworld/call2", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call3");
            }).OnError((req, res, ex, next) =>
            {
                if (throwError == 1)
                {
                    throwError = 10;
                }
            });

            router.Reg("ui://helloworld/call3", (req, res) =>
            {
                throw new ArgumentNullException("hello");
            });

            router.OnNotFound((req, next) =>
            {
                throwNotFound++;
                next(req);
            });


            router.OnError((req, res, ex, next) =>
            {
                throwError++;
                next(req, res, ex);
            });

            ExceptionAssert.DoesNotThrow(() =>
            {
                router.Dispatch("ui://helloworld/call");
            });

            Assert.AreEqual(10, throwError);
            Assert.AreEqual(0, throwNotFound);
        }

        [TestMethod]
        public void TestSimpleNotFoundRouteTest()
        {
            var router = MakeRouter();

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call2");
            });

            ExceptionAssert.Throws<NotFoundRouteException>(() =>
            {
                router.Dispatch("ui://helloworld/call2");
            });
        }

        [TestMethod]
        public void TestNestedNotFoundRouteTest()
        {
            var router = MakeRouter();

            router.Reg("ui://helloworld/call", (req, res) =>
            {
                ExceptionAssert.Throws<NotFoundRouteException>(() =>
                {
                    router.Dispatch("ui://helloworld/call2");
                });
                res.SetContext("helloworld");
            });

            router.Reg("ui://helloworld/call2", (req, res) =>
            {
                router.Dispatch("ui://helloworld/call3");
            });

            var result = router.Dispatch("ui://helloworld/call");

            Assert.AreEqual("helloworld", result.GetContext());
        }
    }
}
