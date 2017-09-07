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
using CatLib.Config.Locator;
using CatLib.Converters;
using CatLib.Converters.Plan;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Config
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void ConfigTest()
        {
            var converent = new global::CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new global::CatLib.Config.Config(converent, new CodeConfigLocator());
            config.SetConverters(converent);
            config.SetLocator(new CodeConfigLocator());

            Assert.AreEqual(null, config.Get<string>("test"));
            config.Set("test", "test");
            Assert.AreEqual("test", config.Get<string>("test"));

            config.Set("test", "222");
            Assert.AreEqual("222", config.Get<string>("test"));

            config.Save();
        }

        [TestMethod]
        public void TestDefaultIConfig()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();

            Assert.AreSame(app.Make<IConfigManager>().Default, app.Make<IConfig>());
        }

        [TestMethod]
        public void NoLocatorTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                var config = new global::CatLib.Config.Config(new global::CatLib.Converters.Converters(), null);
                config.Set("test", "test");
            });
        }

        [TestMethod]
        public void TestNoConverts()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                var config = new global::CatLib.Config.Config(null, new CodeConfigLocator());
                config.Set("test", "test");
            });
        }

        [TestMethod]
        public void GetUndefinedTest()
        {
            var converent = new global::CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new global::CatLib.Config.Config(converent, new CodeConfigLocator());
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");

            Assert.AreEqual(null, config["222"]);
        }

        [TestMethod]
        public void GetWithUndefinedTypeConverterTest()
        {
            var converent = new global::CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new global::CatLib.Config.Config(converent, new CodeConfigLocator());
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");

            Assert.AreEqual(null, config.Get<ConfigTests>("123"));
        }

        [TestMethod]
        public void ExceptionConverterTest()
        {
            var converent = new global::CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            converent.AddConverter(new StringInt32Converter());
            var config = new global::CatLib.Config.Config(converent, new CodeConfigLocator());
            config.Set("123", "abc");
            Assert.AreEqual(0, config.Get("123", 0));
        }

        /// <summary>
        /// 保存测试
        /// </summary>
        public void SaveTest()
        {
            var converent = new global::CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new global::CatLib.Config.Config(converent, new CodeConfigLocator());
            config.Set("123", "abc");
            config.Save();
        }
    }
}
