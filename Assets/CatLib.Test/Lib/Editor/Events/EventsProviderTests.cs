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

using CatLib.API.Events;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Events
{
    [TestClass]
    public class EventsProviderTests
    {
        /// <summary>
        /// 生成测试环境
        /// </summary>
        /// <returns></returns>
        private IContainer MakeEnv()
        {
            var app = new Application();

            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Init();

            return app;
        }

        [TestMethod]
        public void TestSimpleOnEvents()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.On("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
            });

            Assert.AreEqual(null, (dispatcher.Trigger("event.name", 123) as object[])[0]);
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestOffEvents()
        {
            var app = MakeEnv();
            var dispatcher = app.Make<IDispatcher>();

            dispatcher.On("event.faild", SimpleCallFunctionNoResult);
            dispatcher.Listen("event.name", SimpleCallFunction);
            dispatcher.Listen("event.name", SimpleCallFunction);
            dispatcher.Off("event.name", SimpleCallFunction);
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name"));
        }

        [TestMethod]
        public void TestOffRegexEvents()
        {
            var app = MakeEnv();
            var dispatcher = app.Make<IDispatcher>();

            dispatcher.On("event.faild", SimpleCallFunctionNoResult);
            dispatcher.Listen("event.*", SimpleCallFunction);
            dispatcher.Listen("event.*", SimpleCallFunction);
            dispatcher.Off("event.*", SimpleCallFunction);
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name"));
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.*"));
        }

        private void SimpleCallFunctionNoResult(object payload)
        {
            Assert.Fail();
        }

        private object SimpleCallFunction(object payload)
        {
            return "SimpleCallFunction";
        }

        [TestMethod]
        public void TestTriggerReturnResult()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.Listen("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });

            var result = dispatcher.Trigger("event.name", 123) as object[];

            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestAsteriskWildcard()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.Listen("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.Listen("event.age", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            var result = dispatcher.Trigger("event.name", 123) as object[];
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(true, isCall);

            result = dispatcher.Trigger("event.age", 123) as object[];
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);
        }

        [TestMethod]
        public void TestHalfTrigger()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestCancelHandler()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            var handler = dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.Listen("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            });

            handler.Off();

            Assert.AreEqual(2, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(false, isCall);
        }

        [TestMethod]
        public void TestLifeCall()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var isCall = false;
            dispatcher.Listen("event.name", (payload) =>
            {
                isCall = true;
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestDepthSelfTrigger()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            var callNum = 0;
            dispatcher.Listen("event.name", (payload) =>
            {
                dispatcher.Trigger("event.name", payload);
                callNum++;
                Assert.AreEqual(123, payload);
                return 1;
            }, 3);

            Assert.AreEqual(1, (dispatcher.Trigger("event.name", 123) as object[]).Length);
            Assert.AreEqual(3, callNum);

            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name", 123));
        }

        [TestMethod]
        public void TestOrderCancel()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.Listen("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);
            dispatcher.Listen("event.name", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            }, 1);
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            }, 1);

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(2, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(3, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name", 123));
        }

        [TestMethod]
        public void TestRepeatOn()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            }, 1);

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(2, dispatcher.TriggerHalt("event.name", 123));
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.name", 123));
        }

        [TestMethod]
        public void TestOtherWildcardOn()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            }, 1);

            dispatcher.Listen("event.call.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            }, 1);

            dispatcher.Listen("event2.call.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 3;
            }, 1);

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.call.name", 123));
            Assert.AreEqual(2, dispatcher.TriggerHalt("event.call.name", 123));
            Assert.AreEqual(null, dispatcher.TriggerHalt("event.call.name", 123));
        }

        [TestMethod]
        public void TestStopBubbling()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            });
            dispatcher.Listen("event.time", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 2;
            });
            dispatcher.Listen("event.time", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return false;
            }, 1);
            dispatcher.Listen("event.time", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 4;
            });

            var results = dispatcher.Trigger("event.time", 123);

            Assert.AreEqual(1, results.Length);
            Assert.AreEqual(2, results[0]);

            results = dispatcher.Trigger("event.time", 123);

            Assert.AreEqual(3, results.Length);
            Assert.AreEqual(2, results[0]);
            Assert.AreEqual(4, results[1]);
            Assert.AreEqual(1, results[2]);
        }

        [TestMethod]
        public void TestHaltNull()
        {
            var app = MakeEnv();

            var dispatcher = app.Make<IDispatcher>();
            dispatcher.Listen("event.*", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return 1;
            });

            dispatcher.Listen("event.time", (payload) =>
            {
                Assert.AreEqual(123, payload);
                return null;
            });

            Assert.AreEqual(1, dispatcher.TriggerHalt("event.time", 123));
        }
    }
}
