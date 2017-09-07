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

using System.Collections.Generic;
using CatLib.API.Translation;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
using CatLib.Translation;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Translation
{
    [TestClass]
    public class TranslationTests
    {
        private Application GetApplication()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new TranslationProvider());
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();
            return app;
        }

        private class BasicTranslateResources : ITranslateResources
        {
            private Dictionary<string, string> dictZh = new Dictionary<string, string>
            {
                { "hello" , "[1,10]zh_hello_1|[11,*]zh_hello_2" },
                { "world" , "zh_world_1" },
                { "helloworld" , "zh_helloworld_1" },
                { "null" , null },
                { "replace" , "zh :start , my name is :name" },
                { "count" , "count is::count" }
            };

            private Dictionary<string, string> dictJp = new Dictionary<string, string>
            {
                { "world" , "jp_world_1" },
                { "null" , null },
                { "replace" , "jp :start , my name is :name" },
                { "count" , "jp count is::count" }
            };

            private Dictionary<string, string> dictEn = new Dictionary<string, string>
            {
                { "hello" , "[1,10]en_hello_1|[11,*]en_hello_2" },
                { "world" , "en_world_1" },
                { "null" , null },
                { "replace" , "en :start , my name is :name" }
            };

            /// <summary>
            /// 获取映射
            /// </summary>
            /// <param name="locale">语言</param>
            /// <param name="key">键</param>
            /// <param name="str">返回的值</param>
            /// <returns>是否成功获取</returns>
            public bool TryGetValue(string locale, string key, out string str)
            {
                if (locale == Languages.English)
                {
                    return dictEn.TryGetValue(key, out str);
                }
                else if (locale == Languages.Japanese)
                {
                    return dictJp.TryGetValue(key, out str);
                }
                return dictZh.TryGetValue(key, out str);
            }
        }

        [TestMethod]
        public void TestBaseGetTranslation()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(Languages.English);

            Assert.AreEqual("[1,10]en_hello_1|[11,*]en_hello_2", translator.Get("hello"));
            Assert.AreEqual("en_hello_1", translator.Get("hello", 5));
            Assert.AreEqual("en_hello_2", translator.Get("hello", 20));
        }

        [TestMethod]
        public void TestBaseGetTranslationFallback()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(Languages.English);

            Assert.AreEqual("zh_helloworld_1", translator.Get("helloworld"));
        }

        [TestMethod]
        public void TestGetByTranslation()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(Languages.Chinese);

            Assert.AreEqual("[1,10]en_hello_1|[11,*]en_hello_2", translator.GetBy("hello", Languages.English));
            Assert.AreEqual("[1,10]en_hello_1|[11,*]en_hello_2", translator.GetBy("hello", new[] { Languages.Japanese, Languages.English, Languages.Chinese }));
        }

        [TestMethod]
        public void TestGetNullTranslation()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(Languages.English);

            Assert.AreEqual(string.Empty, translator.Get("null"));
        }

        [TestMethod]
        public void TestNullLocaleDefiend()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);
            Assert.AreEqual("zh_world_1", translator.Get("world"));
        }

        [TestMethod]
        public void TestNullFallbackDefiend()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(Languages.English);
            Assert.AreEqual(string.Empty, translator.Get("helloworld"));
        }

        [TestMethod]
        public void TestNullLocaleAndFallbackDefiend()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual(string.Empty, translator.Get("hello"));
        }

        [TestMethod]
        public void TestNullLocaleAndFallbackDefiendAndUseGetBy()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual("[1,10]en_hello_1|[11,*]en_hello_2", translator.GetBy("hello", Languages.English));
        }

        [TestMethod]
        public void TestTranslationReplace()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);

            Assert.AreEqual("zh hello , my name is catlib", translator.Get("replace", "start:hello", "name", "catlib"));
        }

        [TestMethod]
        public void TestTranslationReplaceWithGetBy()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);
            Assert.AreEqual("jp hello , my name is catlib", translator.GetBy("replace", new[] { Languages.Japanese, Languages.English }, "start:hello", "name", "catlib"));
        }

        [TestMethod]
        public void TestGetWithNumber()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);

            Assert.AreEqual("count is:10", translator.Get("count", 10));
        }

        [TestMethod]
        public void TestGetByLocalesWithNumber()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);

            Assert.AreEqual("jp count is:10",
                translator.GetBy("count", 10, new[] { Languages.Japanese, Languages.English }));
        }

        [TestMethod]
        public void TestGetByWithNumber()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(null);

            Assert.AreEqual("count is:10",
                translator.GetBy("count", 10, Languages.English));
        }

        [TestMethod]
        public void TestGetLocale()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(Languages.Chinese);
            translator.SetLocale(Languages.English);

            Assert.AreEqual(Languages.English, translator.GetLocale());
        }

        [TestMethod]
        public void TestGetUndefinedWithNumber()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual(string.Empty, translator.Get("undefined", 10));
        }

        [TestMethod]
        public void TestGetByUndefinedWithNumber()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual(string.Empty, translator.GetBy("undefined", 10, new[] { Languages.Afrikaans }));
            Assert.AreEqual(string.Empty, translator.GetBy("undefined", 10, Languages.Afrikaans));
        }

        [TestMethod]
        public void TestGetByUndefinedLocale()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual(string.Empty, translator.GetBy("undefined", Languages.Afrikaans));
        }

        [TestMethod]
        public void TestGetByUndefiendLocales()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetResources(new BasicTranslateResources());
            translator.SetFallback(null);
            translator.SetLocale(null);

            Assert.AreEqual(string.Empty, translator.GetBy("undefined", new[] { Languages.Afrikaans }));
        }

        [TestMethod]
        public void TestUndefiendMapping()
        {
            var app = GetApplication();
            var translator = app.Make<ITranslator>();
            translator.SetFallback(null);
            translator.SetLocale(null);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                translator.Get("undefined");
            });
        }
    }
}
