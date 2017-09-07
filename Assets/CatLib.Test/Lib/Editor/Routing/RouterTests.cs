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
using CatLib.Config;
using CatLib.Converters;
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
    [TestClass]
    public class RouterTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var app = new Application();
            app.Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(new RoutingProvider());
            app.Register(new EventsProvider());
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            
            //由于熟悉框架流程所以这么写，项目中使用请接受指定事件再生成路由服务
           
            app.On(RouterEvents.OnBeforeRouterAttrCompiler, (payload) =>
            {
                var internalRouter = App.Make<IRouter>();
                internalRouter.Group("default-group").Where("sex", "[0-1]").Defaults("str", "group-str").Middleware(
                    (req, res, next) =>
                    {
                        next(req, res);
                        res.SetContext(res.GetContext() + "[with group middleware]");
                    });

                internalRouter.Group("throw-error-group").OnError((req, res, ex, next) =>
                {
                    //这个组拦截了异常冒泡
                }).Middleware((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with throw error group middleware]");
                });
                internalRouter.Group("DefaultGroup2").Defaults("str", "TestUseGroupAndLocalDefaults");
            });

            app.On(RouterEvents.OnDispatcher, (payload) =>
            {
                var arg = payload as DispatchEventArgs;
                Assert.AreNotEqual(null, arg.Request);
                Assert.AreNotEqual(null, arg.Route);
                Assert.AreNotEqual(string.Empty, (arg.Route as Route).Compiled.ToString());
            });

            app.Init();

            var router = App.Make<IRouter>();
            router.OnError((req, res, ex, next) =>
            {
                if (ex is UndefinedDefaultSchemeException)
                {
                    next(req, res, ex);
                    return;
                }
                Assert.Fail(ex.Message);
            });

            router.OnNotFound((req, next) =>
            {
                throw new NotFoundRouteException("Can not found route.");
            });

            router.Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[global middleware]");
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            App.Handler = null;
        }

        [TestMethod]
        public void SimpleCall()
        {
            var router = App.Make<IRouter>();
            var response = router.Dispatch("catlib://utattr-routing-simple/call");

            Assert.AreEqual("UTAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());
        }

        [TestMethod]
        public void SimpleCallMTest()
        {
            var router = App.Make<IRouter>();
            var response = router.Dispatch("catlib://utattr-routing-simple/call-mtest");

            Assert.AreEqual("UTAttrRoutingSimple.CallMTest[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 多重路由名测试
        /// </summary>
        [TestMethod]
        public void MultAttrRoutingTest()
        {
            var router = App.Make<IRouter>();

            var response = router.Dispatch("utmult-attr-routing-simple/call");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://utmult-attr-routing-simple/call");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://uthello-world/call");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("cat://utmult-attr-routing-simple/call");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://utmult-attr-routing-simple/my-hello");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("catlib://uthello-world/my-hello");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("cat://utmult-attr-routing-simple/my-hello");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("dog://utmyname/call");
            Assert.AreEqual("UTMultAttrRoutingSimple.Call[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 带有中间件的路由请求
        /// </summary>
        [TestMethod]
        public void RoutingMiddlewareTest()
        {
            var router = App.Make<IRouter>();

            var response = router.Dispatch("rm://call");
            Assert.AreEqual("RoutingMiddleware.Call[with middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 参数路由测试
        /// </summary>
        [TestMethod]
        public void ParamsAttrTest()
        {
            var router = App.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.catlib[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 使用路由组的参数路由
        /// </summary>
        [TestMethod]
        public void ParamsAttrWithGroup()
        {
            var router = App.Make<IRouter>();

            var response = router.Dispatch("catlib://params-attr-routing/params-call-with-group/18");
            Assert.AreEqual("ParamsAttrRouting.ParamsCall.18.hello.group-str[with group middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 直接回调测试
        /// </summary>
        [TestMethod]
        public void LambdaCall()
        {
            var router = App.Make<IRouter>();

            router.Reg("lambda://call/lambda-call", (req, res) =>
            {
                res.SetContext("RouterTests.LambdaCall");
            });

            var response = router.Dispatch("lambda://call/lambda-call");
            Assert.AreEqual("RouterTests.LambdaCall[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 引发异常的回调测试
        /// </summary>
        [TestMethod]
        public void ThrowErrorLambdaCall()
        {
            var router = App.Make<IRouter>();

            router.Reg("lambda://call/throw-error-lambda-call", (req, res) =>
            {
                res.SetContext("RouterTests.ThrowErrorLambdaCall");
                throw new Exception("Unit test exception.");
            }).Group("throw-error-group");

            var response = router.Dispatch("lambda://call/throw-error-lambda-call");
            Assert.AreEqual(null, response);
        }

        /// <summary>
        /// 无法找到路由的引发异常测试
        /// </summary>
        [TestMethod]
        public void ThrowNotFoundExceptionCall()
        {
            var router = App.Make<IRouter>();

            ExceptionAssert.Throws<NotFoundRouteException>(() =>
            {
                router.Dispatch("notfound://call");
            });
        }

        /// <summary>
        /// 无法找到路由的引发异常测试(由scheme中引发)
        /// </summary>
        [TestMethod]
        public void ThrowNotFoundExceptionCallInScheme()
        {
            var router = App.Make<IRouter>();
            router.Reg("lambda://call/throw-not-found-exception", (req, res) =>
            {
                res.SetContext("RouterTests.ThrowNotFoundExceptionCallInScheme");
                throw new Exception("Unit test exception.");
            }).Group("throw-error-group");

            ExceptionAssert.Throws<NotFoundRouteException>(() =>
            {
                router.Dispatch("lambda://call/throw-not-found-exception-abc");
            });
        }

        /// <summary>
        /// 路由层的中间件
        /// </summary>
        [TestMethod]
        public void RouteMiddleware()
        {
            var router = App.Make<IRouter>();
            router.Reg("lambda://call/route-middleware", (req, res) =>
            {
                res.SetContext("RouterTests.RouteMiddleware");
            }).Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[route middleware]");
            });

            var response = router.Dispatch("lambda://call/route-middleware");
            Assert.AreEqual("RouterTests.RouteMiddleware[route middleware][global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 路由层异常
        /// </summary>
        [TestMethod]
        public void RouteException()
        {
            var router = App.Make<IRouter>();
            bool isException = false;
            router.Reg("lambda://call/route-exception", (req, res) =>
            {
                res.SetContext("RouterTests.RouteException");
                throw new Exception("Unit test exception.");
            }).Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[route middleware]");
            }).OnError((req, res, ex, next) =>
            {
                isException = true;
            });

            var response = router.Dispatch("lambda://call/route-exception");
            Assert.AreEqual(null, response);
            Assert.AreEqual(true, isException);
        }

        /// <summary>
        /// 大量的路由在一个组中测试
        /// </summary>
        [TestMethod]
        public void MoreRouteWithGroupTest()
        {
            var router = App.Make<IRouter>();

            bool isError = false;
            router.Group(() =>
            {
                router.Reg("lambda://call/more-1/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-1." + req["age"] + "." + req["default"]);
                });
                router.Reg("lambda://call/more-2/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-2." + req["age"] + "." + req["default"]);
                });

                router.Reg("lambda://call/more-error/{age}/{default?}", (req, res) =>
                {
                    res.SetContext("RouterTests.MoreRouteWithGroupTest-3." + req["age"] + "." + req["default"]);
                    throw new Exception("unit test error!");
                });
            }).Defaults("default", "helloworld").Where("age", "[0-9]+").Middleware((req, res, next) =>
            {
                next(req, res);
                res.SetContext(res.GetContext() + "[local group middleware]");
            }).OnError((req, res, ex, next) =>
            {
                isError = true;
            });

            var response = router.Dispatch("lambda://call/more-1/18");
            Assert.AreEqual("RouterTests.MoreRouteWithGroupTest-1.18.helloworld[local group middleware][global middleware]", response.GetContext().ToString());

            response = router.Dispatch("lambda://call/more-2/6/catlib");
            Assert.AreEqual("RouterTests.MoreRouteWithGroupTest-2.6.catlib[local group middleware][global middleware]", response.GetContext().ToString());

            Assert.AreEqual(false, isError);
            response = router.Dispatch("lambda://call/more-error/6/catlib");
            Assert.AreEqual(null, response);
            Assert.AreEqual(true, isError);
        }

        /// <summary>
        /// uri参数绑定
        /// </summary>
        [TestMethod]
        public void QueryParamsBind()
        {
            var router = App.Make<IRouter>();
            router.Reg("lambda://call/query-params-bind/{age}/{default?}", (req, res) =>
            {
                res.SetContext("RouterTests.QueryParamsBind." + req["age"] + "." + req["default"] + req["default2"]);
            });

            var response = router.Dispatch("lambda://call/query-params-bind/16?default=hello&default2=world");
            Assert.AreEqual("RouterTests.QueryParamsBind.16.helloworld[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 可选参数测试
        /// </summary>
        [TestMethod]
        public void OptionsParams()
        {
            var router = App.Make<IRouter>();
            router.Reg("lambda://call/OptionsParams/{age}/{default?}", (req, res) =>
            {
                res.SetContext("RouterTests.OptionsParams." + req["age"] + "." + req["default"]);
            });

            var response = router.Dispatch("lambda://call/OptionsParams/16/");
            Assert.AreEqual("RouterTests.OptionsParams.16.[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("lambda://call/OptionsParams/16");
            Assert.AreEqual("RouterTests.OptionsParams.16.[global middleware]", response.GetContext().ToString());

            response = router.Dispatch("lambda://call/OptionsParams/16/HelloWorld");
            Assert.AreEqual("RouterTests.OptionsParams.16.HelloWorld[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 路由递归调用
        /// </summary>
        [TestMethod]
        public void RoutingRecursiveCall()
        {
            var router = App.Make<IRouter>();
            router.Reg("lambda://call/RoutingRecursiveCall-1", (req, res) =>
            {
                res.SetContext("RouterTests.RoutingRecursiveCall1");
                var re = router.Dispatch("lambda://call/RoutingRecursiveCall-2");
                Assert.AreEqual("RouterTests.RoutingRecursiveCall2[global middleware]", re.GetContext().ToString());
            });

            router.Reg("lambda://call/RoutingRecursiveCall-2", (req, res) =>
            {
                res.SetContext("RouterTests.RoutingRecursiveCall2");
            });

            var response = router.Dispatch("lambda://call/RoutingRecursiveCall-1");
            Assert.AreEqual("RouterTests.RoutingRecursiveCall1[global middleware]", response.GetContext().ToString());
        }

        /// <summary>
        /// 引发循环依赖调用异常
        /// </summary>
        [TestMethod]
        public void RoutingCircularDependencyCall()
        {
            var app = new Application();
            var router = new Router(app, app);

            router.SetDefaultScheme("catlib");

            router.Reg("lambda://call/RoutingCircularDependencyCall-1", (req, res) =>
            {
                res.SetContext("RouterTests.RoutingCircularDependencyCall1");
                var re = router.Dispatch("lambda://call/RoutingCircularDependencyCall-1");
                Assert.AreEqual("RouterTests.RoutingCircularDependencyCall1[global middleware]", re.GetContext().ToString());
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                router.Dispatch("lambda://call/RoutingCircularDependencyCall-1");
            });
        }

        /// <summary>
        /// 可选择接受参数测试
        /// </summary>
        [TestMethod]
        public void OptionsParamsAttrRoutingCall()
        {
            var router = App.Make<IRouter>();

            ExceptionAssert.DoesNotThrow(() =>
            {
                var response = router.Dispatch("options-params-attr-routing/call");
                Assert.AreEqual("[global middleware]", response.GetContext());
            });
        }

        /// <summary>
        /// 可选择接受参数测试（无参数）
        /// </summary>
        [TestMethod]
        public void OptionsParamsAttrRoutingCallNull()
        {
            var router = App.Make<IRouter>();

            ExceptionAssert.DoesNotThrow(() =>
            {
                var response = router.Dispatch("options-params-attr-routing/call-null");
                Assert.AreEqual("[global middleware]", response.GetContext());
            });
        }

        /// <summary>
        /// 可选择接受参数测试（接受来自容器的参数）
        /// </summary>
        [TestMethod]
        public void OptionsParamsAttrRoutingCallResponseAndApp()
        {
            var router = App.Make<IRouter>();
            var app = App.Make<IApplication>();
            ExceptionAssert.DoesNotThrow(() =>
            {
                var response = router.Dispatch("options-params-attr-routing/call-response-and-app");
                Assert.AreEqual(app + "[global middleware]", response.GetContext().ToString());
            });
        }

        /// <summary>
        /// 可选择接受参数测试（接受响应的参数）
        /// </summary>
        [TestMethod]
        public void OptionsParamsAttrRoutingCallResponse()
        {
            var router = App.Make<IRouter>();
            ExceptionAssert.DoesNotThrow(() =>
            {
                var response = router.Dispatch("options-params-attr-routing/call-response");
                Assert.AreEqual("OptionsParamsAttrRouting.CallResponse[global middleware]", response.GetContext().ToString());
            });
        }

        /// <summary>
        /// 带有优先级的中间件测试
        /// </summary>
        [TestMethod]
        public void RoutingPriortityMiddlewareTest()
        {
            var router = App.Make<IRouter>();
            ExceptionAssert.DoesNotThrow(() =>
            {
                var response = router.Dispatch("routing-priortity-middleware/call");
                Assert.AreEqual("RoutingPriortityMiddleware.Call[with middleware 20][with middleware 15][with middleware 10][global middleware]", response.GetContext().ToString());
            });
        }

        [TestMethod]
        public void WrongRouteTest()
        {
            var router = App.Make<IRouter>();

            //变量含有特殊字符将会被降级为字符串
            bool tf = false;
            router.Reg("wrong://hellworld/nihao/{miaomiao*&^$}", (req, res) =>
            {
                tf = true;
            });

            router.Dispatch("wrong://hellworld/nihao/{miaomiao*&^$}");
            Assert.AreEqual(true, tf);
        }

        [TestMethod]
        public void TestUndefindScheme()
        {
            var router = App.Make<IRouter>();
            router.SetDefaultScheme(null);

            ExceptionAssert.Throws<UndefinedDefaultSchemeException>(() =>
            {
                router.Dispatch("TestUndefindScheme/call");
            });

            router.SetDefaultScheme("catlib");
        }

        [TestMethod]
        public void TestFirstCompilerThenAddGroup()
        {
            var router = App.Make<IRouter>();
            router.Group("DefaultGroup").Defaults("str", "TestFirstCompilerThenAddGroup");
            var response = router.Dispatch("routed://first-compiler-then-group");
            Assert.AreEqual("TestFirstCompilerThenAddGroup[global middleware]", response.GetContext());
        }

        [TestMethod]
        public void TestUseGroupAndLocalDefaults()
        {
            var router = App.Make<IRouter>();
            var response = router.Dispatch("routed://use-group-and-local-defaults");
            Assert.AreEqual("hello world[global middleware]", response.GetContext());
        }

        [TestMethod]
        public void TestClassMiddlewareThenRouteMiddleware()
        {
            var router = App.Make<IRouter>();

            router.Group("RoutingMiddleware.ClassMiddlewareThenRouteMiddleTest")
                .Middleware((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[middleware with route group]");
                });

            var response = router.Dispatch("rm://class-middleware-then-route-middle-test");

            Assert.AreEqual("ClassMiddlewareThenRouteMiddleTest[middleware with route group][with middleware][global middleware]", response.GetContext());
        }

        [TestMethod]
        public void TestParamsNameHasString()
        {
            var router = App.Make<IRouter>();
            router.Reg("catlib://test-params-name-has-string/hello{param}/{param2?}", (req, res) =>
            {
                res.SetContext(req.Get("param") + "_" + req.Get("param2"));
            });

            var result = router.Dispatch("catlib://test-params-name-has-string/helloworld/123");
            Assert.AreEqual("world_123[global middleware]", result.GetContext());
        }
    }
}
