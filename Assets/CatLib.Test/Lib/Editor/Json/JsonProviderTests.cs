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
using CatLib.API.Json;
using CatLib.Events;
using CatLib.Json;
using SimpleJson;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Json
{
    [TestClass]
    public class JsonProviderTests
    {
        /// <summary>
        /// 准备测试环境
        /// </summary>
        /// <returns></returns>
        public IApplication MakeApplication()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new JsonProvider());
            app.Register(new EventsProvider());
            app.Init();
            return app;
        }

        public class DemoClass
        {
            public string Name;

            public Dictionary<string, string> Dict;
        }

        [TestMethod]
        public void TestInvalidSetJson()
        {
            var app = MakeApplication();
            var jsonAware = app.Make<IJsonAware>();
            var json = app.Make<IJson>();

            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                jsonAware.SetJson(json);
            });
        }

        [TestMethod]
        public void TestNotSetJson()
        {
            var jsonUnility = new JsonUtility();
            var demoClass = new DemoClass()
            {
                Name = "helloworld",
                Dict = new Dictionary<string, string>()
                {
                    {"key" , "18" }
                }
            };

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                jsonUnility.Encode(demoClass);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                jsonUnility.Decode<JsonObject>(string.Empty);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                jsonUnility.Decode<DemoClass>(string.Empty);
            });
        }

        [TestMethod]
        public void TestJsonEncodeDecode()
        {
            var app = MakeApplication();
            var json = app.Make<IJson>();
            var demoClass = new DemoClass()
            {
                Name = "helloworld",
                Dict = new Dictionary<string, string>()
                {
                    {"key" , "18" }
                }
            };

            var jsonStr = json.Encode(demoClass);
            var decodeClass = json.Decode<DemoClass>(jsonStr);

            Assert.AreEqual("helloworld", decodeClass.Name);
            Assert.AreEqual("18", decodeClass.Dict["key"]);

            var decodeClassWithObject = json.Decode<JsonObject>(jsonStr);

            Assert.AreNotEqual(null, decodeClassWithObject);
        }
    }
}
